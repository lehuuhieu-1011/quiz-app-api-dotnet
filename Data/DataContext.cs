using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CourseQuiz> CourseQuizs { get; set; }
        public DbSet<QuestionQuiz> QuestionQuizs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}