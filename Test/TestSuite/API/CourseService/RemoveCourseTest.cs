using System;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService {
    public class RemoveCourseTest : BaseCourseTest {
        [Fact]
        public async Task RemoveCourse_success () {
            //arrange

            //act
            var golfCourse = await courseService.CreateGolfCourse ("test");

            //assert
            Assert.Equal ("test", golfCourse.CourseName);
            var deletedCourse  = await courseService.RemoveGolfCourse (golfCourse.CourseId);
            //assert
            Assert.Equal (golfCourse, deletedCourse);
        }

        [Fact]
        public async Task UpdateCourse_InvalidID_Fail () {
            //arrange
            Guid courseId = Guid.NewGuid ();
            //assert
            var exception = await Assert.ThrowsAsync<ArgumentException> (
                () => courseService.RemoveGolfCourse (courseId)
            );
            //assert
            Assert.Equal ("Not a valid course id", exception.Message);
        }
    }
}