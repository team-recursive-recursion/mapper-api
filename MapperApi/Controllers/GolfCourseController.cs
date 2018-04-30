using System;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Mapper_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    public class GolfCourseController : Controller
    {
        private readonly GolfCourseService _service;

        public GolfCourseController(GolfCourseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(string courseName)
        {
            var golfCourse = await _service.CreateGolfCourse(courseName);
            return Ok(golfCourse);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseName(Guid courseId, string courseName)
        {
            try
            {
                var golfCourse = await _service.UpdateGolfCourse(courseId, courseName);
                return Ok(golfCourse);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Invalid parameters");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPolygon(string geoJson, CoursePolygon.PolygonTypes type, Guid courseId)
        {
            try
            {
                var coursePoly = await _service.CreatePolygon(courseId, null, type, geoJson);
                return Ok(coursePoly);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var golfCourses = _service.GetGolfCourses();
            return Ok(await golfCourses.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetPolygons(Guid? courseId = null)
        {
            if (courseId == null)
            {
                return BadRequest("Requires course Id");
            }

            var polyList = Queryable.SelectMany(Queryable.Where(_service.GetGolfCourses(),
                    p => p.CourseId == courseId),
                    u => u.CourseElements);
            return Ok(await polyList.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePolygon(Guid polygonId, CoursePolygon.PolygonTypes? type,
            String geoJson)
        {
            try
            {
                return Ok(await _service.UpdatePolygon(polygonId, geoJson, type));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}