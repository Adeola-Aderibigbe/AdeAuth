using AdeAuth.Models;
using AdeAuth.Models.Dtos;
using AdeAuth.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace AdeAuth.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(IAuthService authService, IHttpContextAccessor httpContext)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public IActionResult Signup(SignUpDto signUpDto) 
        {
            var hasRegistered = _authService.SignUpUser(signUpDto);
            if(hasRegistered != new Guid())
            {
                return Ok("Registered successfully, proceed to login");
            }
            return BadRequest("Failed to register");
        }

        [Authorize()]
        [HttpPost("account")]
        public IActionResult SingleSignOn()
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var name = HttpContext.User.FindFirstValue("name")?.Split(' ');

            var currentUser = _authService.IsUserExist(email,AuthenticationType.Microsoft);

            Guid userId = new();
            if(currentUser == null)
            {
               userId =  _authService.SignUpUser(new SignUpDto() { Email = email, FirstName = name[0], LastName = name[1] },
                    AccountType.User,
                    AuthenticationType.Microsoft);
            }
            var result = _authService.Encrypt(email, userId.ToString(), AccountType.User.ToString());
            AppendCookie(result);
            return Ok("Hello user");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var result = _authService.LoginUser(loginDto);
            if (result == "Invalid email/password")
                return BadRequest(result);

            AppendCookie(result);
            return Ok("Hello user");
        }

        private void AppendCookie(string token)
        {
            Response.Cookies.Append("AdeAuth", token, new CookieOptions() 
            {
                HttpOnly = true,
                MaxAge = new TimeSpan(1, 0, 0, 0)
            });
        }

        private readonly IAuthService _authService;
    }
}
