using AutoMapper;
using Quartz.Examples.AspNetCore.CRC_API.DTO;
using Quartz.Examples.AspNetCore.Database.Models;

namespace Quartz.Examples.AspNetCore.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<ExtraFieldDTO, EmployeeExtraField>().ReverseMap();
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.EmployeeExtraFields, mapper => mapper.MapFrom(source => source.ExtraFields))
                .ReverseMap();
            CreateMap<EmployeePresenceDTO, EmployeePresence>().ReverseMap();

        }
    }
}
