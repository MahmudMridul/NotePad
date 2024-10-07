using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotePadAPI.Models;
using NotePadAPI.Models.DTO;
using NotePadAPI.Repository.IRepository;
using NotePadAPI.Utils;
using System.Net;

namespace NotePadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private string _controller => ControllerContext.ActionDescriptor.ControllerName;

        public UserController(IUserRepository repo, IConfiguration config, ILogger<UserController> logger)
        {
            _repo = repo;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUsers()
        {
            ApiResponse response;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                IEnumerable<User> users = await _repo.GetAllUsers();
                string msg = users.Any() ? "Retrieved all users" : "No users found";
                response = Utility.CreateResponse(msg, HttpStatusCode.OK, users, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(response)}");
                return Ok(response);
            }
            catch (Exception e) 
            {
                response = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(response)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserRegistrationDto newUser)
        {
            ApiResponse resp;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                bool emailExists = await _repo.EmailExists(newUser.Email);
                if (emailExists)
                {
                    resp = Utility.CreateResponse("This email is already in use", HttpStatusCode.Conflict);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                    return Conflict(resp);
                }

                if (!UserUtils.IsValidEmail(newUser.Email))
                {
                    resp = Utility.CreateResponse("Email is not valid", HttpStatusCode.BadRequest);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                    return BadRequest(resp);
                }

                if (!UserUtils.IsPasswordValid(newUser.Password))
                {
                    resp = Utility.CreateResponse("Password is not valid", HttpStatusCode.BadRequest);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                    return BadRequest(resp);
                }

                var (Salt, Hash) = UserUtils.GetPasswordSaltAndHash(newUser.Password);

                User user = new User()
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    PasswordSalt = Salt,
                    PasswordHash = Hash,
                };

                _repo.RegisterUser(user);

                var createdUser = new
                {
                    Name = newUser.Name,
                    Email = newUser.Email
                };
                resp = Utility.CreateResponse("Registration successful", HttpStatusCode.Created, createdUser, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                return CreatedAtAction(nameof(Register), resp);
            }
            catch (Exception e) 
            {
                resp = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, resp);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] UserLoginDto loginDto)
        {
            ApiResponse resp;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                _logger.LogInformation($"{_controller}/{apiName} - {loginDto.Email} trying to sign in.");
                User? user = await _repo.GetUserByEmail(loginDto.Email);

                if (user == null)
                {
                    resp = Utility.CreateResponse("This email is not registered", HttpStatusCode.Conflict);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                    return Conflict(resp);
                }

                if (!UserUtils.IsPasswordCorrect(loginDto.Password, user.PasswordSalt, user.PasswordHash))
                {
                    resp = Utility.CreateResponse("Incorrect password", HttpStatusCode.Unauthorized);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                    return Unauthorized(resp);
                }

                string token = UserUtils.GetToken(loginDto.Email, _config);
                var loginObj = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                };

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,       // Prevents access via JavaScript
                    Secure = true,         // Ensures the cookie is only sent over HTTPS
                    SameSite = SameSiteMode.None,  // Prevents CSRF attacks
                    Expires = DateTime.UtcNow.AddHours(1)  // Set the expiration of the token (optional)
                };

                // Append the token to the response as a cookie
                Response.Cookies.Append("AuthToken", token, cookieOptions);

                resp = Utility.CreateResponse("Login successful", HttpStatusCode.OK, loginObj, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                return Ok(resp);
            }
            catch (Exception e)
            {
                resp = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(resp)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, resp);
            }
        }
    }
}
