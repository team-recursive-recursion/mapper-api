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
        private readonly ZoneDB _context;

        public ElementsController(ZoneDB context)
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

            if (!ZoneExists(cid)) {
                return NotFound("The course does not exist");
            }

            var course = await _context.Zones
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.ZoneID == cid);

            if (course == null) {
                return NotFound("The course does not exist");
            }

            var elements = course.Elements.Where(m => m.ZoneID == null);

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

            if (!ZoneExists(hid)) {
                return NotFound("The hole does not exist");
            }

            var hole = await _context.Zones
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.ZoneID == hid);

            if (hole == null) {
                return NotFound("The hole does not exist");
            }

            return Ok(hole.Elements);
        }

        private bool ZoneExists(Guid id)
        {
            return _context.Zones.Any(e => e.ZoneID == id);
        }
    }
}
