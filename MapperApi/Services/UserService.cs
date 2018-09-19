using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Helpers;
using Mapper_Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapper_Api.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        Task<User> CreateUserAsync(User user);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly AppSettings _appSettings;
        private  ZoneDB _ZoneDB;

        public UserService(IOptions<AppSettings> appSettings, ZoneDB ZoneDB)
        {
            _appSettings = appSettings.Value;
            _ZoneDB = ZoneDB;
        }

        public User Authenticate(string email, string password)
        {
            var user = _ZoneDB.Users.SingleOrDefault(x => x.Email == email && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public async Task<User> CreateUserAsync( User user ){
            if (!_ZoneDB.Users.Any( u => u.Email == user.Email)) {
                user.UserID = Guid.NewGuid();
                _ZoneDB.Add(user);
                await _ZoneDB.SaveChangesAsync();
                return user;
            } else {
                throw new ArgumentException("User Email should be unique");
            }
            user.Password = null;
            user.Zones = null;
            user.Token = null;
            return user;
        }


    }
}