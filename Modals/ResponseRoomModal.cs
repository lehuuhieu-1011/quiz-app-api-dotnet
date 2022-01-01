using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_app_dotnet_api.Modals
{
    public class ResponseRoomModal
    {
        public Guid Id { get; set; }
        public string RoomId { get; set; }
        public Boolean Disable { get; set; }
        public int CourseId { get; set; }
    }
}