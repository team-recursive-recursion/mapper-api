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
            try
            {
                zone.UserId = user.UserID;
                zone.ParentZoneID = null;
                zone.ZoneID = Guid.NewGuid();
                context.Zones.Add(zone);
                await context.SaveChangesAsync();
                return zone;
            }
            catch (Exception)
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
                    z.ParentZoneID == null &&
                    z.UserId == user.UserID
                ).ToListAsync();
            }
            else
            {
                throw new ArgumentException("Invalid user provided");
            }
        }
        public async Task<List<Zone>> GetZonesAsync()
        {
            return await context.Zones.Where(zn => zn.ParentZoneID == null).ToListAsync();
        }
        public async Task<Zone> GetZoneAsync(Zone zone)
        {
            if (!context.Zones.Any(z => z.ZoneID == zone.ZoneID))
            {
                throw new ArgumentException("Zone does not exist");
            }
            zone = await context.Zones.Where(z => zone.ZoneID == z.ZoneID)
            .Include(ele => ele.Elements)
            .Include(z => z.InnerZones)
            .FirstOrDefaultAsync();
            return zone;
        }

        public async Task UpdateZoneAsync(Zone zone)
        {
            if (!context.Zones.Any(z => z.ZoneID == zone.ZoneID))
            {
                throw new ArgumentException("Invalid zone provided");
            }
            try
            {
                Zone oldZone = await context.Zones.Where(z => z.ZoneID == zone.ZoneID).FirstOrDefaultAsync();
                if (zone.ZoneName != null)
                    oldZone.ZoneName = zone.ZoneName;
                if (zone.Info != null)
                    oldZone.Info = zone.Info;
                if (zone.UserId != null && zone.UserId != Guid.Empty)
                    oldZone.UserId = zone.UserId;
                if (zone.ParentZoneID != null)
                    throw new ArgumentException("Zones need to be reallocated through the link endpoint");
                if (zone.Elements != null)
                    throw new ArgumentException("Elements needs to be changed through the element endpoint");

                context.Zones.Update(oldZone);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("invalid zone update");
            }

        }

        public async Task<Zone> LinkZoneAsync(Zone parent, Zone child, User user)
        {
            if (!context.Zones.Any(zn => zn.ZoneID == parent.ZoneID))
            {
                throw new ArgumentException("Invalid parent zone");
            }

            try
            {
                if (context.Zones.Any(zn => zn.ZoneID == child.ZoneID))
                {
                    child = await context.Zones.Where(zn => zn.ZoneID == parent.ZoneID)
                        .SingleOrDefaultAsync();
                    child.ParentZoneID = parent.ZoneID;
                    child.UserId = user.UserID;
                    context.Zones.Update(child);
                }
                else
                {
                    child.ZoneID = Guid.NewGuid();
                    child.UserId = user.UserID;
                    child.ParentZoneID = parent.ZoneID;
                    context.Zones.Add(child);
                }
                await context.SaveChangesAsync();
                return child;
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid zone");
            }

        }

        public async Task<List<Zone>> GetZonesInZone(Zone parent)
        {
            if (context.Zones.Any(zn => zn.ZoneID == parent.ZoneID))
            {
                try
                {
                    return await context.Zones.Where(zn => zn.ZoneID == parent.ZoneID)
                        .Include(z => z.InnerZones)
                        .ToListAsync();
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid parent zone");
                }
            }
            throw new AggregateException("Invalid parent zone");
        }

        public async Task<Zone> DeleteZoneAsync(Zone zone)
        {
            if (context.Zones.Any(zn => zn.ZoneID == zone.ZoneID))
            {
                zone = await context.Zones.SingleOrDefaultAsync(zn => zn.ZoneID == zone.ZoneID);
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