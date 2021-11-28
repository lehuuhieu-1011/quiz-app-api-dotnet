using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Helper;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtHelper _jwtHelper;

        public UserRepository(SignInManager<User> signInManager, UserManager<User> userManager, IJwtHelper jwtHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<UserLoginResponse> Login(LoginModal loginModal)
        {
            // login bang username
            User user = await _userManager.FindByNameAsync(loginModal.Username);
            var result = await _signInManager.PasswordSignInAsync(loginModal.Username, loginModal.Password, false, false);

            // if (!result.Succeeded)
            // {
            //     // that bai khi login bang username va password -> tim user theo email, neu thay thi thu dang nhap bang user tim duoc
            //     user = await _userManager.FindByEmailAsync(loginModal.Username);
            //     if (user != null)
            //     {
            //         result = await _signInManager.PasswordSignInAsync(user, loginModal.Password, false, false);
            //     }
            // }
            if (result.Succeeded)
            {
                string token = _jwtHelper.generateJwtToken(user);
                return new UserLoginResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = token
                };
            }
            return null;
        }

        public async Task<string> Register(RegisterModal registerModal)
        {
            var user = new User
            {
                UserName = registerModal.Username,
            };
            var result = await _userManager.CreateAsync(user, registerModal.Password);
            var errorMessage = "";
            foreach (var error in result.Errors)
            {
                errorMessage += error.Description + " ";
            }
            if (result.Succeeded)
            {
                return null;
            }
            return errorMessage;
        }
    }
}