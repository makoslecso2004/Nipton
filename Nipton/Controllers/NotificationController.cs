using Microsoft.AspNetCore.Mvc;
using Nipton.Services;
using Nipton.DataContext.Dtos;

namespace Nipton.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationLogDto>>> GetNotifications([FromQuery] int? userId, [FromQuery] int? courseId)
        {
            try
            {
                var result = await _notificationService.GetNotificationsAsync(userId, courseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}