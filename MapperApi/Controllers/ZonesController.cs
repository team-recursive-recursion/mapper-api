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
using Mapper_Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class ZonesController : Controller
    {
        private readonly ZoneDB _context;

        public ZonesController(ZoneDB context)
        {
            _context = context;
        }

        // GET: api/users/{id}/Zones
        [Route("api/users/{uid}/Zones")]
        [HttpGet]
        public async Task<IActionResult> GetUserZones([FromRoute] Guid uid)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(uid)) {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Users
                    .Include(m => m.Zones)
                    .SingleOrDefaultAsync(m => m.UserID == uid);

            if (user == null) return NotFound();

            return Ok(user.Zones);
        }

        // POST: api/users/{id}/Zones

        [Route("api/users/{uid}/Zones")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostUserZone([FromRoute] Guid uid,
                [FromBody] Zone Zone)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!UserExists(uid)) {
                return NotFound();
            }

            Zone.UserId = uid;

            _context.Zones.Add(Zone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGolfZone",
                    new {id = Zone.ZoneID}, Zone);
        }

        // GET: api/Zones
        [Route("api/Zones")]
        [HttpGet]
        public IEnumerable<Zone>GetZones()
        {
            return _context.Zones;
        }

        // GET: api/Zones/{id}
        [Route("api/Zones/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetZone([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var golfZone = await _context.Zones
                    .Include(m => m.InnerZones)
                    .Select(c => new CourseViewModel()
                    {
                        ZoneID = c.ZoneID,
                        ZoneName = c.ZoneName,
                        Elements = c.Elements.Where(p => p.ElementType == Element.ElementTypes.POINT && p.ZoneID == null)
                        .Cast<Point>()
                        .Select(d => new PointViewModel() {
                            ZoneID = d.ZoneID,
                            ElementID = d.ElementId,
                            ElementType = d.ElementType,
                            GeoJson = d.GeoJson,
                            Info = d.Info,
                            PointType = d.PointType, 
                        } as ElementViewModel
                        ).Concat(
                            c.Elements.Where(q => q.ElementType == Element.ElementTypes.POLYGON && q.ZoneID == null)
                            .Cast<Polygon>()
                            .Select(d => new PolygonViewModel() {
                                ZoneID = d.ZoneID,
                                ElementID = d.ElementId,
                                ElementType = d.ElementType,
                                GeoJson = d.GeoJson,
                                PolygonType = d.PolygonType,
                            } as ElementViewModel
                            )
                        ).ToList(),
                        UserId = c.UserId,
                        InnerZones = c.InnerZones, 
                        Info = c.Info
                    })
                    .SingleOrDefaultAsync(m => m.ZoneID == id);

            if (golfZone == null) return NotFound();

            return Ok(golfZone);
        }

        // PUT: api/Zones/{id}
        [Route("api/Zones/{id}")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutZone([FromRoute] Guid id,
                [FromBody] Zone golfZone)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != golfZone.ZoneID) return BadRequest();

            _context.Entry(golfZone).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZoneExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Zones/{id}
        [Route("api/Zones/{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteZone([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var golfZone = await _context.Zones
                    .SingleOrDefaultAsync(m => m.ZoneID == id);

            if (golfZone == null) return NotFound();

            _context.Zones.Remove(golfZone);
            await _context.SaveChangesAsync();

            return Ok(golfZone);
        }

        private bool ZoneExists(Guid id)
        {
            return _context.Zones.Any(e => e.ZoneID == id);
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
