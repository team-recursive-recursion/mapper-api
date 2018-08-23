using System;
using System.Threading.Tasks;
using Mapper_Api.Models;
using Xunit;

namespace TestSuite.API.CousreService {
    public class UpdateCourseTest : BaseCourseTest {

        [Fact]
        public async Task UpdateCourse_Success () {
            //arrange
            var courseName = "My Golf Course";
            var newCourseName = "My New Golf Course";

            //act
            var golfCourse = await courseService.CreateGolfCourse (courseName);
            Assert.Equal (courseName, golfCourse.CourseName);
            var updatedGolfCourse = await courseService.UpdateGolfCourse (golfCourse.CourseId, newCourseName);

            // assert
            Assert.Equal (newCourseName, updatedGolfCourse.CourseName);
        }

        [Fact]
        public async Task UpdateCourse_InvalidID_Fail () {
            //arrange
            Guid courseId = Guid.NewGuid ();
            string name = "Invalid Course";
            //assert
            var exception = await Assert.ThrowsAsync<ArgumentException> (
                () => courseService.UpdateGolfCourse (courseId, name)
            );
            //assert
            Assert.Equal ("Not a valid course id", exception.Message);
        }
    }
}