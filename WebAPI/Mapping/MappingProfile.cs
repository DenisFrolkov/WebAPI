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
            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectForCreationDto, Project>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<ProjectForUpdateDto, Project>().ReverseMap();

            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentForCreationDto, Department>();
            CreateMap<DepartmentForUpdateDto, Department>();
            CreateMap<DepartmentForUpdateDto, Department>().ReverseMap();

            CreateMap<UserForRegistrationDto, User>();

        }
    }
}
