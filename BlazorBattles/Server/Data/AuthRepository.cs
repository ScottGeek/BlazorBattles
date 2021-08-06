using BlazorBattles.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ServiceReponse<string>> Login(string email, string password)
        {
            var response = new ServiceReponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";  //careful here you are letting the hacker know something.

            }
            else
            {
                if (!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Wrong password.";  //careful here you are letting the hacker know something.
                }
                else
                {
                    response.Data = CreateToken(user);
                    response.Success = true;

                }
            }

            return response;



        }

        public async Task<ServiceReponse<int>> Register(User user, string password, int startUnit)
        {

            if (await UserExists(user.Email))
            {
                return new ServiceReponse<int> { Success = false, Message = "User already exists." };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await AddStartingUnit(user, startUnit);

            return new ServiceReponse<int> { Success = true, Data = user.Id, Message="Registration successful!"};
                
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) //the hash is not the same
                    {
                        return false;
                    }
                }

                return true;

            }
        }

        private string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task AddStartingUnit(User user, int startUnitId)
        {
            var unit = await _context.Units.FirstOrDefaultAsync<Unit>(u => u.Id == startUnitId);
            _context.UserUnits.Add(new UserUnit { 
              UnitId = unit.Id,
              UserId = user.Id,
              HitPoints = unit.HitPoints,
            });

            await _context.SaveChangesAsync();

        }
    }
}
