using System;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService
{
    public class GolfCourseTest : BaseCourseTest
    {
        [Fact]
        public async Task CreateCourses_Input_Success()
        {
            //arrange
            var courseName = "My New Golf Course";

            //act
            var golfCourse = await golfCourseService.CreateGolfCourse(courseName);

            //assert
            Assert.Equal(courseName, golfCourse.CourseName);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CreateCourse_Input_Fail(string courseName)
        {
            //arrange (inline)
            //act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() => golfCourseService.CreateGolfCourse(courseName));
            //assert
            Assert.Equal(nameof(GolfCourse.CourseName), exception.ParamName);
        }

        [Fact]
        public async Task UpdateCourse_Success()
        {
            //arrange
            var courseName = "My Golf Course";
            var newCourseName = "My New Golf Course";

            //act
            var golfCourse = await golfCourseService.CreateGolfCourse(courseName);
            Assert.Equal(courseName, golfCourse.CourseName);
            var updatedGolfCourse = await golfCourseService.UpdateGolfCourse(golfCourse.CourseId, newCourseName);

            // assert
            Assert.Equal(newCourseName, updatedGolfCourse.CourseName);
        }

        [Fact]
        public async Task RemoveCourse_success()
        {
            //arrange

            //act
            var golfCourse = await golfCourseService.CreateGolfCourse("test");
            Assert.Equal("test", golfCourse.CourseName);
            var course = await golfCourseService.RemoveGolfCourse(golfCourse.CourseId);

            //assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => golfCourseService.UpdateGolfCourse(course.CourseId, "updateTest")
            );
            //assert
            Assert.Equal("Not a valid course id", exception.Message);
        }
    }
}