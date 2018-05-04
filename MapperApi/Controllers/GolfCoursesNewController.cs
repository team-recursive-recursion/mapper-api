using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapper_Api.Context;
using Mapper_Api.Models;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    [Route("api/GolfCoursesNew")]
    public class GolfCoursesNewController : Controller
    {
        private readonly CourseDb _context;

        public GolfCoursesNewController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/GolfCoursesNew
        [HttpGet]
        public IEnumerable<GolfCourse> GetGolfCourses()
        {
            return _context.GolfCourses;
        }

        // GET: api/GolfCoursesNew/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGolfCourse([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var golfCourse = await _context.GolfCourses
                .Include(m => m.Holes)
                .SingleOrDefaultAsync(m => m.CourseId == id);    
            
            if (golfCourse == null)
            {
                return NotFound();
            }

            await _context.Entry(golfCourse)
                .Collection(b => b.CourseElements)
                .Query()
                .Where(p => p.Hole == null)
                .LoadAsync();

            return Ok(golfCourse);
        }

        // PUT: api/GolfCoursesNew/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGolfCourse([FromRoute] Guid id, [FromBody] GolfCourse golfCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != golfCourse.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(golfCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GolfCourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GolfCoursesNew
        [HttpPost]
        public async Task<IActionResult> PostGolfCourse([FromBody] GolfCourse golfCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GolfCourses.Add(golfCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGolfCourse", new {id = golfCourse.CourseId}, golfCourse);
        }

        // DELETE: api/GolfCoursesNew/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGolfCourse([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var golfCourse = await _context.GolfCourses
                .SingleOrDefaultAsync(m => m.CourseId == id);

            if (golfCourse == null)
            {
                return NotFound();
            }

            _context.GolfCourses.Remove(golfCourse);
            await _context.SaveChangesAsync();

            return Ok(golfCourse);
        }

        private bool GolfCourseExists(Guid id)
        {
            return _context.GolfCourses.Any(e => e.CourseId == id);
        }
    }
}