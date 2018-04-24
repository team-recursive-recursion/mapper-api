using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Contrib.Wkb;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Newtonsoft.Json;
using SQLitePCL;

namespace Mapper_Api.Services
{
    public class GolfCourseService
    {
        private CourseDb db;

        public GolfCourseService(CourseDb courseDb)
        {
            this.db = courseDb;
        }

        public async Task<GolfCourse> CreateGolfCourse(string courseName)
        {
            GolfCourse course = new GolfCourse
            {
                CourseId = Guid.NewGuid(),
                CourseName = courseName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            ValidationContext validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage, results.First().MemberNames.FirstOrDefault());
            }

            db.GolfCourses.Add(course);
            await db.SaveChangesAsync();
            return course;
        }

        public async Task<GolfCourse> UpdateGolfCourse(Guid courseId, string courseName)
        {
            GolfCourse course = db.GolfCourses.Where(p => p.CourseId == courseId).DefaultIfEmpty(null).First();
            if (course == null)
            {
                throw new ArgumentException("Not a valid course id");
            }

            course.CourseName = courseName;
            ValidationContext validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage,
                    results.First().MemberNames.FirstOrDefault());
            }

            db.GolfCourses.Update(course);
            await db.SaveChangesAsync();
            return course;
        }

        public async Task<CoursePolygon> CreatePolygon(Guid courseId, Guid? holeId,
            CoursePolygon.PolygonTypes polygonType,
            string geoJSONString)
        {
            try
            {
                GeoJSON.Net.Geometry.Polygon polygon =
                    JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Polygon>(geoJSONString);

                var coursePolygon = new CoursePolygon
                {
                    CourseElementID = courseId,
                    HoleID = holeId,
                    PolygonRaw = polygon.ToWkb(),
                    Type = polygonType
                };

                ValidationContext validationContext = new ValidationContext(coursePolygon);

                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon, validationContext, results, true))
                {
                    throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());
                }

                db.CoursePolygons.Update(coursePolygon);

                await db.SaveChangesAsync();
                return coursePolygon;
            }
            catch (JsonSerializationException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (NullReferenceException e)

            {
                throw new ArgumentException("Required argument not set");
            }
        }

        public async Task<Point> CreatePoint(Guid creatorId, Guid courseId, Point.PointTypes pointType)
        {
            throw new NotImplementedException("Create Point");
        }

        public Task<CoursePolygon> UpdatePolygon(Guid polygonID,
            CoursePolygon.PolygonTypes polygonType)
        {
            return UpdatePolygon(polygonID, null, polygonType);
        }

        public Task<CoursePolygon> UpdatePolygon(Guid polygonID, String geoJSONString)
        {
            return UpdatePolygon(polygonID, geoJSONString, null);
        }

        public async Task<CoursePolygon> UpdatePolygon(Guid polygonID, String geoJSONString,
            CoursePolygon.PolygonTypes? polygonType)
        {
            CoursePolygon coursePolygon =
                db.CoursePolygons.Where(polygon => polygon.CourseElementID == polygonID).DefaultIfEmpty(null).First();
            if (coursePolygon == null)
            {
                throw new ArgumentException($"Polygon does not exist with id : {polygonID.ToString()}");
            }

            try
            {
                if (geoJSONString != null)
                {
                    GeoJSON.Net.Geometry.Polygon polygon =
                        JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Polygon>((string) geoJSONString);
                    coursePolygon.PolygonRaw = polygon.ToWkb();
                }

                if (polygonType != null)
                {
                    coursePolygon.Type = (CoursePolygon.PolygonTypes) polygonType;
                }

                ValidationContext validationContext = new ValidationContext(coursePolygon);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon, validationContext, results, true))
                {
                    throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());
                }

                db.CoursePolygons.Update(coursePolygon);
                await db.SaveChangesAsync();
                return coursePolygon;
            }
            catch (JsonSerializationException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException("Required argument not set");
            }
        }

        public async Task<Point> UpdatePoint(Guid creatorId, Guid courseId, Point.PointTypes pointType)
        {
            throw new NotImplementedException("Update Point");
        }

        public async Task<GolfCourse> RemoveGolfCourse(Guid ownerId, string courseName)
        {
            throw new NotImplementedException("Remove Golf Course");
        }

        public async Task<CoursePolygon> RemovePolygon(Guid creatorId, Guid courseId,
            CoursePolygon.PolygonTypes polygonType)
        {
            throw new NotImplementedException("Remove Polygon");
        }

        public async Task<Point> RemovePoint(Guid creatorId, Guid courseId, Point.PointTypes pointType)
        {
            throw new NotImplementedException("Remove Point");
        }

        public IQueryable<GolfCourse> GetGolfCourses()
        {
            return db.GolfCourses;
        }

        public IQueryable<CoursePolygon> GetGolfPolygons()
        {
            return db.CoursePolygons;
        }
    }
}