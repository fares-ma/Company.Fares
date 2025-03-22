using Company.Fares.BLL.Interfaces;
using Company.Fares.DAL.Data.Contexts;
using Company.Fares.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Fares.BLL.Repositories
{
    public class EmplyeeRepository : IEmployeeRepository
    {
        private readonly CompanyDbContext _Context;

        public EmplyeeRepository(CompanyDbContext context)
        {
            _Context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
          return _Context.Employees.ToList();
        }

        public Employee? Get(int id)
        {
            return _Context.Employees.Find();
        }

        public int Add(Employee model)
        {
             _Context.Employees.Add(model);
            return _Context.SaveChanges();
        }

        public int Update(Employee model)
        {
            _Context.Employees.Update(model);
            return _Context.SaveChanges();
        }

        public int Delete(Employee model)
        {
            _Context.Employees.Remove(model);
            return _Context.SaveChanges();
        }

      

    }
}
