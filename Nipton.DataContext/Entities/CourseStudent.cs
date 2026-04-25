using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Entities
{
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }
    }
}
