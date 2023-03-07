using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practising_Authentication
{
    public class AuthenticationManager
    {
        private readonly string? _key;

        private readonly IDictionary<string, string> _systemusers = new Dictionary<string, string>()
        {
            {"test", "password" },
            {"test1", "Psswrd" },
            {"test2", "P@ssw0rd" }
        };

        public AuthenticationManager(string key)
        {
            _key = key;
        }

        public string Authenticate(string username, string password)
        {
            if (!_systemusers.Any( u => u.Key == username && u.Value == password))
            {
                return "User not found!";
            }

            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();

            var tokenkey = Encoding.ASCII.GetBytes(_key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenkey),
                    SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenhandler.CreateToken(tokenDescriptor);

            return tokenhandler.WriteToken(token);
        }
    }
}
