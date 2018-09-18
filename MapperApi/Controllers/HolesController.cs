/***
 * Filename: HolesController.cs
 * Author  : ebendutoit, tilleyd
 * Class   : HolesController
 *
 *      API entry point for Holes
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Mapper_Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class HolesController : Controller
    {
        private readonly CourseDb _context;

        public HolesController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/courses/{id}/holes
        [Route("api/courses/{cid}/holes")]
        [HttpGet]
        public async Task<IActionResult> GetCourseHoles([FromRoute] Guid cid)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!CourseExists(cid)) {
                return NotFound("The course does not exist");
            }

            var course = await _context.Courses
                    .Include(m => m.Holes)
                    .SingleOrDefaultAsync(m => m.CourseId == cid);

            if (course == null) {
                return NotFound("The course does not exist");
            }

            return Ok(course.Holes);
        }

        // POST: api/courses/{id}/holes
        [Route("api/courses/{cid}/holes")]
        [HttpPost]
        public async Task<IActionResult> PostCourseHole([FromRoute] Guid cid,
                [FromBody] Hole hole)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!CourseExists(cid)) {
                return NotFound("The course does not exist");
            }

            hole.CourseId = cid;

            _context.Holes.Add(hole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHole", new {id = hole.HoleId}, hole);
        }

        // GET: api/holes/{id}
        [Route("api/holes/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetHole([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var hole = await _context.Holes
                    .Include(h => h.Elements)
                    .Select(c => new HoleViewModel(){
                        CourseId = c.CourseId, 
                        HoleId = c.HoleId, 
                        Name = c.Name,
                        Elements = c.Elements.Where( p => p.ElementType == Element.ElementTypes.POINT && p.HoleId == id)
                        .Cast<Point>()
                        .Select(d => new PointViewModel(){
                            CourseId = d.CourseId, 
                            HoleId = d.HoleId,
                            ElementId = d.ElementId,
                            ElementType = d.ElementType, 
                            GeoJson = d.GeoJson, 
                            Info = d.Info,
                            PointType = d.PointType
                        } as ElementViewModel).Concat(
                            c.Elements.Where(q => q.ElementType == Element.ElementTypes.POLYGON && q.HoleId == id)
                            .Cast<Polygon>()
                            .Select(d => new PolygonViewModel(){
                                CourseId = d.CourseId, 
                                ElementId = d.ElementId,
                                HoleId = d.HoleId,
                                ElementType = d.ElementType, 
                                GeoJson = d.GeoJson,  
                                PolygonType = d.PolygonType
                            } as ElementViewModel)
                        ).ToList(),
                    })
                    .SingleOrDefaultAsync(m => m.HoleId == id);

            if (hole == null) return NotFound();

            return Ok(hole);
        }

        // PUT: api/holes/{id}
        [Route("api/holes/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutHole([FromRoute] Guid id,
                [FromBody] Hole hole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != hole.HoleId) return BadRequest();

            _context.Entry(hole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoleExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/holes/{id}
        [Route("api/holes/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHole([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hole = await _context.Holes
                    .SingleOrDefaultAsync(m => m.HoleId == id);

            if (hole == null) return NotFound();

            _context.Holes.Remove(hole);
            await _context.SaveChangesAsync();

            return Ok(hole);
        }

        private bool HoleExists(Guid id)
        {
            return _context.Holes.Any(e => e.HoleId == id);
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
