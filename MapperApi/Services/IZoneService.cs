
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapper_Api.Models;

namespace Mapper_Api.Services
{
    public interface IZoneService
    {
        Task<Zone> CreateZone(Zone zone);
        Task<Zone> LinkZone(Zone parent, Zone child);
        Task<User> GetUserZonesAsync(User user);

        Task<Element> CreateElement(Element element);
        Task<Element> UpdateElement(Element element);
        Task<Zone> MoveElement(Element element, Zone toZone);
    }
}