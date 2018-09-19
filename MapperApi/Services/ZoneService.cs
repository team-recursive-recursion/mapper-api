using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Mapper_Api.Services
{
    public class ZoneService : IZoneService
    {
        private ZoneDB context;

        public ZoneService(ZoneDB context)
        {
            this.context = context;
        }

        public Task<Element> CreateElement(Element element)
        {
            throw new NotImplementedException();
        }

        public async Task<Zone> CreateZoneAsync(Zone zone, User user)
        {
            if (user.UserID == null || !context.Users.Any(usr => usr.UserID == user.UserID))
            {
                throw new ArgumentException("Invalid user provided");
            }
            zone.UserId = user.UserID;
            try
            {
                context.Zones.Add(zone);
                await context.SaveChangesAsync();
                return zone;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid Zone provided");
            }
        }

        public async Task<List<Zone>> GetUserZonesAsync(User user)
        {
            if (user.UserID == null)
            {
                throw new ArgumentException("User id is required");
            }
            if (context.Users.Any(u => u.UserID == user.UserID))
            {
                return await context.Zones
                .Where(z =>
                    z.ParentZoneID == Guid.Empty &&
                    z.UserId == user.UserID
                )
                .ToListAsync();
            }
            else
            {
                throw new ArgumentException("Invalid user provided");
            }
        }
        public async Task<List<Zone>> GetZonesAsync()
        {
            return await context.Zones.Where(zn => zn.ParentZoneID == Guid.Empty).ToListAsync();
        }
        public async Task<Zone> GetZoneAsync(Zone zone)
        {
            if (!context.Zones.Any(z => z.ZoneID == zone.ZoneID))
            {
                throw new ArgumentException("Zone does not exist");
            }
            return await context.Zones.Where(z => zone.ZoneID == z.ZoneID)
            .Include(ele => ele.Elements)
            .Include(z => z.InnerZones)
            .ThenInclude(el => el.Elements).FirstOrDefaultAsync();
        }

        public async Task UpdateZoneAsync(Zone zone)
        {
            if (!context.Zones.Any(z => z.ZoneID == zone.ZoneID))
            {
                throw new ArgumentException("Invalid zone provided");
            }
            try
            {
                context.Entry(zone).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new ArgumentException("invalid zone update");
            }

        }

        //         public Task<Zone> LinkZone(Zone parent, Zone child)
        public async Task<Zone> DeleteZoneAsync(Zone zone)
        {
            if (context.Zones.Any(zn => zn.ZoneID == zone.ZoneID))
            {
                zone = await context.Zones.SingleOrDefaultAsync( zn => zn.ZoneID == zone.ZoneID);
                context.Zones.Remove(zone);
                await context.SaveChangesAsync();
                return zone;
            }
            throw new ArgumentException("Invalid zone");
        }
        public Task<Zone> MoveElement(Element element, Zone toZone)
        {
            throw new NotImplementedException();
        }

        public Task<Element> UpdateElement(Element element)
        {
            throw new NotImplementedException();
        }
    }
}