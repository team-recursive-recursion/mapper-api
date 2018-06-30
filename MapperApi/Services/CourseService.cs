/***
 * Filename: CourseService.cs
 * Author : ebendutoit
 * Class   : CourseService
 *
 *      Implementation of gold course making use of repository pattern
 ***/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Newtonsoft.Json;
using Point = Mapper_Api.Models.Point;

namespace Mapper_Api.Services
{
    public class CourseService
    {
        private readonly CourseDb _db;

        public CourseService(CourseDb courseDb)
        {
            _db = courseDb;
        }

/***
 * Course ------ CRUD ------
 */
        public async Task<Course> CreateGolfCourse(string courseName)
        {
            var course = new Course
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

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public IQueryable<Course> GetGolfCourses()
        {
            return _db.Courses;
        }

        public async Task<Course> UpdateGolfCourse(Guid courseId,
                string courseName)
        {
            var course = _db.Courses.Where(p => p.CourseId == courseId)
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

            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task<Course> RemoveGolfCourse(Guid courseId)
        {
            var course = _db.Courses.Where(p => p.CourseId == courseId)
                    .DefaultIfEmpty(null).First();
            if (course == null)
                throw new ArgumentException("Not a valid course id");

            var validationContext = new ValidationContext(course);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(course, validationContext, results,
                    true))
                throw new ArgumentException(results.First().ErrorMessage,
                        results.First().MemberNames.FirstOrDefault());

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return course;
        }
/***
 * Course ------ CRUD ------
 */


/***
 * Polygon ------ CRUD ------
 */
        public async Task<Polygon> CreatePolygon(Guid courseId,
                Guid? holeId,
                Polygon.PolygonTypes polygonType,
                string geoJsonString)
        {
            try
            {
                var polygon =
                        JsonConvert.DeserializeObject<GeoJSON.Net.Geometry.Polygon>(geoJsonString);

                var coursePolygon = new Polygon
                {
                        CourseId = courseId,
                        HoleId = holeId,
                        GeoJson = geoJsonString,
                        PolygonType = polygonType
                };

                var validationContext = new ValidationContext(coursePolygon);

                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon,
                        validationContext, results, true))
                    throw new ArgumentException(results.First().ErrorMessage,
                            results.First().MemberNames.FirstOrDefault());

                _db.Polygons.Add(coursePolygon);

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

        public IQueryable<Polygon> GetGolfPolygons()
        {
            return _db.Polygons;
        }

        public Task<Polygon> UpdatePolygon(Guid polygonID,
                Polygon.PolygonTypes polygonType)
        {
            return UpdatePolygon(polygonID, null, polygonType);
        }

        public Task<Polygon> UpdatePolygon(Guid polygonID,
                string geoJSONString)
        {
            return UpdatePolygon(polygonID, geoJSONString, null);
        }

        public async Task<Polygon> UpdatePolygon(Guid polygonID,
                string geoJSONString,
                Polygon.PolygonTypes? polygonType)
        {
            var coursePolygon =
                    _db.Polygons
                            .Where(polygon =>
                                    polygon.ElementId == polygonID)
                            .DefaultIfEmpty(null).First();
            if (coursePolygon == null)
                throw new ArgumentException(
                        $"Polygon does not exist with id : {polygonID.ToString()}");

            try
            {
                if (geoJSONString != null)
                    coursePolygon.GeoJson = geoJSONString;

                if (polygonType != null)
                    coursePolygon.PolygonType =
                            (Polygon.PolygonTypes) polygonType;

                var validationContext = new ValidationContext(coursePolygon);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(coursePolygon,
                        validationContext, results, true))
                    throw new ArgumentException(results.First().ErrorMessage,
                            results.First().MemberNames.FirstOrDefault());

                _db.Polygons.Update(coursePolygon);
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

        public async Task<Polygon> RemovePolygon(Guid creatorId,
                Guid courseId,
                Polygon.PolygonTypes polygonType)
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
