
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapper_Api.Models;

namespace Mapper_Api.Services
{
    public interface IZoneService
    {
        Task<Zone> CreateZoneAsync(Zone zone, User user);
        Task<Zone> LinkZoneAsync(Zone parent, Zone child, User user);
        Task<List<Zone>> GetZonesInZone(Zone parent);
        Task<List<Zone>> GetUserZonesAsync(User user);
        Task<List<Zone>> GetZonesAsync();
        Task<Zone> GetZoneAsync(Zone zone);
        Task UpdateZoneAsync(Zone zone);
        Task<Zone> DeleteZoneAsync(Zone zone);

        // Task<Element> CreateElementAsync(Element element);
        // Task<Element> UpdateElementAsync(Element element);
        // Task<Zone> MoveElementAsync(Element element, Zone toZone);
    }
}