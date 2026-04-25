using AutoMapper;
using Nipton.DataContext.Dtos;
using Nipton.DataContext.Entities;

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

            CreateMap<Subject, SubjectDto>().ReverseMap();
            CreateMap<SubjectCreateDto, Subject>();
            CreateMap<SubjectUpdateDto, Subject>()
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
