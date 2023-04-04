using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;
using StackExchange.Redis;

namespace quiz_app_dotnet_api.Controllers
{
  // [Authorize(Roles = "User")]
  public class StorageScoresController : BaseApiController
  {
    private readonly StorageScoresService _service;
    private readonly IDistributedCache _distributedCache;
    private readonly QuestionQuizService _questionQuizService;
    public StorageScoresController(StorageScoresService service, QuestionQuizService questionQuizService, IDistributedCache distributedCache)
    {
      _service = service;
      _questionQuizService = questionQuizService;
      _distributedCache = distributedCache;
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

    [HttpPost("AddScoreToRedis")]
    public async Task<ActionResult> AddScoreToRedis(int questionId, string username, string answer)
    {
      //   var cacheKey = "Score:" + userId;
      var cacheKey = "Score:" + username;

      var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20));

      RedisScoreModal redisScoreModal;

      var questionQuiz = _questionQuizService.GetById(questionId);

      try
      {
        var redisScore = await _distributedCache.GetAsync(cacheKey);
        if (redisScore == null)
        {
          //   var options = new DistributedCacheEntryOptions()
          //       .SetSlidingExpiration(TimeSpan.FromMinutes(20));

          List<QuestionQuiz> listQuestionQuiz = new List<QuestionQuiz>();
          listQuestionQuiz.Add(questionQuiz);

          int score = 0;

          if (questionQuiz.correctAnswer.Equals(answer.ToUpper()))
          {
            score += 1;
          }

          redisScoreModal = new RedisScoreModal
          {
            score = score,
            questionQuizArray = listQuestionQuiz,
            // userId = userId
            username = username
          };

          //   // Converts a list of RedisScoreModal to a string using NewtonSoft
          //   string serializedRedisScoreModal = JsonConvert.SerializeObject(redisScoreModal);

          //   //  Converts the string to a Byte Array. This array will be stored in Redis
          //   redisScore = Encoding.UTF8.GetBytes(serializedRedisScoreModal);

          //   await _distributedCache.SetAsync(cacheKey, redisScore, options);
        }
        else
        {
          // Converts the Byte Array to a string
          string serializedRedisScoreModalTemp = Encoding.UTF8.GetString(redisScore);

          // Converts the string to a list of RedisScoreModal using NewtonSoft
          redisScoreModal = JsonConvert.DeserializeObject<RedisScoreModal>(serializedRedisScoreModalTemp);

          List<QuestionQuiz> listQuestionQuiz = redisScoreModal.questionQuizArray;

          int score = redisScoreModal.score;

          if (questionQuiz.correctAnswer.Equals(answer.ToUpper()))
          {
            score += 1;
          }

          listQuestionQuiz.Add(questionQuiz);

          redisScoreModal.score = score;
          redisScoreModal.questionQuizArray = listQuestionQuiz;

          //   // Converts a list of RedisScoreModal to a string using NewtonSoft
          //   serializedRedisScoreModal = JsonConvert.SerializeObject(redisScoreModal);

          //   //  Converts the string to a Byte Array. This array will be stored in Redis
          //   redisScore = Encoding.UTF8.GetBytes(serializedRedisScoreModal);

          //   await _distributedCache.SetAsync(cacheKey, redisScore);
        }

        // Converts a list of RedisScoreModal to a string using NewtonSoft
        string serializedRedisScoreModal = JsonConvert.SerializeObject(redisScoreModal);

        //  Converts the string to a Byte Array. This array will be stored in Redis
        redisScore = Encoding.UTF8.GetBytes(serializedRedisScoreModal);

        await _distributedCache.SetAsync(cacheKey, redisScore, options);

        return Ok(new { message = "Success" });
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return BadRequest(new
        {
          message = "Error: " + e.Message
        });
      }
    }

    [HttpGet("GetScoreFromRedis/{username}")]
    public async Task<ActionResult> GetScoreFromRedis(string username)
    {
      var cacheKey = "Score:" + username;

      var redisScore = await _distributedCache.GetAsync(cacheKey);

      if (redisScore == null)
      {
        return NotFound(new { message = "Username not found" });
      }
      // Converts the Byte Array to a string
      string serializedRedisScoreModalTemp = Encoding.UTF8.GetString(redisScore);

      // Converts the string to a list of RedisScoreModal using NewtonSoft
      RedisScoreModal redisScoreModal = JsonConvert.DeserializeObject<RedisScoreModal>(serializedRedisScoreModalTemp);

      return Ok(new { score = redisScoreModal.score });
    }

    [HttpGet("GetScoreFromRedis")]
    public async Task<ActionResult> GetAllScoreFromRedis()
    {
      ConfigurationOptions options = ConfigurationOptions.Parse("localhost:6379");
      ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

      var keys = connection.GetServer("localhost", 6379).Keys();
      string[] keysArr = keys.Select(key => (string)key).ToArray();
      List<RedisScoreModal> listRedisScoreModal = new List<RedisScoreModal>();
      foreach (string key in keysArr)
      {
        var redisScore = await _distributedCache.GetAsync(key);
        // Converts the Byte Array to a string
        string serializedRedisScoreModalTemp = Encoding.UTF8.GetString(redisScore);

        // Converts the string to a list of RedisScoreModal using NewtonSoft
        RedisScoreModal redisScoreModal = JsonConvert.DeserializeObject<RedisScoreModal>(serializedRedisScoreModalTemp);
        listRedisScoreModal.Add(redisScoreModal);
      }

      return Ok(listRedisScoreModal);
    }

    [HttpDelete("RemoveAllDataInRedis")]
    public async Task<ActionResult> RemoveAllDataInRedis()
    {
      ConfigurationOptions options = ConfigurationOptions.Parse("localhost:6379");
      ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

      var keys = connection.GetServer("localhost", 6379).Keys();
      string[] keysArr = keys.Select(key => (string)key).ToArray();
      foreach (string key in keysArr)
      {
        await _distributedCache.RemoveAsync(key);
      }

      return Ok(new { message = "Success" });
    }
  }
}