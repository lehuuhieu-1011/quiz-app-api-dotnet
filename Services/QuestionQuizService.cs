using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Services
{
    public class QuestionQuizService
    {
        private readonly IQuestionQuizRepository<QuestionQuiz> _repo;

        public QuestionQuizService(IQuestionQuizRepository<QuestionQuiz> repo)
        {
            _repo = repo;
        }

        public async Task<List<QuestionQuiz>> GetAll()
        {
            return await _repo.GetAll();
        }

        public QuestionQuiz GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<QuestionQuiz> CreateQuestion(QuestionQuiz question)
        {
            return await _repo.CreateQuestion(question);
        }

        public async Task<bool> UpdateQuestion(int id, QuestionQuiz question)
        {
            return await _repo.UpdateQuestion(id, question);
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            return await _repo.DeleteQuestion(id);
        }
    }
}