﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace XPlan.Utility.JWT
{
    /// <summary>
    /// JWT Token 生成器，負責產生 JWT 用於認證
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;   // 用於簽名的密鑰
        private readonly string _issuer;      // 簽發者
        private readonly string _audience;    // 接收者

        public JwtTokenGenerator(string secretKey, string issuer, string audience)
        {
            _secretKey  = secretKey;
            _issuer     = issuer;
            _audience   = audience;
        }

        /// <summary>
        /// 產生 JWT Token
        /// </summary>
        /// <param name="userId">使用者 Id，放在 sub claim</param>
        /// <param name="userName">使用者名稱，放在 unique_name claim</param>
        /// <param name="expireMinutes">過期時間（分鐘），預設 60 分鐘</param>
        /// <returns>JWT 字串</returns>
        public string GenerateToken(string userId, string userName, int expireMinutes = 60)
        {
            // 透過密鑰建立簽名憑證
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 定義要放入 Token 的 Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),             // 主體 (Subject)
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),    // 使用者名稱
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID，唯一識別碼
                new Claim("role", "Admin"),                                  // 自訂角色 claim
                new Claim("customClaim", "yourValue")                        // 其他自訂 claim
            };

            // 建立 JWT Token
            var token = new JwtSecurityToken
            (
                issuer:             _issuer,
                audience:           _audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            // 將 JWT 物件轉成字串
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
