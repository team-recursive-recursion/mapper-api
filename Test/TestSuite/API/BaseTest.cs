using System;
using Mapper_Api.Context;
using Mapper_Api.Services;
using Microsoft.EntityFrameworkCore;

namespace TestSuite.API
{
    public class BaseTest
    {
        // Testable
        protected IZoneService ZoneService;
        protected ZoneDB db;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<ZoneDB>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ZoneDB(options);
            ZoneService = new ZoneService(context);
        }
    }
}