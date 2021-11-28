using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<string> Register(RegisterModal registerModal)
        {
            return await _repo.Register(registerModal);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _repo.GetByUsername(username);
        }

        public async Task<UserLoginResponse> Login(LoginModal loginModal)
        {
            return await _repo.Login(loginModal);
        }
    }
}