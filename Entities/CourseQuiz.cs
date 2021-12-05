using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace quiz_app_dotnet_api.Entities
{
    [Table("course_quiz")]
    public class CourseQuiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string name { get; set; }
        [Column("image")]
        public string image { get; set; }
        [JsonIgnore]
        public StorageScores StorageScores { get; set; }
        [JsonIgnore]
        public IList<QuestionQuiz> QuestionQuiz { get; } = new List<QuestionQuiz>();
    }
}