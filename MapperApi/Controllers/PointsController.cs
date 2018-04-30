﻿using System;
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
    [Route("api/Points")]
    public class PointsController : Controller
    {
        private readonly CourseDb _context;

        public PointsController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/Points
        [HttpGet]
        public IEnumerable<Point> GetPoint()
        {
            return _context.Point;
        }

        // GET: api/Points/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoint([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var point = await _context.Point.SingleOrDefaultAsync(m => m.CourseElementId == id);

            if (point == null)
            {
                return NotFound();
            }

            return Ok(point);
        }

        // PUT: api/Points/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoint([FromRoute] Guid id, [FromBody] Point point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != point.CourseElementId)
            {
                return BadRequest();
            }

            _context.Entry(point).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointExists(id))
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

        // POST: api/Points
        [HttpPost]
        public async Task<IActionResult> PostPoint([FromBody] Point point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Point.Add(point);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoint", new { id = point.CourseElementId }, point);
        }

        // DELETE: api/Points/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoint([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var point = await _context.Point.SingleOrDefaultAsync(m => m.CourseElementId == id);
            if (point == null)
            {
                return NotFound();
            }

            _context.Point.Remove(point);
            await _context.SaveChangesAsync();

            return Ok(point);
        }

        private bool PointExists(Guid id)
        {
            return _context.Point.Any(e => e.CourseElementId == id);
        }
    }
}