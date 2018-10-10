
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
    public class UserControllerIntegrationTests : BaseTest
    {
        [Fact(DisplayName = "Auth_retruns_OkUser_and_User")]
        public async void Auth_retruns_Ok_user()
        {
            User user = new User()
            {
                UserID = Guid.NewGuid(),
                Email = "Email@mail.com",
                Name = "Name",
                Password = "password"
            };
            Zone zone = new Zone()
            {
                ZoneID = Guid.NewGuid()

            };
            // Arrange
            var mockZoneService = new Mock<IZoneService>();
            mockZoneService.Setup(service =>
                service.GetZoneAsync(It.IsAny<Zone>())
                ).ReturnsAsync(zone);
            // Arrange
            var mockUser = new Mock<IUserService>();
            mockUser.Setup(service =>
                service.CreateUserAsync(It.IsAny<User>())
                ).ReturnsAsync(user);

            // var sut = new UsersController(mockUser.Object, mockZoneService.Object,);

            // Act
            // var result = await sut.Create(user);

            // Assert
            // var okObjectResult = Assert.IsType<CreatedAtActionResult>(result);
            // var outputModel =
            //     Assert.IsType<User>(okObjectResult.Value);
        }


        [Fact(DisplayName = "Auth_retruns_OkUser_and_User")]
        public async void Auth_retruns_Ok_user_auth()
        {
            User user = new User()
            {
                UserID = Guid.NewGuid(),
                Email = "Email@mail.com",
                Name = "Name",
                Password = "password"
            };
            Zone zone = new Zone()
            {
                ZoneID = Guid.NewGuid()

            };
            // Arrange
            var mockZoneService = new Mock<IZoneService>();
            mockZoneService.Setup(service =>
                service.GetZoneAsync(It.IsAny<Zone>())
                ).ReturnsAsync(zone);
            // Arrange
            var mockUser = new Mock<IUserService>();
            mockUser.Setup(service =>
                service.Authenticate(It.IsAny<string>(), It.IsAny<string>())
                ).ReturnsAsync(user);

            // var sut = new UsersController(mockUser.Object, mockZoneService.Object);

            // Act
            // var result = await sut.Authenticate(user);

            // // Assert
            // var okObjectResult = Assert.IsType<OkObjectResult>(result);
            // var outputModel =
            //     Assert.IsType<User>(okObjectResult.Value);
        }

    }
}