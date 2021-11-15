using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Repositories
{
    public class CourseQuizRepository : ICourseQuizRepository<CourseQuiz>
    {
        DataContext _context;

        public CourseQuizRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CourseQuiz> CreateCourse(CourseQuiz course)
        {
            await _context.CourseQuizs.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            CourseQuiz course = await _context.CourseQuizs.FindAsync(id);
            if (course == null)
            {
                return false;
            }
            _context.CourseQuizs.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public List<CourseQuiz> GetAll()
        {
            return _context.CourseQuizs.ToList();
        }

        public async Task<CourseQuiz> GetById(int id)
        {
            CourseQuiz course = await _context.CourseQuizs.FindAsync(id);
            if (course == null)
            {
                return null;
            }
            return course;
        }

        public async Task UpdateCourse(CourseQuiz newCourse)
        {
            _context.CourseQuizs.Update(newCourse);
            await _context.SaveChangesAsync();
        }
    }
}