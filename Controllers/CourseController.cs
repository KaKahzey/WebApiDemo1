using Demo_01.DAL.Entities;
using Demo_01.DAL.Repositories;
using Demo_01.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private CourseRepository _CourseRepository;

        public CourseController(CourseRepository courseRepository)
        {
            _CourseRepository = courseRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]

        public IActionResult GetCourses()
        {
            IEnumerable<Course> courses = _CourseRepository.GetAll();
            return Ok(courses);
        }

        [HttpGet("{courseId:int:min(1)}")]
        [ProducesResponseType(200, Type = typeof(Course))]

        public IActionResult GetCourseById(int id)
        {
            Course? course = _CourseRepository.GetById(id);
            if(course is null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Course))]

        public IActionResult CreateCourse(Course course)
        {
            Course newCourse = new Course
            {
                Name = course.Name,
                Description = course.Description
            };
            Course courseAdded = _CourseRepository.Create(newCourse);

            return CreatedAtAction(
                nameof(GetCourseById),
                new { courseId = courseAdded.Id },
                courseAdded
                );
        }

        [HttpDelete("{courseId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCourse(int id)
        {
            if (_CourseRepository.Delete(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        //[HttpPut("{courseId:int}")]
        //public IActionResult UpdateCourse(int id, Course course)
        //{
            
        //}
    }
}
