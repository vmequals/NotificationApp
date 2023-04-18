using Microsoft.EntityFrameworkCore;
using NotificationApp.Data;
using NotificationApp.Models;


namespace NotificationApp.Services
{
    public class NotificationService
    {
        private readonly NotificationDbContext _dbContext;

        public NotificationService(NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Notification> CreateNotificationAsync(string message)
        {
            var notification = new Notification { Message = message, CreatedAt = DateTime.UtcNow };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            return notification;
        }

        public async Task SendNotificationToUsersAsync(int notificationId, List<string> userIds)
        {
            var userNotifications = userIds.Select(userId => new UserNotification
            {
                UserId = userId,
                NotificationId = notificationId,
                IsRead = false
            });

            _dbContext.UserNotifications.AddRange(userNotifications);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserNotification>> GetUserNotificationsAsync(string userId)
        {
            return await _dbContext.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<UserNotification>> GetUnreadUserNotificationsAsync(string userId)
        {
            return await _dbContext.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToListAsync();
        }
        public async Task MarkNotificationAsReadAsync(string userId, int notificationId)
        {
            var userNotification = await _dbContext.UserNotifications
                .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notificationId);

            if (userNotification != null)
            {
                userNotification.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

//
// using Microsoft.EntityFrameworkCore;
// using NotificationApp.Data;
// using NotificationApp.Models;
// using System.IdentityModel.Tokens.Jwt;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;
//
//
// namespace NotificationApp.Services
// {
//     public class NotificationService
//     {
//         private readonly NotificationDbContext _dbContext;
//
//         public NotificationService(NotificationDbContext dbContext)
//         {
//             _dbContext = dbContext;
//         }
//
//         public async Task<Notification> CreateNotificationAsync(string message)
//         {
//             var notification = new Notification { Message = message, CreatedAt = DateTime.UtcNow };
//             _dbContext.Notifications.Add(notification);
//             await _dbContext.SaveChangesAsync();
//
//             return notification;
//         }
//         public async Task<List<UserNotification>> GetUnreadUserNotificationsAsync(string token, string userId)
//         {
//             var handler = new JwtSecurityTokenHandler();
//             var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")),
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ClockSkew = TimeSpan.Zero
//             }, out SecurityToken validatedToken);
//
//             if (claimsPrincipal == null)
//             {
//                 throw new SecurityTokenException("Invalid token");
//             }
//
//             return await _dbContext.UserNotifications
//                 .Include(un => un.Notification)
//                 .Where(un => un.UserId == userId && !un.IsRead)
//                 .ToListAsync();
//         }
//
//         public async Task SendNotificationToUsersAsync(int notificationId, List<string> userIds)
//         {
//             var userNotifications = userIds.Select(userId => new UserNotification
//             {
//                 UserId = userId,
//                 NotificationId = notificationId,
//                 IsRead = false
//             });
//
//             _dbContext.UserNotifications.AddRange(userNotifications);
//             await _dbContext.SaveChangesAsync();
//         }
//
//         public async Task<List<UserNotification>> GetUserNotificationsAsync(string userId)
//         {
//             return await _dbContext.UserNotifications
//                 .Include(un => un.Notification)
//                 .Where(un => un.UserId == userId)
//                 .ToListAsync();
//         }
//         public async Task<List<UserNotification>> GetUserNotificationsAsync(string token, string userId)
//         {
//             var handler = new JwtSecurityTokenHandler();
//             var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")),
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ClockSkew = TimeSpan.Zero
//             }, out SecurityToken validatedToken);
//
//             if (claimsPrincipal == null)
//             {
//                 throw new SecurityTokenException("Invalid token");
//             }
//
//             return await _dbContext.UserNotifications
//                 .Include(un => un.Notification)
//                 .Where(un => un.UserId == userId)
//                 .ToListAsync();
//         }
//         public async Task MarkNotificationAsReadAsync(string token, string userId, int notificationId)
//         {
//             var handler = new JwtSecurityTokenHandler();
//             var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
//             {
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")),
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ClockSkew = TimeSpan.Zero
//             }, out SecurityToken validatedToken);
//
//             if (claimsPrincipal == null)
//             {
//                 throw new SecurityTokenException("Invalid token");
//             }
//
//             var userNotification = await _dbContext.UserNotifications
//                 .FirstOrDefaultAsync(un => un.UserId == userId && un.NotificationId == notificationId);
//
//             if (userNotification != null)
//             {
//                 userNotification.IsRead = true;
//                 await _dbContext.SaveChangesAsync();
//             }
//         }
//     }
// }