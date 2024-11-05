using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace fridgeplus_server.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private ILogger _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult TokenSignIn(string token)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(token).Result;
                _logger.LogInformation("Auth Request => " + payload.Email);

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, payload.Name),
                    new Claim(ClaimTypes.Sid, payload.JwtId)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                

                return Ok();

            }
            catch (InvalidJwtException e)
            {
                _logger.LogWarning("Auth Failed => InvalidJwt: " + token);
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Auth Failed => Exception: " + e.ToString());
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Hello, World!");
        }
    }
}
