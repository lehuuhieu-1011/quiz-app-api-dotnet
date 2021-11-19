using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using quiz_app_dotnet_api.Dtos;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterDto> _logger;
        // private readonly IEmailSender _emailSender;


        // cac service duoc inject vao: UserManager, SignInManager, ILogger, IEmailSender
        // public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RegisterDto> logger, IEmailSender emailSender)
        // {
        //     _userManager = userManager;
        //     _signInManager = signInManager;
        //     _logger = logger;
        //     _emailSender = emailSender;
        // }

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RegisterDto> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // [Route("{id}")]
        // [HttpGet]
        // public async Task<User> GetById(string id)
        // {
        //     var user = await _userManager.FindByIdAsync(id);
        //     return user;
        // }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            var errorMessage = new ErrorDto { Message = "" };
            foreach (var error in result.Errors)
            {
                errorMessage.Message += error.Description + " ";
                _logger.LogError(error.Description);
            }
            if (result.Succeeded)
            {
                _logger.LogInformation("Create user succesfully");
                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // return CreatedAtAction(nameof(GetById), new User{Id = });
                return NoContent();
            }

            return BadRequest(errorMessage);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            // login bang username
            var result = await _signInManager.PasswordSignInAsync(loginDto.UsernameOrEmail, loginDto.Password, false, false);
            var errorMessage = new ErrorDto { Message = "" };

            if (!result.Succeeded)
            {
                // that bai khi login bang username va password -> tim user theo email, neu thay thi thu dang nhap bang user tim duoc
                var user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
                if (user != null)
                {
                    result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
                }
                else
                {
                    errorMessage.Message = "Username or Password is Invalid";
                }
            }
            if (result.Succeeded)
            {
                _logger.LogInformation("Login successfully");
                return NoContent();
            }
            return BadRequest(errorMessage);

        }
    }
}