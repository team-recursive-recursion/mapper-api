using System;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService
{
    public class CourseTest : BaseCourseTest
    {
        [Fact]
        public async Task CreateCourses_Input_Success()
        {
            //arrange
            var courseName = "My New Golf Course";

            //act
            var golfCourse = await courseService.CreateGolfCourse(courseName);

            //assert
            Assert.Equal(courseName, golfCourse.CourseName);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CreateCourse_WrongDate_Fail(string courseName)
        {
            //arrange (inline)
            //act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() => courseService.CreateGolfCourse(courseName));
            //assert
            Assert.Equal(nameof(Course.CourseName), exception.ParamName);
        }
    }
}