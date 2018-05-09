/***
 * Filename: HolesController.cs
 * Author : ebendutoit
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Holes")]
    public class HolesController : Controller
    {
        private readonly CourseDb _context;

        public HolesController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/Holes
        [HttpGet]
        public IEnumerable<Hole> GetHole()
        {
            return _context.Hole;
        }

        // GET: api/Holes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHole([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hole = await _context.Hole
                    .Include(h => h.CourseElements)
                    .SingleOrDefaultAsync(m => m.HoleID == id);

            if (hole == null) return NotFound();

            return Ok(hole);
        }

        // PUT: api/Holes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHole([FromRoute] Guid id,
                [FromBody] Hole hole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != hole.HoleID) return BadRequest();

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

        // POST: api/Holes
        [HttpPost]
        public async Task<IActionResult> PostHole([FromBody] Hole hole)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Hole.Add(hole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHole", new {id = hole.HoleID}, hole);
        }

        // DELETE: api/Holes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHole([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hole = await _context.Hole
                    .SingleOrDefaultAsync(m => m.HoleID == id);

            if (hole == null) return NotFound();

            _context.Hole.Remove(hole);
            await _context.SaveChangesAsync();

            return Ok(hole);
        }

        private bool HoleExists(Guid id)
        {
            return _context.Hole.Any(e => e.HoleID == id);
        }
    }
}