using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.DTO;

namespace Infrastructure.Authentication
{
    public interface IGenerateToken
    {
        string Generate(UserDto user);

        public class Fake : IGenerateToken
        {
            public string Generate(UserDto user)
            {
                return "new fake token";
            }
        }

    }
    public class Jwt : IGenerateToken
    {
        private readonly byte[] _key;
        private readonly  DateTime _expiresDateTime;
        public Jwt(IOptions<JwtOptions> jwtOptions)
        {
            _key = Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey);
            _expiresDateTime = DateTime.UtcNow.AddHours(jwtOptions.Value.ExpiresHours);
        }

        public string Generate(UserDto user)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new Claim[] { new("userId", user.Id.ToString()) },
                signingCredentials: signingCredentials,
                expires: _expiresDateTime);

            var handler = new JwtSecurityTokenHandler();

            var result = handler.WriteToken(token);

            return result;
        }
    }
}
