using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Dtos
{
    public class CourseDto
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
        public string SubjectCode { get; set; }

        public List<int> TeacherIds { get; set; }
    }

    public class CourseCreateDto
    {
        public string CourseCode { get; set; }
        public string SubjectCode { get; set; }
        public string Semester { get; set; }
        public int MaxStudents { get; set; }
        public string Type { get; set; }
        public string Form { get; set; }
        public int Hours { get; set; }
        public bool IsWeeklyHours { get; set; }

        public List<int> TeacherIds { get; set; }
    }

    public class CourseUpdateDto
    {
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public int MaxStudents { get; set; }
        public string Type { get; set; }
        public string Form { get; set; }
        public int Hours { get; set; }
        public bool IsWeeklyHours { get; set; }
    }

    public class CourseRegisterDto
    {
        public int StudentId { get; set; }
        public List<int> CourseIds { get; set; }
    }

    public class CourseUnregisterDto
    {
        public int StudentId { get; set; }
        public string Semester { get; set; }
    }

    public class CourseChangeDto
    {
        public int StudentId { get; set; }
        public int FromCourseId { get; set; }
        public int ToCourseId { get; set; }
    }
}
