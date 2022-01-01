using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace quiz_app_dotnet_api.Entities
{
    [Table("room")]
    public class Room
    {
        public Guid Id { get; set; }
        public string RoomId { get; set; }
        public Boolean Disable { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public virtual CourseQuiz Course { get; set; }
    }
}