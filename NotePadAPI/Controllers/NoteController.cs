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
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _repo;

        public NoteController(INoteRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> GetNotesForUser([FromBody] string email)
        {
            ApiResponse response;
            try
            {
                IEnumerable<Note> notes = await _repo.GetNotesForUser(email);
                string msg = notes.Any() ? $"{notes.Count()} Notes found for this user" : "No note found for this user";
                response = Utility.CreateResponse(msg, HttpStatusCode.OK, notes, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = Utility.CreateResponse(ex.Message, HttpStatusCode.InternalServerError);
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetNoteForUser(int id) 
        {
            ApiResponse res;
            try
            {
                Note? note = await _repo.GetNoteForUser(id);

                if (note == null)
                {
                    res = Utility.CreateResponse("Note not found.", HttpStatusCode.NotFound);
                    return NotFound(res);
                }
                res = Utility.CreateResponse("Retrieved note.", HttpStatusCode.OK, note, true);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res = Utility.CreateResponse(ex.Message, HttpStatusCode.InternalServerError);
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        //// GET api/<NoteController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<NoteController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<NoteController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<NoteController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
