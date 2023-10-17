using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.VisualBasic;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));   
            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<Project, ProjectDto>();

            CreateMap<Department, DepartmentDto>();
        }
    }
}
