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

        public Task<Zone> CreateZone(Zone zone)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserZonesAsync(User user)
        {
            if (user.UserID == null)
            {
                throw new ArgumentException("User id is required");
            }
            user = await context.Users.Where(u => u.UserID == user.UserID)
                .Include(i => (i.Zones).Where(z => z.ParentZoneID == Guid.Empty))
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Password = null;
                user.Token = null;
            }
            else
            {
                throw new ArgumentException("Invalid user provided");
            }
            return user;
        }

        public Task<Zone> LinkZone(Zone parent, Zone child)
        {
            throw new NotImplementedException();
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