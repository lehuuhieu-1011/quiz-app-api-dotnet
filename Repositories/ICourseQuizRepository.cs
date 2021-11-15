using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Repositories
{
    public interface ICourseQuizRepository<T>
    {
        List<CourseQuiz> GetAll();
        Task<CourseQuiz> GetById(int id);
        Task<CourseQuiz> CreateCourse(CourseQuiz course);
        Task UpdateCourse(CourseQuiz newCourse);
        Task<bool> DeleteCourse(int id);
    }
}