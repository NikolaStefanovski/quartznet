using AutoMapper;
using Quartz.Examples.AspNetCore.CRC_API.Models;
using Quartz.Examples.AspNetCore.DBModels;

namespace Quartz.Examples.AspNetCore.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDTO, Employee>().ReverseMap();

            CreateMap<ExtraFieldDTO, EmployeeExtraField>().ReverseMap();
        }
    }
}
