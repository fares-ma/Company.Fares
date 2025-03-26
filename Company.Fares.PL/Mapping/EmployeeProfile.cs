using AutoMapper;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;

namespace Company.Fares.PL.Mapping
{
    // ClR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(d =>d.Name, o => o.MapFrom(s => $"{s.Name} Hello"));
            CreateMap<Employee, CreateEmployeeDto>();

        }
    }
}
