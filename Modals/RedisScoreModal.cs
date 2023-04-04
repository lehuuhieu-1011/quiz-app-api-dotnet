using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Modals
{
  public class RedisScoreModal
  {
    public List<QuestionQuiz> questionQuizArray { get; set; }
    public int score { get; set; }
    // public Guid userId { get; set; }
    public string username { get; set; }
  }
}