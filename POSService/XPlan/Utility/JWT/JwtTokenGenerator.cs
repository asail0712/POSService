using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace XPlan.Utility.JWT
{
    /// <summary>
    /// JWT Token 生成器
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenGenerator(string secretKey, string issuer, string audience)
        {
            _secretKey  = secretKey;
            _issuer     = issuer;
            _audience   = audience;
        }

        public string GenerateToken(string userId, string userName, int expireMinutes = 60)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", "Admin"), // 自訂 claim
                new Claim("customClaim", "yourValue") // 你可以加入更多
            };

            var token = new JwtSecurityToken
            (
                issuer:             _issuer,
                audience:           _audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
