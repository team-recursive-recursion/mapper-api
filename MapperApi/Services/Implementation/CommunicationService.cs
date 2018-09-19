using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mapper_Api.Services
{
    public class CommunicationService : ICommunicationService
    {
        IWeatherService WeatherService;
        ZoneDB ZoneDB;
        public CommunicationService(IWeatherService WeatherService, ZoneDB ZoneDB)
        {
            this.ZoneDB = ZoneDB;
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
            var result = JsonConvert.SerializeObject(await interpretInput(query));
            return new ArraySegment<Byte>(Encoding.ASCII.GetBytes(result.ToString()));
        }

        private async Task<List<LiveLocationMessage>> interpretInput(string query)
        {
            var newList = new List<LiveLocationMessage>();
            try
            {
                var inputData = JsonConvert.DeserializeObject<LiveLocationMessage>(query);
                LiveUser liveUser = null;
                if (inputData.UserID != null)
                {
                    liveUser = ZoneDB.LiveUser.Where(c => c.UserID == inputData.UserID).SingleOrDefault();
                }

                if (liveUser == null)
                {
                    liveUser = new LiveUser
                    {
                        UserID = new Guid()
                    };

                    ZoneDB.LiveUser.Add(liveUser);
                }
                string additionalInfo = "";

                if (inputData.Location != null)
                {
                    ZoneDB.LiveLocation.Add(new LiveLocation
                    {
                        UserID = liveUser.UserID,
                        GeoJson = inputData.Location
                    });
                    additionalInfo += inputData.Location;
                }

                await ZoneDB.SaveChangesAsync();

                if (inputData.UserID != null)
                {
                    newList.Add(new LiveLocationMessage()
                    {
                        UserID = liveUser.UserID,
                        Location = @"User Sent us info already yay " + additionalInfo
                    });
                }
                else
                {
                    if (inputData.UserID == Guid.Empty)
                    {
                        newList.Add(new LiveLocationMessage()
                        {
                            UserID = liveUser.UserID,
                            Location = "User Created type unknown"
                        });
                    }
                }
            }
            catch (Exception e)
            {
                newList.Add(new LiveLocationMessage()
                {
                    UserID = Guid.Empty,
                    Location = e.Message
                });
            }
            return newList;
        }
    }
}