using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using quiz_app_dotnet_api.Entities;

namespace quiz_app_dotnet_api.Helper
{
    public interface IJwtHelper
    {
        string generateJwtToken(User user);
    }
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        public string generateJwtToken(User user)
        {
            // security key
            string securityKey = _config["JWT:Key"];

            // symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            // signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claim = new[]{
                new Claim("Id", user.Id.ToString()),
                new Claim("Username", user.UserName),
                new Claim("Role", user.Role == null ? "" : user.Role)
            };

            // create token
            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claim
            );

            // return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}