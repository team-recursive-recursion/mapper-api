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

        public async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                byte[] data = new byte[result.Count];
                Array.Copy(buffer, data, result.Count);
                var response = await generateResponse(data);
                await webSocket.SendAsync(response, result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task<ArraySegment<byte>> generateResponse(byte[] input)
        {
            string query = Encoding.ASCII.GetString(input);
            // List<Message> messages = interpretInput(query);
            var result = JsonConvert.SerializeObject(await WeatherService.GetWeatherInLatLng(10f,10f));
            return new ArraySegment<Byte>(Encoding.ASCII.GetBytes(result.ToString()));
        }

        private static List<Message> interpretInput(string query)
        {
            var inputList = (List<Message>)JsonConvert.DeserializeObject(query);
            var newList  = new List<Message>();
            return inputList;
        }

        public class Message {
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