using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.Repositories
{
    public interface IUserRepository<User>
    {
        Task<User> Register(User user);
        UserLoginResponse Login(LoginModal user);
        User GetByUserName(string username);
    }
}