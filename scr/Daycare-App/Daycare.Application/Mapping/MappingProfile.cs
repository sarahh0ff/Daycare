using AutoMapper;
using Daycare.Application.DTOs;
using Daycare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Daycare.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Child, ChildDto>();
            CreateMap<ChildDto, Child>();
        }
    }
}
