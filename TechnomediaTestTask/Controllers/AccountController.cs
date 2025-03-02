using System.Web.Http;
using TechnomediaTestTask.Services;
using TechnomediaTestTask.DTO;
using System;

namespace TechnomediaTestTask.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly AuthService _authService;
        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginDTO user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                if (_authService.ValidateUser(user, out var userRoles))
                {
                    var token = _authService.GenerateToken(user.Username, userRoles);
                    return Ok(new { Token = token });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(System.Net.HttpStatusCode.Unauthorized, "Invalid username or password.");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(RegisterDTO user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.RoleName))
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var result = _authService.RegisterUser(user);
                if (result)
                {
                    return Ok("User registered successfully.");
                }
                return BadRequest("Registration failed. Username may already exist or role is invalid.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var users = _authService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}