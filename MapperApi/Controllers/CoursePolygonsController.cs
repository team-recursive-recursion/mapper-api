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
    [Route("api/CoursePolygons")]
    public class CoursePolygonsController : Controller
    {
        private readonly CourseDb _context;

        public CoursePolygonsController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/CoursePolygons
        [HttpGet]
        public IEnumerable<CoursePolygon> GetCoursePolygons()
        {
            return _context.CoursePolygons;
        }

        // GET: api/CoursePolygons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoursePolygon([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coursePolygon = await _context.CoursePolygons.SingleOrDefaultAsync(m => m.CourseElementId == id);

            if (coursePolygon == null)
            {
                return NotFound();
            }

            return Ok(coursePolygon);
        }

        // PUT: api/CoursePolygons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoursePolygon([FromRoute] Guid id, [FromBody] CoursePolygon coursePolygon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coursePolygon.CourseElementId)
            {
                return BadRequest();
            }

            _context.Entry(coursePolygon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursePolygonExists(id))
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

        // POST: api/CoursePolygons
        [HttpPost]
        public async Task<IActionResult> PostCoursePolygon([FromBody] CoursePolygon coursePolygon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CoursePolygons.Add(coursePolygon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoursePolygon", new { id = coursePolygon.CourseElementId }, coursePolygon);
        }

        // DELETE: api/CoursePolygons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoursePolygon([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coursePolygon = await _context.CoursePolygons.SingleOrDefaultAsync(m => m.CourseElementId == id);
            if (coursePolygon == null)
            {
                return NotFound();
            }

            _context.CoursePolygons.Remove(coursePolygon);
            await _context.SaveChangesAsync();

            return Ok(coursePolygon);
        }

        private bool CoursePolygonExists(Guid id)
        {
            return _context.CoursePolygons.Any(e => e.CourseElementId == id);
        }
    }
}