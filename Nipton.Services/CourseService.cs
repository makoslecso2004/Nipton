using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nipton.DataContext;
using Nipton.DataContext.Context;
using Nipton.DataContext.Dtos;
using Nipton.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nipton.Services
{
    public interface ICourseService
    {
        Task<CourseDto> CreateAsync(CourseCreateDto dto);
        Task<CourseDto> GetByIdAsync(int id);
        Task<CourseDto> UpdateAsync(int id, CourseUpdateDto dto);
        Task DeleteAsync(int id);

        Task ChangeCourseAsync(CourseChangeDto dto);
        Task<List<UserDto>> GetStudentsAsync(int courseId);

        Task AddScheduleAsync(int courseId, ScheduleCreateDto dto);
        Task ModifyScheduleAsync(int courseId, ScheduleModifyDto dto);
    }

    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Code == dto.SubjectCode && s.IsActive);
            if (subject == null) throw new Exception("Aktív tantárgy nem található a megadott kóddal!");

            var course = _mapper.Map<Course>(dto);
            course.SubjectId = subject.Id;
            course.Teachers = new List<CourseTeacher>();

            if (dto.TeacherIds != null && dto.TeacherIds.Any())
            {
                foreach (var teacherId in dto.TeacherIds)
                {
                    course.Teachers.Add(new CourseTeacher { TeacherId = teacherId });
                }
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Kurzus nem található!");
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateAsync(int id, CourseUpdateDto dto)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) throw new Exception("Kurzus nem található!");

            _mapper.Map(dto, course);
            await _context.SaveChangesAsync();
            return _mapper.Map<CourseDto>(course);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Kurzus nem található!");

            // Specifikáció: "A kurzus csak akkor törölhető, ha nincs rajta hallgató"
            if (course.Students.Any()) throw new Exception("A kurzus nem törölhető, mert vannak feliratkozott hallgatók!");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeCourseAsync(CourseChangeDto dto)
        {
            var oldEnrollment = await _context.CourseStudents
                .Include(cs => cs.Course)
                .FirstOrDefaultAsync(cs => cs.StudentId == dto.StudentId && cs.CourseId == dto.FromCourseId);

            if (oldEnrollment == null) throw new Exception("A hallgató nincs feliratkozva a kiinduló kurzusra!");

            var newCourse = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == dto.ToCourseId);

            if (newCourse == null) throw new Exception("Cél kurzus nem található!");

            if (oldEnrollment.Course.SubjectId != newCourse.SubjectId)
                throw new Exception("Csak azonos tárgyon belüli kurzusok között lehet átjelentkezni!");

            if (oldEnrollment.Course.Type != newCourse.Type)
                throw new Exception("Csak azonos típusú (pl. elmélet -> elmélet) kurzusra lehet átjelentkezni!");

            if (newCourse.Students.Count >= newCourse.MaxStudents)
                throw new Exception("A cél kurzus betelt!");

            _context.CourseStudents.Remove(oldEnrollment);
            _context.CourseStudents.Add(new CourseStudent { CourseId = dto.ToCourseId, StudentId = dto.StudentId });

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetStudentsAsync(int courseId)
        {
            var students = await _context.CourseStudents
                .Include(cs => cs.Student)
                .Where(cs => cs.CourseId == courseId)
                .Select(cs => cs.Student)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(students);
        }

        public async Task AddScheduleAsync(int courseId, ScheduleCreateDto dto)
        {
            foreach (var slot in dto.TimeSlots)
            {
                _context.Schedules.Add(new Schedule
                {
                    CourseId = courseId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task ModifyScheduleAsync(int courseId, ScheduleModifyDto dto)
        {
            var existingSchedules = await _context.Schedules.Where(s => s.CourseId == courseId).ToListAsync();
            _context.Schedules.RemoveRange(existingSchedules); 

            foreach (var slot in dto.TimeSlots) 
            {
                _context.Schedules.Add(new Schedule
                {
                    CourseId = courseId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}