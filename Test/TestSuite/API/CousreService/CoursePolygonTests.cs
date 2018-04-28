using System;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService
{
    public class CoursePolygon : BaseCourseTest
    {
        [Fact]
        public async Task CreatePoly_Valid_Success()
        {
            //arrange
            string jsonStringValid =
                "{\"type\": \"Polygon\",\"coordinates\": [[[-64.73, 32.31],[-80.19, 25.76],[-66.09, 18.43],[-64.73, 32.31]]]}";
            GolfCourse course = await golfCourseService.CreateGolfCourse("testCourse");
            var polygonType = Mapper_Api.Models.CoursePolygon.PolygonTypes.BUNKER;
            //act
            var polygon = await golfCourseService.CreatePolygon(course.CourseId, null, polygonType, jsonStringValid);
            //assert
            Assert.Equal(polygonType, polygon.Type);
            Assert.Equal(course.CourseId, polygon.CourseElementID);
        }

        [Theory]
        [InlineData("", 1)]
        [InlineData(" ", -1)]
        [InlineData(
            "{\"tpe\": \"Polygon\",\"coordinates\": [[[-64.73, 32.31],[-80.19, 25.76],[-66.09, 18.43],[-64.73, 32.31]]]}",
            1)]
        [InlineData(
            "{\"type\": \"Pogon\",\"coordinates\": [[[-64.73, 32.31],[-80.19, 25.76],[-66.09, 18.43],[-64.73, 32.31]]]}",
            -1)]
        [InlineData(
            "{\"type\": \"Pogon\",\"coordinates\": [[[-64.73,[-80.19, 25.76],[-66.09, 18.43],[-64.73, 32.31]]]}", -1)]
        public async Task CreateCourse_Input_Fail(string polygon, int type)
        {
            // arrange (inline)
            Guid courseID = Guid.NewGuid();
            // act
            // assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                golfCourseService.CreatePolygon(courseID, null, (Mapper_Api.Models.CoursePolygon.PolygonTypes) type,
                    polygon));
        }
    }
}