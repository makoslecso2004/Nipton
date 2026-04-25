using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Dtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public bool IsActive { get; set; }
    }

    public class SubjectCreateDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
    }

    public class SubjectUpdateDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
    }
}
