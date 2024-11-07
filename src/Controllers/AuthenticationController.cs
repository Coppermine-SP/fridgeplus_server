using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Org.BouncyCastle.Tls;

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
        public IActionResult TokenSignIn([FromForm]string token)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(token).Result;
                _logger.LogInformation("Auth Request => " + payload.Subject);

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, payload.Name),
                    new Claim(ClaimTypes.Email, payload.Email),
                    new Claim(ClaimTypes.Sid, payload.Subject)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties{ IsPersistent = false });

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

        private record Account(string Sub, string Name);

        [HttpGet]
        [Authorize]
        public IActionResult AccountInfo()
        {
            string sid = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid))?.Value ?? "null";
            string name = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value ?? "null";

            return new JsonResult(new Account(sid, name));
        }

        [HttpGet]
        [Authorize]
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
