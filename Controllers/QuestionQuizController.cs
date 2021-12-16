using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api.Controllers
{
    public class QuestionQuizController : BaseApiController
    {
        private readonly QuestionQuizService _service;
        private readonly IQuestionQuizRepository<QuestionQuiz> _repo;
        private readonly IDistributedCache _distributedCache;

        public QuestionQuizController(QuestionQuizService service, IQuestionQuizRepository<QuestionQuiz> repo, IDistributedCache distributedCache)
        {
            _service = service;
            _repo = repo;
            _distributedCache = distributedCache;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            // https://codewithmukesh.com/blog/redis-caching-in-aspnet-core/

            // var cacheKey = "listQuestion";
            // string serializedListQuestion;
            // var listQuestion = new List<QuestionQuiz>();
            // try
            // {
            //     var redisListQuestion = await _distributedCache.GetAsync(cacheKey);
            //     if (redisListQuestion != null)
            //     {
            //         serializedListQuestion = Encoding.UTF8.GetString(redisListQuestion);
            //         listQuestion = JsonConvert.DeserializeObject<List<QuestionQuiz>>(serializedListQuestion);
            //     }
            //     else
            //     {
            //         listQuestion = await _service.GetAll();
            //         serializedListQuestion = JsonConvert.SerializeObject(listQuestion);
            //         redisListQuestion = Encoding.UTF8.GetBytes(serializedListQuestion);
            //         var options = new DistributedCacheEntryOptions()
            //             .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
            //             .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            //         await _distributedCache.SetAsync(cacheKey, redisListQuestion, options);
            //     }
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return BadRequest(new { message = "Can't connection to Redis" });
            // }

            // return Ok(listQuestion);

            return Ok(await _service.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            QuestionQuiz response = _service.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            ResponseQuestionQuizModal question = new ResponseQuestionQuizModal
            {
                Id = response.id,
                Question = response.question,
                AnswerA = response.answerA,
                AnswerB = response.answerB,
                AnswerC = response.answerC,
                AnswerD = response.answerC,
                CorrectAnswer = response.correctAnswer,
                Image = response.image,
                Point = response.point,
                CourseId = response.courseId
            };
            return Ok(question);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateQuestion(ResponseQuestionQuizModal newQuestion)
        {
            QuestionQuiz question = new QuestionQuiz
            {
                id = newQuestion.Id,
                question = newQuestion.Question,
                answerA = newQuestion.AnswerA,
                answerB = newQuestion.AnswerB,
                answerC = newQuestion.AnswerC,
                answerD = newQuestion.AnswerC,
                correctAnswer = newQuestion.CorrectAnswer,
                point = newQuestion.Point,
                image = newQuestion.Image,
                courseId = newQuestion.CourseId
            };
            QuestionQuiz response = await _service.CreateQuestion(question);
            return CreatedAtAction(nameof(GetById), new { id = response.id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, ResponseQuestionQuizModal updateQuestion)
        {
            if (id != updateQuestion.Id)
            {
                return BadRequest();
            }
            QuestionQuiz question = new QuestionQuiz
            {
                id = updateQuestion.Id,
                question = updateQuestion.Question,
                answerA = updateQuestion.AnswerA,
                answerB = updateQuestion.AnswerB,
                answerC = updateQuestion.AnswerC,
                answerD = updateQuestion.AnswerD,
                correctAnswer = updateQuestion.CorrectAnswer,
                image = updateQuestion.Image,
                point = updateQuestion.Point,
                courseId = updateQuestion.CourseId
            };
            bool check = await _service.UpdateQuestion(question);
            if (!check)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            bool check = await _service.DeleteQuestion(id);
            if (!check)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("/api/GetAllQuestionByIdCourse/{id}")]
        public async Task<ActionResult> GetQuestionByIdCourse(int id)
        {
            List<QuestionQuiz> questions = await _service.GetQuestionByIdCourse(id);
            if (questions == null)
            {
                return BadRequest();
            }
            return Ok(questions);
        }


    }
}