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
    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAllAsync();
        Task<SubjectDto> GetByIdAsync(int id);
        Task<SubjectDto> CreateAsync(SubjectCreateDto dto);
        Task<SubjectDto> UpdateAsync(int id, SubjectUpdateDto dto);
        Task DeactivateAsync(int id);
        Task ReactivateAsync(int id);

        // Feliratkozás és lejelentkezés
        Task RegisterToSubjectAsync(int subjectId, CourseRegisterDto dto);
        Task UnregisterFromSubjectAsync(int subjectId, CourseUnregisterDto dto);
        Task<List<UserDto>> GetStudentsForSubjectAsync(int subjectId, string semester);
    }

    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SubjectService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SubjectDto>> GetAllAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> CreateAsync(SubjectCreateDto dto)
        {
            var subject = _mapper.Map<Subject>(dto);
            subject.IsActive = true;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> UpdateAsync(int id, SubjectUpdateDto dto)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");

            subject.Code = dto.Code;
            subject.Name = dto.Name;
            subject.Credits = dto.Credits;

            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task DeactivateAsync(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task ReactivateAsync(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.IsActive = true;
            await _context.SaveChangesAsync();
        }

        public async Task RegisterToSubjectAsync(int subjectId, CourseRegisterDto dto)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.StudentId && u.Role == "Student" && u.IsActive);
            if (student == null) throw new Exception("Aktív hallgató nem található!");

            var courses = await _context.Courses
                .Include(c => c.Students)
                .Where(c => dto.CourseIds.Contains(c.Id) && c.SubjectId == subjectId)
                .ToListAsync();

            if (courses.Count != dto.CourseIds.Count) throw new Exception("Néhány kurzus nem létezik, vagy nem ehhez a tárgyhoz tartozik!");

            // Validációk a specifikáció alapján
            foreach (var course in courses)
            {
                if (course.Students.Count >= course.MaxStudents)
                    throw new Exception($"A(z) {course.CourseCode} kurzus megtelt!");

                if (course.Form != "Undefined" && course.Form != student.StudentStudyForm)
                    throw new Exception($"A hallgató tagozata ({student.StudentStudyForm}) nem egyezik a kurzus tagozatával ({course.Form})!");
            }

            // Hozzáadás
            foreach (var courseId in dto.CourseIds)
            {
                _context.CourseStudents.Add(new CourseStudent { CourseId = courseId, StudentId = dto.StudentId });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UnregisterFromSubjectAsync(int subjectId, CourseUnregisterDto dto)
        {
            var enrollments = await _context.CourseStudents
                .Include(cs => cs.Course)
                .Where(cs => cs.StudentId == dto.StudentId && cs.Course.SubjectId == subjectId && cs.Course.Semester == dto.Semester)
                .ToListAsync();

            if (!enrollments.Any()) throw new Exception("Nincs ilyen felvett tárgy/kurzus a megadott félévben!");

            _context.CourseStudents.RemoveRange(enrollments);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetStudentsForSubjectAsync(int subjectId, string semester)
        {
            var students = await _context.CourseStudents
                .Include(cs => cs.Course)
                .Include(cs => cs.Student)
                .Where(cs => cs.Course.SubjectId == subjectId && cs.Course.Semester == semester)
                .Select(cs => cs.Student)
                .Distinct()
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(students);
        }
    }
}
