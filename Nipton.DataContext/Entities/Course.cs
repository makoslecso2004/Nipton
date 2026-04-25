using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public int MaxStudents { get; set; }
        public string Type { get; set; }
        public string Form { get; set; }

        public int Hours { get; set; }
        public bool IsWeeklyHours { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ICollection<CourseTeacher> Teachers { get; set; }
        public ICollection<CourseStudent> Students { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
