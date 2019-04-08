using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlogApi.Services.Helpers.Tokens
{
    public class RefreshTokenHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
