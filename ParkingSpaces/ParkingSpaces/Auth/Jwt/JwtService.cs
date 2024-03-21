using Microsoft.IdentityModel.Tokens;
using ParkingSpaces.Migrations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingSpaces.Auth.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.First(i => i.Type == "userId").Value);
        }

        public string Generate(int userId)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(3);

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("userId", userId.ToString())
            });

            //create the jwt
            var token = (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"],
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: credentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
