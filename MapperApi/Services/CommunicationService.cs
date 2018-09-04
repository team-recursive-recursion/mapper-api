using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mapper_Api.Services
{
    public class CommunicationService
    {
        WeatherService WeatherService;
        public CommunicationService(WeatherService WeatherService)
        {
            this.WeatherService = WeatherService;
        }
        
        public async Task SocketHandler(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                byte[] data = new byte[result.Count];
                Array.Copy(buffer, data, result.Count);
                var response = await GenerateResponse(data);
                await webSocket.SendAsync(response, result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task<ArraySegment<byte>> GenerateResponse(byte[] input)
        {
            string query = Encoding.ASCII.GetString(input);
            var result = JsonConvert.SerializeObject(interpretInput(query));
            return new ArraySegment<Byte>(Encoding.ASCII.GetBytes(result.ToString()));
        }

        private async Task<List<Message>> interpretInput(string query)
        {
            var newList = new List<Message>();
            try
            {
                var inputData = JsonConvert.DeserializeObject<LiveLocationMessage>(query);
                if (inputData.UserID != null)
                {
                    newList.Add(new Message()
                    {
                        MessageType = Message.Type.INFORMATION,
                        Description = "-- Its name is --",
                        Payload = "It exists"
                    });
                }
                else
                {
                    if (inputData.Device == LiveLocationMessage.DeviceType.MONITOR)
                    {
                        newList.Add(new Message()
                        {
                            MessageType = Message.Type.INFORMATION,
                            Description = "-- Its name is --",
                            Payload = "Monitor"
                        });
                    }
                    if (inputData.Device == LiveLocationMessage.DeviceType.APPLICATION)
                    {
                        newList.Add(new Message()
                        {
                            MessageType = Message.Type.INFORMATION,
                            Description = "-- Its name is --",
                            Payload = "Application"
                        });
                    }
                    if (inputData.Device == null && inputData.UserID == Guid.Empty)
                    {
                        newList.Add(new Message()
                        {
                            MessageType = Message.Type.INFORMATION,
                            Description = "-- Its name is --",
                            Payload = "Error"
                        });
                    }
                }
            }
            catch (Exception e)
            {
                newList.Add(new Message()
                {
                    MessageType = Message.Type.INFORMATION,
                    Description = "-- Its name is --",
                    Payload = "Error"
                });
            }
            return newList;
        }
        public class Message
        {
            public enum Type
            {
                WARNING, INFORMATION, LOCATION
            }
            public Type MessageType { get; set; }
            public string Description { get; set; }
            public string Payload { get; set; }
            public Message MyMessage { get; set; }
        }
    }
}