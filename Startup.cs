using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Helper;
using quiz_app_dotnet_api.Middlewares;
using quiz_app_dotnet_api.Modals;
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

            services.AddDbContext<DataContext>(options => options.UseSqlServer(_config.GetConnectionString("DbConnection")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])),
                    RoleClaimType = "Role" // important
                };
            });
            services.AddCors();
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(x =>
                 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "quiz_app_dotnet_api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
            });

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
                options.User.RequireUniqueEmail = false; // email la duy nhat

                // cau hinh dang nhap
                options.SignIn.RequireConfirmedEmail = false; // cau hinh xac thuc dia chi email (email phai ton tai)
                options.SignIn.RequireConfirmedPhoneNumber = false; // xac thuc so dien thoai                    
            });

            services.AddTransient<IUserRepository<User>, UserRepository>();
            services.AddTransient<UserService, UserService>();
            services.AddTransient<IJwtHelper, JwtHelper>();
            services.AddTransient<ICourseQuizRepository<CourseQuiz>, CourseQuizRepository>();
            services.AddTransient<CourseQuizService, CourseQuizService>();
            services.AddTransient<IQuestionQuizRepository<QuestionQuiz>, QuestionQuizRepository>();
            services.AddTransient<QuestionQuizService, QuestionQuizService>();
            services.AddTransient<IStorageScoresRepository<StorageScores>, StorageScoresRepository>();
            services.AddTransient<StorageScoresService, StorageScoresService>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

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

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var response = new ErrorModal()
                    {
                        Message = "Token Validation Has Failed. Request Access Denied"
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }

            });

            app.UseMiddleware<JwtMiddleware>();

            // phuc hoi thong tin dang nhap (xac thuc)
            app.UseAuthentication();
            // phuc hoi thong tin ve quyen cua User
            app.UseAuthorization();

            // enable cors
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
