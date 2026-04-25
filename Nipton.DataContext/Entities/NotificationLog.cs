using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Entities
{
    public class NotificationLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
