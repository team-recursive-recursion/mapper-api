
using System;
using Mapper_Api;
using Mapper_Api.Controllers;
using Mapper_Api.Models;
using Mapper_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestSuite.API.CousreService
{
    public class ZoneControllerIntegrationtests: BaseTest
    {
        [Fact(DisplayName = "Get_retruns_OkZones_and_Zone")]
        public async void Get_retruns_Ok_result_and_model()
        {
            Zone zone = new Zone()
            {
                ZoneID = Guid.NewGuid()

            };
            // Arrange
            var mockZoneService = new Mock<IZoneService>();
            mockZoneService.Setup(service =>
                service.GetZoneAsync(It.IsAny<Zone>())
                ).ReturnsAsync(zone);

            var sut = new ZonesController(mockZoneService.Object);

            // Act
            var result = await sut.GetZone(zone);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var outputModel =
                Assert.IsType<Zone>(okObjectResult.Value);
        }


        [Fact(DisplayName = "Get_retruns_OkHoles_and_Hole")]
        public async void Get_retruns_Ok_hole()
        {
            Zone zone = new Zone()
            {
                ZoneID = Guid.NewGuid()

            };
            // Arrange
            var mockZoneService = new Mock<IZoneService>();
            mockZoneService.Setup(service =>
                service.GetZoneAsync(It.IsAny<Zone>())
                ).ReturnsAsync(zone);

            var sut = new ZonesController(mockZoneService.Object);

            // Act
            var result = await sut.GetZone(zone);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var outputModel =
                Assert.IsType<Zone>(okObjectResult.Value);
        }

        [Theory(DisplayName = "Get_retruns_OkHoles_and_Hole")]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public async void Get_retruns_Fail_hole(int input)
        {
            Zone zone = new Zone()
            {
                ZoneID = Guid.NewGuid()
            };

            // Arrange
            var mockZoneService = new Mock<IZoneService>();
            mockZoneService.Setup(service =>
                service.GetZoneAsync(It.IsAny<Zone>())
                ).ReturnsAsync(zone);

            var sut = new ZonesController(mockZoneService.Object);

            // Act
            var result = await sut.GetZone(zone);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var outputModel =
                Assert.IsType<Zone>(okObjectResult.Value);
        }
    }
}