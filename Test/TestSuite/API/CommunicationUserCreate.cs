
using System;
using System.Collections.Generic;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Mapper_Api.Services;
using Moq;
using Newtonsoft.Json;
using Xunit;
using static Mapper_Api.Services.CommunicationService;

namespace TestSuite.API.CousreService
{
    public class CommunicationUser : BaseTest
    {

        [Fact(DisplayName = "Get_retruns_OkHoles_and_Hole")]
        public async void Get_retruns_Success_hole()
        {
            LiveLocationMessage zone = new LiveLocationMessage()
            {
                UserID = null,
                Location = "{\"type\":\"Point\",\"coordinates\":[100.0,0.0]}"
            };

            var query = JsonConvert.SerializeObject(zone);

            LiveLocationMessage liveMessage = new LiveLocationMessage()
            {
                UserID = Guid.NewGuid(),
                Location = "{\"type\":\"Point\",\"coordinates\":[100.0,0.0]}"
            };
            var res = new List<LiveLocationMessage>();
            res.Add(liveMessage);
            // Arrange
            var result = await CommunicationService.interpretInput(query);
            // Assert
            var okObjectResult = Assert.IsType<ReturnMessage>(result);
        }

        [Theory(DisplayName = "Get_retruns_OkHoles_and_Hole")]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public async void Get_retruns_Success_location_second(int input)
        {
            LiveLocationMessage zone = new LiveLocationMessage()
            {
                UserID = null,
                Location = "{\"type\":\"Point\",\"coordinates\":[100.0,0.0]}"
            };

            var query = JsonConvert.SerializeObject(zone);

            LiveLocationMessage liveMessage = new LiveLocationMessage()
            {
                UserID = Guid.NewGuid(),
                Location = "{\"type\":\"Point\",\"coordinates\":[100.0,0.0]}"
            };
            var res = new List<LiveLocationMessage>();
            res.Add(liveMessage);
            // Arrange
            var mockZoneService = new Mock<ICommunicationService>();
            // mockZoneService.Setup(service =>
            //     service.interpretInput(It.IsAny<string>())
            //     ).ReturnsAsync(res);

            // Act
            var result = await CommunicationService.interpretInput(query);
            var query2 = JsonConvert.SerializeObject(result);
            var result2 = await CommunicationService.interpretInput(query);

            // Assert
            var okObjectResult = Assert.IsType<ReturnMessage>(result);
        }

    }
}