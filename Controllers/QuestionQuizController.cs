using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api.Controllers
{
    // [Authorize(Roles = "User")]
    public class QuestionQuizController : BaseApiController
    {
        private readonly QuestionQuizService _service;
        private readonly IQuestionQuizRepository<QuestionQuiz> _repo;

        public QuestionQuizController(QuestionQuizService service, IQuestionQuizRepository<QuestionQuiz> repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuestionQuiz>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<QuestionQuiz> GetById(int id)
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
                CourseId = response.courseId
            };
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionQuiz>> CreateQuestion(QuestionQuiz newQuestion)
        {
            QuestionQuiz response = await _service.CreateQuestion(newQuestion);
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
                CourseId = response.courseId
            };
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, QuestionQuiz question)
        {
            if (id != question.id)
            {
                return BadRequest();
            }
            await _service.UpdateQuestion(question);
            return NoContent();
        }

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