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
using Mapper_Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Controllers
{
    [Produces("application/json")]
    public class PolygonsController : Controller
    {
        private readonly ZoneDB _context;

        public PolygonsController(ZoneDB context)
        {
            _context = context;
        }

        // GET: api/Zones/{id}/polygons
        [Route("api/Zones/{cid}/polygons")]
        [HttpGet]
        public async Task<IActionResult> GetZonePolygons(
                [FromRoute] Guid cid)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!ZoneExists(cid)) {
                return NotFound("The Zone does not exist");
            }

            var Zone = await _context.Zones
                    .Include(m => m.Elements)
                    .SingleOrDefaultAsync(m => m.ZoneID == cid);

            if (Zone == null) {
                return NotFound("The Zone does not exist");
            }

            var polygons = Zone.Elements.Where(m =>
                    m.ElementType == Element.ElementTypes.POLYGON &&
                    m.ZoneID == null).Cast<Polygon>()
                    .Select( c => new PolygonViewModel(){
                        ZoneID = c.ZoneID, 
                        ElementID = c.ElementId, 
                        ElementType = c.ElementType, 
                        GeoJson = c.GeoJson, 
                        PolygonType = c.PolygonType, 
                    });


            return Ok(polygons);
        }

        // POST: api/Zones/{id}/polygons
        [Route("api/Zones/{cid}/polygons")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostZonePolygon([FromRoute] Guid cid,
                [FromBody] Polygon polygon)
        {
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                return BadRequest(errors);
            }

            if (!ZoneExists(cid)) {
                return NotFound("The Zone does not exist");
            }

            polygon.ElementType = Element.ElementTypes.POLYGON;
            polygon.ZoneID = cid;

            _context.Polygons.Add(polygon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZonePolygon",
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

            PolygonViewModel polygon =
                    await _context.Polygons.Cast<Polygon>()
                    .Select( c => new PolygonViewModel(){
                        ZoneID = c.ZoneID, 
                        ElementID = c.ElementId, 
                        ElementType = c.ElementType, 
                        GeoJson = c.GeoJson, 
                        PolygonType = c.PolygonType, 
                    }).SingleOrDefaultAsync(m =>
                            m.ElementID == id);

            if (polygon == null) {
                return NotFound("The polygon does not exist");
            }

            return Ok(polygon);
        }

        // PUT: api/polygons/{id}
        [Route("api/polygons/{id}")]
        [HttpPut]
        [Authorize]
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
        [Authorize]
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

        private bool ZoneExists(Guid id)
        {
            return _context.Zones.Any(e => e.ZoneID == id);
        }

    }
}
