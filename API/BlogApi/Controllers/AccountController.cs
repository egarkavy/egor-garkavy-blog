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
using BlogApi.Common.Tokens;

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
        public JsonResult Token([FromBody]Form form)
        {
            var username = form.UserName;
            var password = form.Password;

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid username or password.");
            }

            var encodedJwt = JwtHelper.GenerateToken(identity.Claims);
            var refreshToken = RefreshTokenHelper.GenerateRefreshToken();

            _refreshTokenRepository.Delete(username);
            _refreshTokenRepository.Save(username, refreshToken);

            var response = new AppTokenModel
            {
                AccessToken = encodedJwt,
                RefreshToken = refreshToken,
                Username = identity.Name
            };
            
            return Json(response);
        }

        [HttpPost("Refresh")]
        public JsonResult Refresh([FromBody]AppTokenModel refreshModel)
        {
            var token = refreshModel.AccessToken;
            var refreshToken = refreshModel.RefreshToken;

            var principal = JwtHelper.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = _refreshTokenRepository.Get(username); //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = JwtHelper.GenerateToken(principal.Claims);
            var newRefreshToken = RefreshTokenHelper.GenerateRefreshToken();
            _refreshTokenRepository.Delete(username, refreshToken);
            _refreshTokenRepository.Save(username, newRefreshToken);

            return Json(new AppTokenModel
            {
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        public class AppTokenModel
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public string Username { get; set; }
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
                    new ClaimsIdentity(claims, "Token");

                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}