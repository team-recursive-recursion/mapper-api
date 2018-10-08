using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static Mapper_Api.Services.CommunicationService;

namespace Mapper_Api.Services
{
    public interface ICommunicationService
    {
        Task SocketHandler(HttpContext context, WebSocket webSocket);
        Task<ReturnMessage> interpretInput(string query);
    }
}