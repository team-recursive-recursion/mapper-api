
using System;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService {
    public class RemoveCourseTest : BaseTest {

        [Fact]
        public async void CreateZone_Valid_Success()
        {
        //Given

        string zoneName = "ZoneName";
        string info = null;

        Zone zone = new Zone () {
            ZoneName = zoneName
        };

        //When
        zone = await ZoneService.CreateZone(zone);

        //Then
        Assert.Equal(zone.Info, info);
        Assert.Equal(zone.ZoneName, zoneName);
        Assert.Equal(zone.ParentZoneID, Guid.Empty);
        Assert.NotEqual(zone.ZoneID, Guid.Empty);
        Assert.NotEqual(zone.UserId, Guid.Empty);
        }
    }
}