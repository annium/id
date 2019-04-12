using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Annium.AspNetCore.Extensions;
using Annium.Data.Operations;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Annium.Id.Api.Controllers
{
    [Route("demo")]
    public class DemoController : ServerController
    {
        private readonly Func<Instant> getInstant;

        public DemoController(
            Func<Instant> getInstant
        )
        {
            this.getInstant = getInstant;
        }

        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var token = LZ4MessagePackSerializer.Serialize(new Token("login", "password", Guid.NewGuid()));

            return Ok(Convert.ToBase64String(token));
        }

        [HttpPost("token")]
        public IActionResult ParseToken([FromBody] string tokenString)
        {
            var token = LZ4MessagePackSerializer.Deserialize<Token>(Convert.FromBase64String(tokenString));

            return Ok(token);
        }

        [MessagePackObject]
        public class Token
        {
            [Key(0)]
            public string Login { get; set; }

            [Key(1)]
            public string Password { get; set; }

            [Key(2)]
            public Guid ApiToken { get; set; }

            public Token(string login, string password, Guid apiToken)
            {
                Login = login;
                Password = password;
                ApiToken = apiToken;
            }
        }

        [HttpGet("jwt")]
        public IActionResult GetJwtAsync()
        {
            const string signingSecurityKey = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingSecurityKey));

            var claims = new Claim[]
            {
                new Claim("some", "value")
            };

            var token = new JwtSecurityToken(
                issuer: "mssg",
                audience: "mssgClient",
                claims : claims,
                expires: (getInstant() + Duration.FromDays(1)).ToDateTimeUtc(),
                signingCredentials : new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("jwt")]
        public IActionResult TestAsync([FromBody] string tokenString)
        {
            const string signingSecurityKey = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingSecurityKey));

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return BadRequest(Result.Failure().Error("Cannot read token"));

            var claimsPrincipal = handler.ValidateToken(tokenString, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateIssuer = true,
                    ValidIssuer = "mssg",

                    ValidateAudience = true,
                    ValidAudience = "mssgClient",

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5),
            }, out var token);

            return Ok(new { claimsPrincipal, token });
        }
    }
}