/***
 * Filename: GolfCourseService.cs
 * Author : ebendutoit
 * Class   : GolfCourseService
 *
 *      Implementation of gold course making use of repository pattern
 ***/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Newtonsoft.Json;
using Point = Mapper_Api.Models.Point;

namespace Mapper_Api.Services
{
    public class GolfCourseService
    {
        private readonly CourseDb _db;

        public GolfCourseService(CourseDb courseDb)
        {
            _db = courseDb;
        }

/***
 * Course ------ CRUD ------
 */
        public async Task<GolfCourse> CreateGolfCourse(string courseName)
        {
            var course = new GolfCourse
            {
                    CourseId = Guid.NewGuid(),
                    CourseName = courseName,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
            };

            var validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results,
                    true))
                throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());

            _db.GolfCourses.Add(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public IQueryable<GolfCourse> GetGolfCourses()
        {
            return _db.GolfCourses;
        }

        public async Task<GolfCourse> UpdateGolfCourse(Guid courseId,
                string courseName)
        {
            var course = _db.GolfCourses.Where(p => p.CourseId == courseId)
                    .DefaultIfEmpty(null).First();
            if (course == null)
                throw new ArgumentException("Not a valid course id");

            course.CourseName = courseName;
            var validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results,
                    true))
                throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());

            _db.GolfCourses.Update(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task<GolfCourse> RemoveGolfCourse(Guid courseId)
        {
            var course = _db.GolfCourses.Where(p => p.CourseId == courseId)
                    .DefaultIfEmpty(null).First();
            if (course == null)
                throw new ArgumentException("Not a valid course id");

            var validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results,
                    true))
                throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());

            _db.GolfCourses.Remove(course);
            await _db.SaveChangesAsync();
            return course;
        }
/***
 * Course ------ CRUD ------
 */


/***
 * Polygon ------ CRUD ------
 */
        public async Task<CoursePolygon> CreatePolygon(Guid courseId,
                Guid? holeId,
                CoursePolygon.PolygonTypes polygonType,
                string geoJsonString)
        {
            try
            {
                var polygon =
                        JsonConvert.DeserializeObject<Polygon>(geoJsonString);

                var coursePolygon = new CoursePolygon
                {
                        CourseId = courseId,
                        HoleId = holeId,
                        GeoJson = geoJsonString,
                        Type = polygonType
                };

                var validationContext = new ValidationContext(coursePolygon);

                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon,
                        validationContext, results, true))
                    throw new ArgumentException(results.First().ErrorMessage,
                            results.First().MemberNames.FirstOrDefault());

                _db.CoursePolygons.Add(coursePolygon);

                await _db.SaveChangesAsync();
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

        public IQueryable<CoursePolygon> GetGolfPolygons()
        {
            return _db.CoursePolygons;
        }

        public Task<CoursePolygon> UpdatePolygon(Guid polygonID,
                CoursePolygon.PolygonTypes polygonType)
        {
            return UpdatePolygon(polygonID, null, polygonType);
        }

        public Task<CoursePolygon> UpdatePolygon(Guid polygonID,
                string geoJSONString)
        {
            return UpdatePolygon(polygonID, geoJSONString, null);
        }

        public async Task<CoursePolygon> UpdatePolygon(Guid polygonID,
                string geoJSONString,
                CoursePolygon.PolygonTypes? polygonType)
        {
            var coursePolygon =
                    _db.CoursePolygons
                            .Where(polygon =>
                                    polygon.CourseElementId == polygonID)
                            .DefaultIfEmpty(null).First();
            if (coursePolygon == null)
                throw new ArgumentException(
                        $"Polygon does not exist with id : {polygonID.ToString()}");

            try
            {
                if (geoJSONString != null)
                    coursePolygon.GeoJson = geoJSONString;

                if (polygonType != null)
                    coursePolygon.Type =
                            (CoursePolygon.PolygonTypes) polygonType;

                var validationContext = new ValidationContext(coursePolygon);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon,
                        validationContext, results, true))
                    throw new ArgumentException(results.First().ErrorMessage,
                            results.First().MemberNames.FirstOrDefault());

                _db.CoursePolygons.Update(coursePolygon);
                await _db.SaveChangesAsync();
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

        public async Task<CoursePolygon> RemovePolygon(Guid creatorId,
                Guid courseId,
                CoursePolygon.PolygonTypes polygonType)
        {
            throw new NotImplementedException("Remove Polygon");
        }
/***
 * Polygon ------ CRUD ------
 */

/***
 * Create Point ------ CRUD ------
 */
        public async Task<Point> CreatePoint(Guid creatorId, Guid courseId,
                Point.PointTypes pointType)
        {
            throw new NotImplementedException("Create Point");
        }

        public async Task<Point> UpdatePoint(Guid creatorId, Guid courseId,
                Point.PointTypes pointType)
        {
            throw new NotImplementedException("Update Point");
        }

        public async Task<Point> RemovePoint(Guid creatorId, Guid courseId,
                Point.PointTypes pointType)
        {
            throw new NotImplementedException("Remove Point");
        }

/***
 * Create Point ------ CRUD ------
 */
    }
}