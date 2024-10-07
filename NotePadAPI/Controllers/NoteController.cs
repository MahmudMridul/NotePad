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
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _repo;
        private readonly ILogger<NoteController> _logger;
        private string _controller => ControllerContext.ActionDescriptor.ControllerName;

        public NoteController(INoteRepository repo, ILogger<NoteController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> GetNotesForUser([FromBody] string email)
        {
            ApiResponse response;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                IEnumerable<Note> notes = await _repo.GetNotesForUser(email);
                string msg = notes.Any() ? $"{notes.Count()} Notes found for this user" : "No note found for this user";
                response = Utility.CreateResponse(msg, HttpStatusCode.OK, notes, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = Utility.CreateResponse(ex.Message, HttpStatusCode.InternalServerError);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(response)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetNote(int id) 
        {
            ApiResponse res;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                Note? note = await _repo.GetNoteById(id);

                if (note == null)
                {
                    res = Utility.CreateResponse("Note not found.", HttpStatusCode.NotFound);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                    return NotFound(res);
                }
                res = Utility.CreateResponse("Retrieved note.", HttpStatusCode.OK, note, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return Ok(res);
            }
            catch (Exception ex)
            {
                res = Utility.CreateResponse(ex.Message, HttpStatusCode.InternalServerError);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse>> CreateNote([FromBody] CreateNoteDto noteDto)
        {
            ApiResponse res;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                if (noteDto == null || noteDto.UserId <= 0 || string.IsNullOrEmpty(noteDto.Title) || string.IsNullOrEmpty(noteDto.Description)) 
                {
                    res = Utility.CreateResponse("No data found", HttpStatusCode.BadRequest);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                    return BadRequest(res);
                }
                Note note = new Note
                {
                    Title = noteDto.Title,
                    Description = noteDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    UserId = noteDto.UserId,
                };
                await _repo.CreateNote(note);
                res = Utility.CreateResponse("Note created", HttpStatusCode.OK, note, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.Created, res);
            }
            catch (Exception e)
            {
                res = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<ApiResponse>> EditNote(int id, [FromBody] EditNoteDto dto)
        {
            ApiResponse res;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Description))
                {
                    res = Utility.CreateResponse("No data found", HttpStatusCode.BadRequest);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                    return BadRequest(res);
                }

                Note? note = await _repo.GetNoteById(id);
                if (note == null)
                {
                    res = Utility.CreateResponse("Note doesn't exist", HttpStatusCode.NotFound);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                    return NotFound(res);
                }

                note.Title = dto.Title;
                note.Description = dto.Description;
                note.LastUpdatedAt = DateTime.UtcNow;

                await _repo.UpdateNote(note);
                res = Utility.CreateResponse("Note updated", HttpStatusCode.OK, note, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return Ok(res);
            }
            catch (Exception e) 
            {
                res = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteNote(int id)
        {
            ApiResponse res;
            string apiName = ControllerContext.ActionDescriptor.ActionName;
            try
            {
                Note? note = await _repo.GetNoteById(id);
                if (note == null)
                {
                    res = Utility.CreateResponse("Note doesn't exist", HttpStatusCode.NotFound);
                    _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                    return NotFound(res);
                }

                await _repo.DeleteNote(note);
                res = Utility.CreateResponse("Note deleted", HttpStatusCode.OK, note, true);
                _logger.LogInformation($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.Gone, res);
            }
            catch (Exception e)
            {
                res = Utility.CreateResponse(e.Message, HttpStatusCode.InternalServerError);
                _logger.LogError($"{_controller}/{apiName} - {Utility.ResponseToString(res)}");
                return StatusCode((int)HttpStatusCode.InternalServerError, res);
            }
        }
    }
}
