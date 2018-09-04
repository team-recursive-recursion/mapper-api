/***
 * Filename: ElementsController.cs
 * Author  : Duncan Tilley
 * Class   : ElementsController
 *
 *      API endpoint for elements.
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
    public class ElementsController : Controller
    {
        private readonly CourseDb _context;

        public ElementsController(CourseDb context)
        {
            _context = context;
        }

        // GET: api/courses/{id}/elements
        [Route("api/courses/{cid}/elements")]
        [HttpGet]
        public async Task<IActionResult> GetCourseElements(
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

            var elements = course.Elements.Where(m => m.HoleId == null);

            return Ok(elements);
        }

        // GET: api/holes/{id}/elements
        [Route("api/holes/{hid}/elements")]
        [HttpGet]
        public async Task<IActionResult> GetHoleElements(
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

            return Ok(hole.Elements);
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
