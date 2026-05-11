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

        // Prerequisite management
        Task AddPrerequisiteAsync(int subjectId, PrerequisiteAddDto dto);
        Task RemovePrerequisiteAsync(int subjectId, PrerequisiteRemoveDto dto);
        Task<List<SubjectDto>> GetPrerequisitesAsync(int subjectId);
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
            var subjects = await _context.Subjects
                .Include(s => s.Prerequisites)
                .ToListAsync();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Prerequisites)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> CreateAsync(SubjectCreateDto dto)
        {
            var subject = _mapper.Map<Subject>(dto);
            subject.IsActive = true;

            // Add subject first so it has an Id when adding prerequisites
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            if (dto.PrerequisiteIds != null && dto.PrerequisiteIds.Any())
            {
                foreach (var preId in dto.PrerequisiteIds.Distinct())
                {
                    var pre = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == preId);
                    if (pre == null) throw new Exception($"Előfeltételként megadott tantárgy nem található: {preId}");

                    _context.SubjectPrerequisites.Add(new SubjectPrerequisite
                    {
                        SubjectId = subject.Id,
                        PrerequisiteSubjectId = preId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> UpdateAsync(int id, SubjectUpdateDto dto)
        {
            var subject = await _context.Subjects
                .Include(s => s.Prerequisites)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) throw new Exception("Tantárgy nem található!");

            subject.Code = dto.Code;
            subject.Name = dto.Name;
            subject.Credits = dto.Credits;

            // Update prerequisites: replace existing with provided list
            if (dto.PrerequisiteIds != null)
            {
                var existing = subject.Prerequisites.ToList();
                _context.SubjectPrerequisites.RemoveRange(existing);

                foreach (var preId in dto.PrerequisiteIds.Distinct())
                {
                    var pre = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == preId);
                    if (pre == null) throw new Exception($"Előfeltételként megadott tantárgy nem található: {preId}");

                    _context.SubjectPrerequisites.Add(new SubjectPrerequisite
                    {
                        SubjectId = subject.Id,
                        PrerequisiteSubjectId = preId
                    });
                }
            }

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

            var subject = await _context.Subjects
                .Include(s => s.Prerequisites)
                .FirstOrDefaultAsync(s => s.Id == subjectId && s.IsActive);

            if (subject == null) throw new Exception("Aktív tantárgy nem található!");

            var courses = await _context.Courses
                .Include(c => c.Students)
                .Where(c => c.SubjectId == subjectId)
                .ToListAsync();

            if (!courses.Any()) throw new Exception("Nincsenek elérhető kurzusok ehhez a tárgyhoz!");

            // Szűrés a dto.Semester alapján
            var semesterCourses = courses.Where(c => c.Semester == dto.Semester).ToList();

            // Az összes kurzustípus az adott szemeszterben
            var requiredCourseTypes = semesterCourses.Select(c => c.Type).Distinct().ToList();

            // Ellenőrizzük, hogy minden szükséges kurzustípusra jelentkezett-e az adott szemeszterben
            var selectedCourses = semesterCourses.Where(c => dto.CourseIds.Contains(c.Id)).ToList();
            var selectedCourseTypes = selectedCourses.Select(c => c.Type).Distinct().ToList();

            foreach (var requiredType in requiredCourseTypes)
            {
                if (!selectedCourseTypes.Contains(requiredType))
                {
                    throw new Exception($"A(z) {requiredType} típusú kurzusra is jelentkezni kell a(z) {dto.Semester} szemeszterben!");
                }
            }

            // Előfeltétel ellenőrzés: ha vannak előfeltételek, a hallgatónak meg kell lennie a passznak (CourseStudent.Passed=true) az előfeltétel tantárgyakból
            if (subject.Prerequisites != null && subject.Prerequisites.Any())
            {
                var prereqIds = subject.Prerequisites.Select(p => p.PrerequisiteSubjectId).Distinct().ToList();

                // Ellenőrizzük, hogy a hallgató átvizsgázott-e a szükséges tantárgyakból (bármelyik kurzusból ahol Passed == true)
                foreach (var prereqId in prereqIds)
                {
                    var passed = await _context.CourseStudents
                        .Include(cs => cs.Course)
                        .Where(cs => cs.StudentId == dto.StudentId && cs.Passed == true && cs.Course.SubjectId == prereqId)
                        .AnyAsync();

                    if (!passed)
                        throw new Exception($"A tárgy felvételéhez teljesíteni kell az előfeltételt: tantárgyId={prereqId}");
                }
            }

            // Félév ellenőrzés: Egy tárgyat nem lehet ugyanabban a félévben felvenni, amikor az előfeltételet is felvették
            if (subject.Prerequisites != null && subject.Prerequisites.Any())
            {
                var prereqIds = subject.Prerequisites.Select(p => p.PrerequisiteSubjectId).Distinct().ToList();

                foreach (var prereqId in prereqIds)
                {
                    var prereqCourse = await _context.CourseStudents
                        .Include(cs => cs.Course)
                        .Where(cs => cs.StudentId == dto.StudentId && cs.Course.SubjectId == prereqId)
                        .OrderByDescending(cs => cs.Course.Semester) // Legutóbbi félév
                        .FirstOrDefaultAsync();

                    if (prereqCourse != null && prereqCourse.Course.Semester == dto.Semester)
                    {
                        throw new Exception($"A tárgyat nem lehet ugyanabban a félévben felvenni, amikor az előfeltételét is felvették: tantárgyId={prereqId}");
                    }
                }
            }

            // Validációk a specifikáció alapján
            foreach (var course in selectedCourses)
            {
                if (course.Students.Count >= course.MaxStudents)
                    throw new Exception($"A(z) {course.CourseCode} kurzus megtelt!");

                if (course.Form != "Undefined" && course.Form != student.StudentStudyForm)
                    throw new Exception($"A hallgató tagozata ({student.StudentStudyForm}) nem egyezik a kurzus tagozatával ({course.Form})!");
            }

            // Hozzáadás
            foreach (var courseId in dto.CourseIds)
            {
                _context.CourseStudents.Add(new CourseStudent { CourseId = courseId, StudentId = dto.StudentId, Passed = false });
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

        // Prerequisite management implementations
        public async Task AddPrerequisiteAsync(int subjectId, PrerequisiteAddDto dto)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject == null) throw new Exception("Tantárgy nem található!");

            var pre = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == dto.PrerequisiteId);
            if (pre == null) throw new Exception("Előfeltételként megadott tantárgy nem található!");

            // check not duplicate
            var exists = await _context.SubjectPrerequisites.AnyAsync(sp => sp.SubjectId == subjectId && sp.PrerequisiteSubjectId == dto.PrerequisiteId);
            if (exists) throw new Exception("Már létezik ilyen előfeltétel!");

            _context.SubjectPrerequisites.Add(new SubjectPrerequisite { SubjectId = subjectId, PrerequisiteSubjectId = dto.PrerequisiteId });
            await _context.SaveChangesAsync();
        }

        public async Task RemovePrerequisiteAsync(int subjectId, PrerequisiteRemoveDto dto)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject == null) throw new Exception("Tantárgy nem található!");

            SubjectPrerequisite toRemove = null;
            if (dto.PrerequisiteId.HasValue)
            {
                toRemove = await _context.SubjectPrerequisites.FirstOrDefaultAsync(sp => sp.SubjectId == subjectId && sp.PrerequisiteSubjectId == dto.PrerequisiteId.Value);
            }
            else if (!string.IsNullOrEmpty(dto.PrerequisiteCode))
            {
                var pre = await _context.Subjects.FirstOrDefaultAsync(s => s.Code == dto.PrerequisiteCode);
                if (pre == null) throw new Exception("Előfeltételként megadott tantárgy nem található a kód alapján!");
                toRemove = await _context.SubjectPrerequisites.FirstOrDefaultAsync(sp => sp.SubjectId == subjectId && sp.PrerequisiteSubjectId == pre.Id);
            }
            else
            {
                throw new Exception("Adj meg egy eltávolítandó előfeltételt azonosítóval vagy kóddal.");
            }

            if (toRemove == null) throw new Exception("Nem található ilyen előfeltétel a tantárgyhoz.");

            _context.SubjectPrerequisites.Remove(toRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubjectDto>> GetPrerequisitesAsync(int subjectId)
        {
            var subject = await _context.Subjects
                .Include(s => s.Prerequisites)
                .ThenInclude(sp => sp.PrerequisiteSubject)
                .FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subject == null) throw new Exception("Tantárgy nem található!");

            var prerequisites = subject.Prerequisites.Select(sp => sp.PrerequisiteSubject).ToList();
            return _mapper.Map<List<SubjectDto>>(prerequisites);
        }
    }
}
