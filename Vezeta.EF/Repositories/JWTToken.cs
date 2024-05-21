using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vezeta.Application.Interfaces;
using VezetaCore.Models;

namespace Vezeta.EF.Repositories
{
    public class JWTToken : IJwtToken
    {
        private readonly IConfiguration _configuration;
        public JWTToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateToken(ApplicationUser user,string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, Role),
                    // Add more claims as needed
                }),
                //Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:Duration"])),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public DateTime? ExtractValidToDateFromToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Parse the token's string representation into a JwtSecurityToken object
                var jwtToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;
                if (jwtToken != null)
                {
                    // Return the ValidTo date
                    return jwtToken.ValidTo;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the parsing error
                Console.WriteLine($"Error parsing JWT token: {ex.Message}");
            }
            // Return null if the token cannot be parsed
            return null;
        }

    }
}
