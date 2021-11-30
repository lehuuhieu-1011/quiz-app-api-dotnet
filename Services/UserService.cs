using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Services
{
    public class UserService
    {
        private readonly IUserRepository<User> _repo;
        public UserService(IUserRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<User> Register(RegisterModal registerModal)
        {
            return await _repo.Register(registerModal);
        }
        public UserLoginResponse Login(LoginModal loginModal)
        {
            return _repo.Login(loginModal);
        }
        public User GetByUserName(string username)
        {
            return _repo.GetByUserName(username);
        }
    }
}