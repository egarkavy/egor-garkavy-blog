using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApi.Services.Helpers.Tokens
{
    public static class JwtHelper
    {
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might dont want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidAudience = Constants.AUDIENCE,
                ValidIssuer = Constants.ISSUER,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Constants.GetSymmetricSecurityKey(),
                ValidateLifetime = false //to allow expired token be validated
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public static string GenerateToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: Constants.ISSUER,
                    audience: Constants.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.AddMinutes(1),
                    signingCredentials: new SigningCredentials(Constants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
