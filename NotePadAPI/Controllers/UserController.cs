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

        public UserController(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUsers()
        {
            ApiResponse response;
            try
            {
                IEnumerable<User> users = await _repo.GetAllUsers();
                string msg = users.Any() ? "Retrieved all users" : "No users found";
                response = Utility.CreateResponse(msg, HttpStatusCode.OK, users, true);
                return Ok(response);
            }
            catch (Exception e) 
            {
                response = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserRegistrationDto newUser)
        {
            ApiResponse resp;
            try
            {
                bool emailExists = await _repo.EmailExists(newUser.Email);
                if (emailExists)
                {
                    resp = Utility.CreateResponse("This email is already in use", HttpStatusCode.Conflict);
                    return Conflict(resp);
                }

                if (!UserUtils.IsValidEmail(newUser.Email))
                {
                    resp = Utility.CreateResponse("Email is not valid", HttpStatusCode.BadRequest);
                    return BadRequest(resp);
                }

                if (!UserUtils.IsPasswordValid(newUser.Password))
                {
                    resp = Utility.CreateResponse("Password is not valid", HttpStatusCode.BadRequest);
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
                return CreatedAtAction(nameof(Register), resp);
            }
            catch (Exception e) 
            {
                resp = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                return StatusCode((int)HttpStatusCode.InternalServerError, resp);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] UserLoginDto loginDto)
        {
            ApiResponse resp;
            try
            {
                User? user = await _repo.GetUserByEmail(loginDto.Email);

                if (user == null)
                {
                    resp = Utility.CreateResponse("This email is not registered", HttpStatusCode.Conflict);
                    return Conflict(resp);
                }

                if (!UserUtils.IsPasswordCorrect(loginDto.Password, user.PasswordSalt, user.PasswordHash))
                {
                    resp = Utility.CreateResponse("Incorrect password", HttpStatusCode.Unauthorized);
                    return Unauthorized(resp);
                }

                string token = UserUtils.GetToken(loginDto.Email, _config);
                var loginObj = new
                {
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
                return Ok(resp);
            }
            catch (Exception e)
            {
                resp = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                return StatusCode((int)HttpStatusCode.InternalServerError, resp);
            }
        }

        // GET: api/<UserController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<UserController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
