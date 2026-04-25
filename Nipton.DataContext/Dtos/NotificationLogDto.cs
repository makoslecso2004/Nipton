using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Dtos
{
    public class NotificationLogDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
