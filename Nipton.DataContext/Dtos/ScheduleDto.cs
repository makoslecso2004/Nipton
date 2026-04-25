using System;
using System.Collections.Generic;
using System.Text;

namespace Nipton.DataContext.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class ScheduleCreateDto
    {
        public List<TimeSlotDto> TimeSlots { get; set; }
    }

    public class ScheduleModifyDto
    {
        public List<TimeSlotDto> TimeSlots { get; set; }
    }

    public class TimeSlotDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
