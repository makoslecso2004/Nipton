using Microsoft.AspNetCore.Mvc;
using Nipton.Services;
using Nipton.DataContext.Dtos;

namespace Nipton.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CourseCreateDto dto)
        {
            try
            {
                var result = await _courseService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseDto>> GetById(int courseId)
        {
            try
            {
                var result = await _courseService.GetByIdAsync(courseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{courseId}")]
        public async Task<ActionResult<CourseDto>> Update(int courseId, CourseUpdateDto dto)
        {
            try
            {
                var result = await _courseService.UpdateAsync(courseId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<ActionResult> Delete(int courseId)
        {
            try
            {
                await _courseService.DeleteAsync(courseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeCourse(CourseChangeDto dto)
        {
            try
            {
                await _courseService.ChangeCourseAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<List<UserDto>>> GetCourseStudents(int courseId)
        {
            try
            {
                var result = await _courseService.GetStudentsAsync(courseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{courseId}")]
        public async Task<ActionResult> AddSchedule(int courseId, ScheduleCreateDto dto)
        {
            try
            {
                await _courseService.AddScheduleAsync(courseId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{courseId}")]
        public async Task<ActionResult> ModifySchedule(int courseId, ScheduleModifyDto dto)
        {
            try
            {
                await _courseService.ModifyScheduleAsync(courseId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}