using System;
using Mapper_Api.Context;
using Mapper_Api.Helpers;
using Mapper_Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;  

namespace TestSuite.API
{
    public class BaseTest
    {
        // Testable
        protected ICommunicationService CommunicationService;
        protected IElementService ElementService;
        protected IUserService UserService;
        protected IWeatherService WeatherService;
        protected IZoneService ZoneService;
        protected ZoneDB db;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<ZoneDB>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ZoneDB(options);
            ZoneService = new ZoneService(context);
            ElementService = new ElementService(context);
            WeatherService = new WeatherService();
            CommunicationService = new CommunicationService(WeatherService, context);
            IOptions<AppSettings> appSettings = Options.Create<AppSettings>(new AppSettings(){
                Secret = "Testing"
            });
            UserService = new UserService(appSettings, context);

        }
    }
}