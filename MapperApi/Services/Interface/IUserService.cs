using System.Threading.Tasks;
using Mapper_Api.Models;

namespace Mapper_Api.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        Task<User> CreateUserAsync(User user);
    }
}