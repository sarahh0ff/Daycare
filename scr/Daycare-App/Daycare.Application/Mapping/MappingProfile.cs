
using AutoMapper;
using Daycare.Domain.Entities;
using Daycare.Application.DTOs;


namespace Daycare.Application.Mapping;


public class DaycareProfile : Profile
{
    public DaycareProfile()
    {
        // CHILD
        CreateMap<ChildDto, Child>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ReverseMap();                                   

        // GUARDIAN
        CreateMap<GuardianDto, Guardian>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

        // ACTIVITY
        CreateMap<ActivityDto, Activity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

        // ATTENDANCE
        CreateMap<AttendanceDto, Attendance>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
        

    }
}

