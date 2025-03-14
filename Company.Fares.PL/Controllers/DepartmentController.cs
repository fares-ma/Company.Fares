using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Fares.PL.Controllers
{
    // MVC Controller for Department
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet] // GET: / Department/ Index
        public IActionResult Index()
        {

           var departments = _departmentRepository.GetAll();

            return View(departments);
        }
    }
}
