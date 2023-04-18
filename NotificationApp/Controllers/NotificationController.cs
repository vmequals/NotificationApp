using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationApp.Hubs;
using NotificationApp.Services;

namespace NotificationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(NotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(List<string> userIds, string message)
        {
            var notification = await _notificationService.CreateNotificationAsync(message);
            await _notificationService.SendNotificationToUsersAsync(notification.Id, userIds);

            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
            }

            return Ok();
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications(string userId)
        {
            var notifications = await _notificationService.GetUnreadUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("markAsRead")]
        public async Task<IActionResult> MarkNotificationAsRead(string userId, int notificationId)
        {
            await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);
            return Ok();
        }
    }
}