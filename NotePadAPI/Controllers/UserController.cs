using Microsoft.AspNetCore.Mvc;
using NotePadAPI.Models;
using NotePadAPI.Models.DTO;
using NotePadAPI.Repository;
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
        private readonly ApiResponse _res;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
            _res = new ApiResponse();    
        }

        private void CreateResponse(string msg, HttpStatusCode code, object? data = null, bool isSuccess = false)
        {
            _res.Data = data;
            _res.StatusCode = code;
            _res.IsSuccess = isSuccess;
            _res.Message = msg;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUsers()
        {
            try
            {
                IEnumerable<User> users = await _repo.GetAllUsers();
                string msg = users.Any() ? "Retrieved all users" : "No users found";
                CreateResponse(msg, HttpStatusCode.OK, users, true);
                return Ok(_res);
            }
            catch (Exception e) 
            {
                CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                return StatusCode((int)HttpStatusCode.InternalServerError, _res);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserRegistrationDto newUser)
        {
            try
            {
                bool emailExists = await _repo.EmailExists(newUser.Email);
                if (emailExists)
                {
                    CreateResponse("This email is already in use", HttpStatusCode.Conflict);
                    return Conflict(_res);
                }

                if (!UserUtils.IsValidEmail(newUser.Email))
                {
                    CreateResponse("Email is not valid", HttpStatusCode.BadRequest);
                    return BadRequest(_res);
                }

                if (!UserUtils.IsPasswordValid(newUser.Password))
                {
                    CreateResponse("Password is not valid", HttpStatusCode.BadRequest);
                    return BadRequest(_res);
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
                CreateResponse("Registration successful", HttpStatusCode.Created, createdUser, true);
                return CreatedAtAction(nameof(Register), _res);
            }
            catch (Exception e) 
            {
                CreateResponse(e.Message, HttpStatusCode.InternalServerError, e);
                return StatusCode((int)HttpStatusCode.InternalServerError, _res);
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
