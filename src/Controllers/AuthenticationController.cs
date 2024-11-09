/*
    AuthenticationController - fridgeplus_server
    Copyright (C) 2024-2025 Coppermine-SP
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace fridgeplus_server.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthenticationController(ILogger<AuthenticationController> logger) : ControllerBase
    {
        [HttpPost]
        public IActionResult TokenSignIn([FromForm]string token)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(token).Result;
                logger.LogInformation("Auth Request => " + payload.Subject);

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
            catch (InvalidJwtException _)
            {
                logger.LogWarning("Auth Failed => InvalidJwt: " + token);
                return Unauthorized();
            }
            catch (Exception e)
            {
                logger.LogWarning("Auth Failed => Exception: " + e.ToString());
                return StatusCode(500);
            }
        }

        record Account(string Sub, string Name);

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
        public new IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
