using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using quiz_app_dotnet_api.Data;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Helper;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly DataContext _context;

        public UserRepository(IJwtHelper jwtHelper, DataContext context)
        {
            _jwtHelper = jwtHelper;
            _context = context;
        }

        public User GetByUserName(string username)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public UserLoginResponse Login(LoginModal loginModal)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == loginModal.Username);
            if (user == null)
            {
                return null;
            }
            if (user.Password == loginModal.Password)
            {
                string token = _jwtHelper.generateJwtToken(user);
                return new UserLoginResponse
                {
                    Id = user.Id.ToString(),
                    Username = user.UserName,
                    Token = token
                };
            }
            return null;
        }

        public async Task<User> Register(User user)
        {
            bool checkUser = _context.Users.Any(u => u.UserName == user.UserName);
            if (checkUser)
            {
                return null;
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}