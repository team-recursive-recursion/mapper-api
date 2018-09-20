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
using Mapper_Api.Services;
using Mapper_Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class ElementsController : Controller
    {
        private readonly IElementService elementService;

        public ElementsController(IElementService elementService)
        {
            this.elementService = elementService;
        }

        // GET: api/courses/{id}/elements
        // GET: api/holes/{id}/elements
        [Route("api/courses/{ZoneID}/elements")]
        [Route("api/holes/{ZoneID}/elements")]
        [Route("api/Zone/{ZoneID}/elements")]
        [HttpGet]
        public async Task<IActionResult> GetCourseElements([FromRoute] Zone zone)
        {
            try
            {
                return Ok(await elementService.GetElementsAsync(zone));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // GET: api/Zones/{id}/points
        [Route("api/Zones/{ZoneID}/{ElementTypes}")]
        // todo : [Route("api/Zones/{cid}/polygons")]
        [HttpGet]
        public async Task<IActionResult> GetZoneTyped([FromRoute] Zone zone, [FromRoute] Element element)
        {
            try
            {
                return Ok(await elementService.GetElementsAsync(zone, element));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // POST: api/Zones/{id}/points
        [Route("api/Zones/{ZoneID}/polygons")]
        [Route("api/Zones/{ZoneID}/points")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostZonePoint([FromRoute] Zone zone, [FromBody] Element element)
        {
            try
            {
                // return CreatedAtAction("GetZonePoint",
                // new { id = point.ElementId }, point);
                return Ok(await elementService.CreateElementAsync(zone, element));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        // GET: api/points/{id}
        [Route("api/points/{ElementID}")]
        [HttpGet]
        public async Task<IActionResult> GetElement([FromRoute] Element element)
        {
            try
            {
                return Ok(await elementService.GetElementAsync(element));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // GET: api/points/{id}
        [Route("api/polygons/{ElementID}")]
        [Route("api/points/{ElementID}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateElement([FromRoute] Element element)
        {
            try
            {
                return Ok(await elementService.UpdateElementAsync(element));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // DELETE: api/points/{id}
        [Route("api/polygons/{id}")]
        [Route("api/points/{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePoint([FromRoute] Element element)
        {
            try
            {
                return Ok(await elementService.DeleteElementAsync(element));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
