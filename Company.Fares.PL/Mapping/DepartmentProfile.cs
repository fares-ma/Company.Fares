using AutoMapper;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;

namespace Company.Fares.PL.Mapping
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, CreateDepartmentDto>().ReverseMap();
            CreateMap<Department, UpdateDepartmentDto>().ReverseMap();
        }
    }
}
