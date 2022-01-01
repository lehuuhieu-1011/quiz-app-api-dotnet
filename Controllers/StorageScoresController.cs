using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class StorageScoresController : BaseApiController
    {
        private readonly StorageScoresService _service;
        public StorageScoresController(StorageScoresService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            StorageScores response = await _service.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            ResponseStorageScoresModal scores = new ResponseStorageScoresModal
            {
                Id = response.Id,
                Scores = response.Scores,
                TimeSubmit = response.TimeSubmit,
                CourseQuizId = response.CourseQuizId,
                UserId = response.UserId,
            };
            return Ok(scores);
        }

        [HttpPost]
        public async Task<ActionResult> AddScores(AddScoresModal newScores)
        {

            StorageScores scores = new StorageScores
            {
                Scores = newScores.Scores,
                TimeSubmit = newScores.TimeSubmit,
                UserId = newScores.UserId,
                CourseQuizId = newScores.CourseQuizId,
            };
            StorageScores response = await _service.AddScores(scores);
            ResponseStorageScoresModal temp = new ResponseStorageScoresModal
            {
                Id = response.Id,
                Scores = response.Scores,
                TimeSubmit = response.TimeSubmit,
                CourseQuizId = response.CourseQuizId,
                UserId = response.UserId,
            };
            return CreatedAtAction(nameof(GetById), new { id = temp.Id }, temp);
        }

        [HttpGet("GetScoresByUserId/{id}")]
        public async Task<ActionResult> GetScoresByUserId(Guid id)
        {
            List<StorageScores> scores = await _service.GetScoresByUserId(id);
            if (scores == null)
            {
                return NotFound();
            }
            return Ok(scores);
        }
    }
}