using System.Collections.Generic;
using System.Threading.Tasks;
using Mapper_Api.Models;

namespace Mapper_Api.Services
{
    public interface IElementService
    {
        Task<Element> GetElementAsync(Element element);
        Task<List<Element>> GetElementsAsync(Zone zone);
        Task<List<Element>> GetElementsAsync(Zone zone, Element element);
        Task<Element> CreateElementAsync(Zone zone, Element element);
        Task<Element> DeleteElementAsync(Element element);
        Task<Element> UpdateElementAsync(Element element);
    }
}