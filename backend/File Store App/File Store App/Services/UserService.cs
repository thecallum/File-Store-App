using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using File_Store_App.Entities;
using File_Store_App.Helpers;
using File_Store_App.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace File_Store_App.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(string username, string password);
        object GetById(int userId);
    }
    
    public class UserService : IUserService
    {
        
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        private readonly List<User> _users = new()
        {
            new User()
            {
                Id = 1,
                Username = "username",
                Password = "password"
            },
            new User()
            {
                Id = 2,
                Username = "username2",
                Password = "password2"
            }
        };
        
        public AuthenticateResponse Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _users.SingleOrDefault(x => x.Username == username);
            if (user == null) return null;
            
            // validate password
            if (user.Password != password) return null;
            // return user
            
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public object GetById(int userId)
        {
            var user = _users.SingleOrDefault(x => x.Id == userId);
            if (user == null) return null;

            return new User
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}