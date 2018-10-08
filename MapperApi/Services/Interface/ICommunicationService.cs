using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mapper_Api.Services
{
    public interface ICommunicationService
    {
        Task SocketHandler(HttpContext context, WebSocket webSocket);
        Task<List<LiveLocationMessage>> interpretInput(string query);
    }
}