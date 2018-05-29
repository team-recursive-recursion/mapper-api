/***
 * Filename: GolfCoursesNewController.cs
 * Author  : ebendutoit, tilleyd
 * Class   : CoursesController
 *
 *      API entry point for Golf Courses
 ***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class CoursesController : Controller
    {
        private readonly CourseDb _context;

        public CoursesController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/users/{id}/courses
        [Route("api/users/{uid}/courses")]
        [HttpGet]
        public async Task<IActionResult> GetUserCourses([FromRoute] Guid uid)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(uid)) {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.User
                    .Include(m => m.Courses)
                    .SingleOrDefaultAsync(m => m.UserID == uid);

            if (user == null) return NotFound();

            return Ok(user.Courses);
        }

        // POST: api/users/{id}/courses
        [Route("api/users/{uid}/courses")]
        [HttpPost]
        public async Task<IActionResult> PostUserCourse([FromRoute] Guid uid,
                [FromBody] GolfCourse course)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(uid)) {
                return NotFound();
            }

            course.UserId = uid;

            _context.GolfCourses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGolfCourse",
                    new {id = course.CourseId}, course);
        }

        // GET: api/courses
        [Route("api/courses")]
        [HttpGet]
        public IEnumerable<GolfCourse> GetCourses()
        {
            return _context.GolfCourses;
        }

        // GET: api/courses/{id}
        [Route("api/courses/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCourse([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var golfCourse = await _context.GolfCourses
                    .Include(m => m.Holes)
                    .SingleOrDefaultAsync(m => m.CourseId == id);

            if (golfCourse == null) return NotFound();

            await _context.Entry(golfCourse)
                    .Collection(b => b.CourseElements)
                    .Query()
                    .Where(p => p.HoleId == null)
                    .LoadAsync();

            return Ok(golfCourse);
        }

        // PUT: api/courses/{id}
        [Route("api/courses/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutCourse([FromRoute] Guid id,
                [FromBody] GolfCourse golfCourse)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != golfCourse.CourseId) return BadRequest();

            _context.Entry(golfCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/courses/{id}
        [Route("api/courses/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var golfCourse = await _context.GolfCourses
                    .SingleOrDefaultAsync(m => m.CourseId == id);

            if (golfCourse == null) return NotFound();

            _context.GolfCourses.Remove(golfCourse);
            await _context.SaveChangesAsync();

            return Ok(golfCourse);
        }

        private bool CourseExists(Guid id)
        {
            return _context.GolfCourses.Any(e => e.CourseId == id);
        }

        private bool UserExists(Guid id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
