/***
 * Filename: PointsController.cs
 * Author  : Eben du Toit, Duncan Tilley
 * Class   : PointsController
 *
 *      API endpoint for points.
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
    public class PointsController : Controller
    {
        private readonly CourseDb _context;

        public PointsController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/courses/{id}/points
        [Route("api/courses/{cid}/points")]
        [HttpGet]
        public async Task<IActionResult> GetCoursePoints(
                [FromRoute] Guid cid)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!CourseExists(cid)) {
                return NotFound("The course does not exist");
            }

            var course = await _context.Courses
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.CourseId == cid);

            if (course == null) {
                return NotFound("The course does not exist");
            }

            var points = course.Elements.Where(m =>
                    m.ElementType == Element.ElementTypes.POINT &&
                    m.HoleId == null);

            return Ok(points);
        }

        // POST: api/courses/{id}/points
        [Route("api/courses/{cid}/points")]
        [HttpPost]
        public async Task<IActionResult> PostCoursePoint([FromRoute] Guid cid,
                [FromBody] Point point)
        {
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                return BadRequest(errors);
            }

            if (!CourseExists(cid)) {
                return NotFound("The course does not exist");
            }

            point.ElementType = Element.ElementTypes.POINT;
            point.CourseId = cid;
            point.HoleId = null;

            _context.Points.Add(point);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoursePoint",
                    new {id = point.ElementId}, point);
        }

        // GET: api/holes/{id}/points
        [Route("api/holes/{hid}/points")]
        [HttpGet]
        public async Task<IActionResult> GetHolePoints(
                [FromRoute] Guid hid)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!HoleExists(hid)) {
                return NotFound("The hole does not exist");
            }

            var hole = await _context.Holes
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.HoleId == hid);

            if (hole == null) {
                return NotFound("The hole does not exist");
            }

            var points = hole.Elements.Where(m =>
                    m.ElementType == Element.ElementTypes.POINT);

            return Ok(points);
        }

        // POST: api/holes/{id}/points
        [Route("api/holes/{hid}/points")]
        [HttpPost]
        public async Task<IActionResult> PostHolePoint([FromRoute] Guid hid,
                [FromBody] Point point)
        {
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                return BadRequest(errors);
            }

            if (!HoleExists(hid)) {
                return NotFound("The hole does not exist");
            }

            var hole = await _context.Holes
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.HoleId == hid);

            if (hole == null) {
                return NotFound("The hole does not exist");
            }

            point.ElementType = Element.ElementTypes.POINT;
            point.HoleId = hid;
            point.CourseId = hole.CourseId;

            _context.Points.Add(point);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoursePoint",
                    new {id = point.ElementId}, point);
        }

        // GET: api/points/{id}
        [Route("api/points/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPoint([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var point =
                    await _context.Points.SingleOrDefaultAsync(m =>
                            m.ElementId == id);

            if (point == null) {
                return NotFound("The point does not exist");
            }

            return Ok(point);
        }

        // PUT: api/points/{id}
        [Route("api/points/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutPoint([FromRoute] Guid id,
                [FromBody] Point point)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != point.ElementId) {
                return BadRequest();
            }

            _context.Entry(point).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!PointExists(id)) {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/points/{id}
        [Route("api/points/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePoint(
                [FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var point =
                    await _context.Points.SingleOrDefaultAsync(m =>
                            m.ElementId == id);
            if (point == null) return NotFound();

            _context.Points.Remove(point);
            await _context.SaveChangesAsync();

            return Ok(point);
        }

        private bool PointExists(Guid id)
        {
            return _context.Points.Any(e => e.ElementId == id);
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        private bool HoleExists(Guid id)
        {
            return _context.Holes.Any(e => e.HoleId == id);
        }
    }
}
