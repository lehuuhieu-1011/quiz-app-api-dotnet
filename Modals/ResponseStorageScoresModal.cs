using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_app_dotnet_api.Modals
{
    public class ResponseStorageScoresModal
    {
        public Guid Id { get; set; }
        public int Scores { get; set; }
        public string TimeSubmit { get; set; }
        public Guid UserId { get; set; }
        public int CourseQuizId { get; set; }
    }
}