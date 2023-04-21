using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ftrip.io.framework.auth
{
    public class JwtBuilder
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SecurityTokenDescriptor _tokenDescriptor;

        public JwtBuilder()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity()
            };
        }

        public JwtBuilder SetSecret(string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            _tokenDescriptor.SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            return this;
        }

        public JwtBuilder SetTime(int minutes)
        {
            _tokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(minutes);

            return this;
        }

        public JwtBuilder AddClaim(string claim, object value)
        {
            _tokenDescriptor.Subject.AddClaim(new Claim(claim, value.ToString()));

            return this;
        }

        public string Build()
        {
            var token = _tokenHandler.CreateToken(_tokenDescriptor);

            return _tokenHandler.WriteToken(token);
        }
    }
}