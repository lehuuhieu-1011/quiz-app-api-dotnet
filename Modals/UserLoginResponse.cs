using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_app_dotnet_api.Modals
{
    public class UserLoginResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}