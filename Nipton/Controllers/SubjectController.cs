using Microsoft.AspNetCore.Mvc;
using Nipton.Services;
using Nipton.DataContext.Dtos;

namespace Nipton.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectDto>>> GetAll()
        {
            try
            {
                var result = await _subjectService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{subjectId}")]
        public async Task<ActionResult<SubjectDto>> GetById(int subjectId)
        {
            try
            {
                var result = await _subjectService.GetByIdAsync(subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDto>> Create(SubjectCreateDto dto)
        {
            try
            {
                var result = await _subjectService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{subjectId}")]
        public async Task<ActionResult<SubjectDto>> Update(int subjectId, SubjectUpdateDto dto)
        {
            try
            {
                var result = await _subjectService.UpdateAsync(subjectId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{subjectId}")]
        public async Task<ActionResult> Deactivate(int subjectId)
        {
            try
            {
                await _subjectService.DeactivateAsync(subjectId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{subjectId}")]
        public async Task<ActionResult> Reactivate(int subjectId)
        {
            try
            {
                await _subjectService.ReactivateAsync(subjectId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{subjectId}")]
        public async Task<ActionResult> RegisterToSubject(int subjectId, CourseRegisterDto dto)
        {
            try
            {
                await _subjectService.RegisterToSubjectAsync(subjectId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{subjectId}")]
        public async Task<ActionResult> UnregisterFromSubject(int subjectId, CourseUnregisterDto dto)
        {
            try
            {
                await _subjectService.UnregisterFromSubjectAsync(subjectId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{subjectId}")]
        public async Task<ActionResult<List<UserDto>>> GetStudentsForSubject(int subjectId, [FromQuery] string semester)
        {
            try
            {
                var result = await _subjectService.GetStudentsForSubjectAsync(subjectId, semester);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}