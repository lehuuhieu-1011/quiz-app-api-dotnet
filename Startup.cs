using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MimeKit.Cryptography;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Mail;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "quiz_app_dotnet_api", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options => options.UseSqlServer(_config.GetConnectionString("DbConnection")));
            services.AddTransient<ICourseQuizRepository<CourseQuiz>, CourseQuizRepository>();
            services.AddTransient<CourseQuizService, CourseQuizService>();
            services.AddTransient<IQuestionQuizRepository<QuestionQuiz>, QuestionQuizRepository>();
            services.AddTransient<QuestionQuizService, QuestionQuizService>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // truy cap IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                // thiet lap ve password
                options.Password.RequireDigit = false; //khong bat buoc phai co so
                options.Password.RequireLowercase = false; // khong bat buoc phai co chu thuong
                options.Password.RequireNonAlphanumeric = false; // khong bat ky tu dat biet
                options.Password.RequireUppercase = false; // khong bat buoc chu in
                options.Password.RequiredLength = 6; // so ky tu toi thieu cua password
                options.Password.RequiredUniqueChars = 1; // so ky tu rieng biet

                // cau hinh lockout - khoa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // khoa 5p
                options.Lockout.MaxFailedAccessAttempts = 5; // that bai 5 lan thi khoa
                options.Lockout.AllowedForNewUsers = true;

                // cau hinh ve User
                options.User.AllowedUserNameCharacters = // cac ky tu dat ten User
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true; // email la duy nhat

                // cau hinh dang nhap
                options.SignIn.RequireConfirmedEmail = true; // cau hinh xac thuc dia chi email (email phai ton tai)
                options.SignIn.RequireConfirmedPhoneNumber = false; // xac thuc so dien thoai                    
            });


            services.AddOptions(); // kich hoat options
            services.Configure<MailSettings>(_config.GetSection("MailSettings")); // dki de inject
            services.AddTransient<IEmailSender, SendMailService>(); // dki dich vu mail

            // enable cors
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "quiz_app_dotnet_api v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            // enable cors
            app.UseCors("AllowAll");

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // phuc hoi thong tin dang nhap (xac thuc)
            app.UseAuthentication();
            // phuc hoi thong tin ve quyen cua User
            app.UseAuthorization();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
    }
}
