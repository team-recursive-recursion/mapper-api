/***
 * Filename: ZonesController.cs
 * Author  : Eben du Toit, Duncan Tilley
 * Class   : ZonesController
 *
 *      API entry point for Zones.
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class ZonesController : Controller
    {
        private readonly IZoneService _context;

        public ZonesController(IZoneService zoneService)
        {
            _context = zoneService;
        }

        // GET: api/users/{id}
        // GET: api/users/{id}/Zones
        [Route("api/users/{UserID}")]
        [Route("api/users/{UserID}/Courses")]
        [HttpGet]
        public async Task<IActionResult> GetGolfCourse([FromRoute]User user)
        {
            try
            {
                return Ok(await _context.GetUserZonesAsync(user));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // POST: api/users/{id}/Zones
        [Route("api/users/{UserID}/Zones")]
        [Route("api/users/{UserID}/Courses")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUserZone([FromRoute] User user, [FromBody] Zone Zone)
        {
            try
            {
                return Ok(await _context.CreateZoneAsync(Zone, user));

            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // GET: api/Zones
        [Route("api/Zones")]
        [Route("api/Courses")]
        [HttpGet]
        public async Task<IActionResult> GetZones()
        {
            return Ok(await _context.GetZonesAsync());
        }

        // GET: api/holes/{id}
        // GET: api/Zones/{id}
        [Route("api/Zones/{ZoneID}")]
        [Route("api/Courses/{ZoneID}")]
        [Route("api/Holes/{ZoneID}")]
        [HttpGet]
        public async Task<IActionResult> GetZone([FromRoute] Zone zone)
        {
            try
            {
                return Ok(await _context.GetZoneAsync(zone));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // PUT: api/Zones/{id}
        [Route("api/Zones/{ZoneID}")]
        [Route("api/Course/{ZoneID}")]
        [Route("api/Holes/{ZoneID}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutZone([FromRoute] Guid id, [FromBody] Zone zone)
        {
            try
            {
                await _context.UpdateZoneAsync(zone);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }

        }

        // DELETE: api/Zones/{id}
        [Route("api/Course/{ZoneID}")]
        [Route("api/Zones/{ZoneID}")]
        [Route("api/Holes/{ZoneID}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteZone([FromRoute] Zone zone)
        {
            try
            {
                return Ok(await _context.DeleteZoneAsync(zone));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // POST: api/courses/{id}/holes
        [Route("api/zones/{ZoneID}/zones")]
        [Route("api/courses/{ZoneID}/holes")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LinkZoneAsync([FromRoute]Zone parent, [FromBody]Zone child)
        {
            try
            {
                return Ok(await _context.LinkZoneAsync(parent, child, new User()
                {
                    UserID = Guid.Parse(User.Identity.Name)
                }));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [Route("api/courses/{ZoneID}/holes")]
        [HttpGet]
        public async Task<IActionResult> GetZonesInZoneAsync([FromRoute]Zone parent)
        {
            try
            {
                return Ok(await _context.GetZonesInZone(parent));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
