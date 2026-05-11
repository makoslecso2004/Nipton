using AutoMapper;
using Nipton.DataContext.Dtos;
using Nipton.DataContext.Entities;
using System.Linq;

namespace Nipton.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.PrerequisiteIds, opt => opt.MapFrom(src => src.Prerequisites != null ? src.Prerequisites.Select(p => p.PrerequisiteSubjectId).ToList() : new System.Collections.Generic.List<int>()))
                .ReverseMap();

            CreateMap<SubjectCreateDto, Subject>()
                .ForMember(dest => dest.Prerequisites, opt => opt.Ignore());

            CreateMap<SubjectUpdateDto, Subject>()
                .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src => src.Teachers.Select(t => t.TeacherId).ToList()))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.Code))
                .ReverseMap();

            CreateMap<CourseCreateDto, Course>();
            CreateMap<CourseUpdateDto, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<TimeSlotDto, Schedule>();

            CreateMap<NotificationLog, NotificationLogDto>().ReverseMap();
        }
    }
}
