﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using quiz_app_dotnet_api.Data;

namespace quiz_app_dotnet_api.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230326153810_initDb")]
    partial class initDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.CourseQuiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("course_quiz");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.QuestionQuiz", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("answerA")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("answer_a");

                    b.Property<string>("answerB")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("answer_b");

                    b.Property<string>("answerC")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("answer_c");

                    b.Property<string>("answerD")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("answer_d");

                    b.Property<string>("correctAnswer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("correct_answer");

                    b.Property<int>("courseId")
                        .HasColumnType("int")
                        .HasColumnName("course_id");

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<float>("point")
                        .HasColumnType("real")
                        .HasColumnName("point");

                    b.Property<string>("question")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("question");

                    b.HasKey("id");

                    b.HasIndex("courseId");

                    b.ToTable("question_quiz");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.StorageScores", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CourseQuizId")
                        .HasColumnType("int");

                    b.Property<int>("Scores")
                        .HasColumnType("int");

                    b.Property<string>("TimeSubmit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CourseQuizId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("storage_scores");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserName");

                    b.ToTable("user");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.QuestionQuiz", b =>
                {
                    b.HasOne("quiz_app_dotnet_api.Entities.CourseQuiz", "course")
                        .WithMany("QuestionQuiz")
                        .HasForeignKey("courseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("course");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.StorageScores", b =>
                {
                    b.HasOne("quiz_app_dotnet_api.Entities.CourseQuiz", "CourseQuiz")
                        .WithOne("StorageScores")
                        .HasForeignKey("quiz_app_dotnet_api.Entities.StorageScores", "CourseQuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("quiz_app_dotnet_api.Entities.User", "User")
                        .WithMany("Scores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseQuiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.CourseQuiz", b =>
                {
                    b.Navigation("QuestionQuiz");

                    b.Navigation("StorageScores");
                });

            modelBuilder.Entity("quiz_app_dotnet_api.Entities.User", b =>
                {
                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}