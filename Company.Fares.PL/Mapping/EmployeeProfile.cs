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
            CreateMap<CreateEmployeeDto, Employee>().ForMember(D => D.DepartmentId, S => S.MapFrom(O => O.DepartmentId));
            CreateMap<Employee, CreateEmployeeDto>();

        }
    }
}
