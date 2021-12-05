using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace quiz_app_dotnet_api.Entities
{
    [Table("question_quiz")]
    public class QuestionQuiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("question")]
        public string question { get; set; }
        [Column("answer_a")]
        public string answerA { get; set; }
        [Column("answer_b")]
        public string answerB { get; set; }
        [Column("answer_c")]
        public string answerC { get; set; }
        [Column("answer_d")]
        public string answerD { get; set; }
        [Column("correct_answer")]
        public string correctAnswer { get; set; }
        [Column("image")]
        public string image { get; set; }
        [Column("point")]
        public float point { get; set; }
        [Column("course_id")]
        public int courseId { get; set; }
        [JsonIgnore]
        public CourseQuiz course { get; set; }
    }
}