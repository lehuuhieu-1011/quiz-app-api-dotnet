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
        private readonly ICourseQuizRepository<CourseQuiz> _repo;

        public CourseQuizService(ICourseQuizRepository<CourseQuiz> repo)
        {
            _repo = repo;
        }

        public List<CourseQuiz> GetAll()
        {
            return _repo.GetAll().ToList();
        }

        public async Task<CourseQuiz> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<CourseQuiz> CreateCourse(CourseQuiz course)
        {
            return await _repo.CreateCourse(course);
        }

        public async Task<bool> UpdateCourse(CourseQuiz newCourse)
        {
            return await _repo.UpdateCourse(newCourse);
        }

        public async Task<bool> DeleteCourse(int id)
        {
            return await _repo.DeleteCourse(id);
        }

    }
}