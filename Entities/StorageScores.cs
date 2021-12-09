using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace quiz_app_dotnet_api.Entities
{
    [Table("storage_scores")]
    public class StorageScores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Scores { get; set; }
        public string TimeSubmit { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int CourseQuizId { get; set; }
        public virtual CourseQuiz CourseQuiz { get; set; }
    }
}