using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Services
{
    public class CourseQuizService
    {
        private readonly ICourseQuizRepository<CourseQuiz> _courseQuiz;

        public CourseQuizService(ICourseQuizRepository<CourseQuiz> courseQuiz)
        {
            _courseQuiz = courseQuiz;
        }

        public List<CourseQuiz> GetAll()
        {
            return _courseQuiz.GetAll().ToList();
        }

        public async Task<CourseQuiz> GetById(int id)
        {
            return await _courseQuiz.GetById(id);
        }

        public async Task<CourseQuiz> CreateCourse(CourseQuiz course)
        {
            return await _courseQuiz.CreateCourse(course);
        }

        public async Task UpdateCourse(CourseQuiz newCourse)
        {
            await _courseQuiz.UpdateCourse(newCourse);
        }

        public async Task<bool> DeleteCourse(int id)
        {
            return await _courseQuiz.DeleteCourse(id);
        }

    }
}