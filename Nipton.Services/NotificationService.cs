using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nipton.DataContext;
using Nipton.DataContext.Context;
using Nipton.DataContext.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nipton.Services
{
    public interface INotificationService
    {
        Task<List<NotificationLogDto>> GetNotificationsAsync(int? userId, int? courseId);
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NotificationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NotificationLogDto>> GetNotificationsAsync(int? userId, int? courseId)
        {
            var query = _context.NotificationLogs.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(n => n.UserId == userId.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(n => n.CourseId == courseId.Value);
            }

            var logs = await query.OrderByDescending(n => n.SentAt).ToListAsync();
            return _mapper.Map<List<NotificationLogDto>>(logs);
        }
    }
}