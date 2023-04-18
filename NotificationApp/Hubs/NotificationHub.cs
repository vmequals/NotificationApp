// Hubs/NotificationHub.cs

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NotificationApp.Services;

namespace NotificationApp.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly NotificationService _notificationService;

        public NotificationHub(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task SendNotification(string message, List<string> userIds)
        {
            var notification = await _notificationService.CreateNotificationAsync(message);
            await _notificationService.SendNotificationToUsersAsync(notification.Id, userIds);

            foreach (var userId in userIds)
            {
                await Clients.User(userId).SendAsync("ReceiveNotification", notification);
            }
        }
    }
}