using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Project, ProjectDto>()
            .ForMember(c => c.FullDescription, opt => opt.MapFrom(x => string.Join(' ', x.Description)));

            CreateMap<Employee, EmployeeDto>();
        }
    }
}
