// Daycare.Application/Mapping/DaycareProfile.cs
using AutoMapper;
using Daycare.Domain.Entities;
using Daycare.Application.DTOs;

namespace Daycare.Application.Mapping
{
    public class DaycareProfile : Profile
    {
        public DaycareProfile()
        {
            // Entity <-> DTO

            CreateMap<ChildDto, Child>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Guardian, GuardianDto>().ReverseMap();
            CreateMap<Attendance, AttendanceDto>().ReverseMap()
             .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Activity, ActivityDto>().ReverseMap()
    .ForMember(dest => dest.Id, opt => opt.Ignore());


        }
    }
}
