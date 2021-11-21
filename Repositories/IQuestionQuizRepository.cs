using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Repositories
{
    public interface IQuestionQuizRepository<T>
    {
        Task<List<QuestionQuiz>> GetAll();
        Task<List<QuestionQuiz>> GetQuestionByIdCourse(int id);
        QuestionQuiz GetById(int id);
        Task<QuestionQuiz> CreateQuestion(QuestionQuiz question);
        Task<bool> UpdateQuestion(QuestionQuiz question);
        Task<bool> DeleteQuestion(int id);

    }
}