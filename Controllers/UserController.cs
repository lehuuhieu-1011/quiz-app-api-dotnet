using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Services;
using quiz_app_dotnet_api.Repositories;

namespace quiz_app_dotnet_api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserService _service;
        private readonly IUserRepository<User> _repo;

        public UserController(UserService service, IUserRepository<User> repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpGet("{username}")]
        public IActionResult GetByUserName(string username)
        {
            User user = _service.GetByUserName(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModal registerModal)
        {
            var response = await _service.Register(registerModal);
            if (response == null)
            {
                return BadRequest(response);
            }
            return CreatedAtAction(nameof(GetByUserName), new { username = registerModal.UserName }, registerModal);
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(LoginModal loginModal)
        {
            var response = _service.Login(loginModal);
            if (response == null)
            {
                return BadRequest(new { message = "User name or password not correct" });
            }
            return Ok(response);
        }
    }
}