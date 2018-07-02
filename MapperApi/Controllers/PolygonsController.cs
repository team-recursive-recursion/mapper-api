/***
 * Filename: PolygonsController.cs
 * Author  : Eben du Toit, Duncan Tilley
 * Class   : PolygonsController
 *
 *      API endpoint for polygons.
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
    public class PolygonsController : Controller
    {
        private readonly CourseDb _context;

        public PolygonsController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/courses/{id}/polygons
        [Route("api/courses/{cid}/polygons")]
        [HttpGet]
        public async Task<IActionResult> GetCoursePolygons(
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

            var polygons = course.Elements.Where(m =>
                    m.ElementType == Element.ElementTypes.POLYGON &&
                    m.HoleId == null);

            return Ok(polygons);
        }

        // POST: api/courses/{id}/polygons
        [Route("api/courses/{cid}/polygons")]
        [HttpPost]
        public async Task<IActionResult> PostCoursePolygon([FromRoute] Guid cid,
                [FromBody] Polygon polygon)
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

            polygon.ElementType = Element.ElementTypes.POLYGON;
            polygon.CourseId = cid;
            polygon.HoleId = null;

            _context.Polygons.Add(polygon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoursePolygon",
                    new {id = polygon.ElementId}, polygon);
        }

        // GET: api/holes/{id}/polygons
        [Route("api/holes/{hid}/polygons")]
        [HttpGet]
        public async Task<IActionResult> GetHolePolygons(
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

            var polygons = hole.Elements.Where(m =>
                    m.ElementType == Element.ElementTypes.POLYGON);

            return Ok(polygons);
        }

        // POST: api/holes/{id}/polygons
        [Route("api/holes/{hid}/polygons")]
        [HttpPost]
        public async Task<IActionResult> PostHolePolygon([FromRoute] Guid hid,
                [FromBody] Polygon polygon)
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

            polygon.ElementType = Element.ElementTypes.POLYGON;
            polygon.HoleId = hid;
            polygon.CourseId = hole.CourseId;

            _context.Polygons.Add(polygon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoursePolygon",
                    new {id = polygon.ElementId}, polygon);
        }

        // GET: api/polygons/{id}
        [Route("api/polygons/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPolygon([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var polygon =
                    await _context.Polygons.SingleOrDefaultAsync(m =>
                            m.ElementId == id);

            if (polygon == null) {
                return NotFound("The polygon does not exist");
            }

            return Ok(polygon);
        }

        // PUT: api/polygons/{id}
        [Route("api/polygons/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutPolygon([FromRoute] Guid id,
                [FromBody] Polygon polygon)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != polygon.ElementId) {
                return BadRequest();
            }

            _context.Entry(polygon).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!PolygonExists(id)) {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/polygons/{id}
        [Route("api/polygons/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePolygon(
                [FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var polygon =
                    await _context.Polygons.SingleOrDefaultAsync(m =>
                            m.ElementId == id);
            if (polygon == null) return NotFound();

            _context.Polygons.Remove(polygon);
            await _context.SaveChangesAsync();

            return Ok(polygon);
        }

        private bool PolygonExists(Guid id)
        {
            return _context.Polygons.Any(e => e.ElementId == id);
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
