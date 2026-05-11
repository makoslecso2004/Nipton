using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Entities
{
    public class SubjectPrerequisite
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int PrerequisiteSubjectId { get; set; }
        public Subject PrerequisiteSubject { get; set; }
    }
}
