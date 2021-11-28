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
        public async Task<User> GetByUsername(string username)
        {
            return await _service.GetByUsername(username);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModal registerModal)
        {
            var response = await _service.Register(registerModal);
            if (response == null)
            {
                return CreatedAtAction(nameof(GetByUsername), new { username = registerModal.Username }, registerModal);
            }
            return BadRequest(response);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginModal loginModal)
        {
            var response = await _service.Login(loginModal);
            if (response == null)
            {
                return BadRequest(new { message = "User name or password not correct" });
            }
            return Ok(response);
        }
    }
}