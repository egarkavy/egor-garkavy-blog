using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using BlogApi.Services.Models;
using BlogApi.Services;
using System.Net;
using Microsoft.AspNetCore.Cors;
using BlogApi.Services.Helpers.Tokens;
using BlogApi.Common.JWT;

namespace TokenApp.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AccountController(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        private List<User> people = new List<User>
        {
            new User {Login="admin@gmail.com", Password="12345", Role = "admin", Role2 = "allowAll" },
            new User { Login="qwerty", Password="55555", Role = "user" }
        };
        

        [HttpGet("Hello")]
        public string Hello()
        {
            return "asdddd";
        }
        public class Form
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        [HttpPost("Token")]
        public async Task Token([FromBody]Form form)
        {
            var username = form.UserName;
            var password = form.Password;

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }
            
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    //issuer: Constants.ISSUER,
                    //audience: Constants.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.AddMinutes(1),
                    signingCredentials: new SigningCredentials(Constants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost]
        public IActionResult Refresh(string token, string refreshToken)
        {
            var principal = JWTHelper.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = GenerateToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            DeleteRefreshToken(username, refreshToken);
            SaveRefreshToken(username, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                };

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token"
                    //, 
                    //ClaimsIdentity.DefaultNameClaimType,
                    //myRole
                    );
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}