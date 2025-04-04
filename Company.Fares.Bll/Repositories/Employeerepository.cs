using Company.Fares.BLL.Interfaces;
using Company.Fares.DAL.Data.Contexts;
using Company.Fares.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Fares.BLL.Repositories
{
    public class Employeerepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public Employeerepository(CompanyDbContext context) : base(context) // ASK CLR To Create object from DbContext
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
