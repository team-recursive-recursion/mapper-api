using System.Threading.Tasks;

namespace Mapper_Api.Services
{
    public interface IWeatherService{
        Task<string> GetWeatherInLatLng(double lat, double lng);
    }
}
