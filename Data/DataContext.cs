using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<CourseQuiz> CourseQuizs { get; set; }
        public DbSet<QuestionQuiz> QuestionQuizs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StorageScores> StorageScores { get; set; }
        public DbSet<Room> Room { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}