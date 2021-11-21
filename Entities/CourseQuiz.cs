using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        // public virtual ICollection<QuestionQuiz> QuestionQuiz { get; set; }
    }
}