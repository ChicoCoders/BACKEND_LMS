using Azure.Core;
using LMS.DTOs;
using LMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("NewNotice")]
        public async Task<bool> NewNotice(NewNoticeDto newnotice)
        {
            return await _notificationService.NewNotice(newnotice);
        }

        [HttpPost("SetRemind")]
        public async Task<bool> SetRemind(Reservation reservation)
        {
            return await _notificationService.SetRemind(reservation);
        }

        [HttpGet("GetNotificatons")]
        public async Task<List<NewNoticeDto>> GetNotification(string username)
        {
           var httpContext = HttpContext;
           return await _notificationService.GetNotification(username,httpContext);
        }

        [HttpDelete("RemoveNotification")]
        public async Task<bool> RemoveNotification(int id)
        {
            return await _notificationService.RemoveNotification(id);
        }
    }
}
