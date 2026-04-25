using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Entities
{
    public class CourseTeacher
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int TeacherId { get; set; }
        public User Teacher { get; set; }
    }
}
