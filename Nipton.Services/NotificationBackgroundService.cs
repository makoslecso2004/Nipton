using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nipton.DataContext;
using Nipton.DataContext.Context;
using Nipton.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nipton.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var upcomingCourses = context.Courses
                            .Include(c => c.Subject)
                            .Include(c => c.Schedules)
                            .Where(c => c.Schedules.Any(s => s.StartTime > DateTime.Now && s.StartTime <= DateTime.Now.AddMinutes(30)))
                            .ToList();

                        foreach (var course in upcomingCourses)
                        {
                            var participants = context.CourseStudents
                                .Where(cs => cs.CourseId == course.Id)
                                .Select(cs => cs.Student)
                                .ToList();

                            foreach (var user in participants)
                            {
                                var firstSchedule = course.Schedules
                                    ?.Where(s => s.StartTime > DateTime.Now && s.StartTime <= DateTime.Now.AddMinutes(30))
                                    .FirstOrDefault();

                                if (firstSchedule != null)
                                {
                                    // Ellenőrizd, hogy van-e már értesítés az adott kurzushoz és felhasználóhoz
                                    bool notificationExists = context.NotificationLogs
                                        .Any(n => n.UserId == user.Id && n.CourseId == course.Id && n.Message.Contains(firstSchedule.StartTime.ToString()));

                                    if (!notificationExists)
                                    {
                                        // Log notification to database
                                        context.NotificationLogs.Add(new NotificationLog
                                        {
                                            UserId = user.Id,
                                            CourseId = course.Id,
                                            Message = $"Reminder: Your course '{course.Subject?.Name}' starts at {firstSchedule.StartTime}.",
                                            SentAt = DateTime.Now // Adj hozzá egy időbélyeget, ha szükséges
                                        });
                                    }
                                }
                            }

                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while generating notifications.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}