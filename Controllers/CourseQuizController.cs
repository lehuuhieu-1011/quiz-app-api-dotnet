using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api.Controllers
{
    public class CourseQuizController : BaseApiController
    {
        private readonly CourseQuizService _courseQuizService;
        private readonly ICourseQuizRepository<CourseQuiz> _courseQuiz;

        public CourseQuizController(ICourseQuizRepository<CourseQuiz> courseQuiz, CourseQuizService courseQuizService)
        {
            _courseQuizService = courseQuizService;
            _courseQuiz = courseQuiz;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
        {
            return Ok(_courseQuizService.GetAll());
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            CourseQuiz response = await _courseQuizService.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            ResponseCourseQuizModal course = new ResponseCourseQuizModal
            {
                Id = response.Id,
                Image = response.image,
                Name = response.name
            };
            return Ok(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateCourse(ResponseCourseQuizModal newCourse)
        {
            CourseQuiz course = new CourseQuiz
            {
                Id = newCourse.Id,
                name = newCourse.Name,
                image = newCourse.Image,
            };
            CourseQuiz response = await _courseQuizService.CreateCourse(course);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, ResponseCourseQuizModal updateCourse)
        {
            if (id != updateCourse.Id)
            {
                return BadRequest();
            }
            CourseQuiz course = new CourseQuiz
            {
                Id = updateCourse.Id,
                name = updateCourse.Name,
                image = updateCourse.Image,
            };
            bool check = await _courseQuizService.UpdateCourse(course);
            if (!check)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            bool check = await _courseQuizService.DeleteCourse(id);
            if (!check)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}