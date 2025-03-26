using AutoMapper;
using Company.Fares.BLL.Interfaces;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Fares.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // ASK CLR Create Object From IEmployeeRepository
        public EmployeeController(
            IEmployeeRepository employeeRepository ,
            //IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        [HttpGet] // GET: / Department/ Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeeRepository.GetAll();
            }
            else
            {
                 employees = _employeeRepository.GetByName(SearchInput);
            }
            
            //// Dictionary  : 3 Property
            //// 1. ViewData : Transfer Extra Infomation From Controller (Action)To View 
            //ViewData["Message"] = "Hello From ViewData";


            //// 2. ViewBag  : Transfer Extra Infomation From Controller (Action)To View 
            //ViewBag.Message = new {Message = "Hello From ViewBag" };


            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {


          //var departments =  _departmentRepository.GetAll();
          //  ViewData["departments"] = departments;
            return View();

        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                try
                {
                    // Manual Mapping
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Address = model.Address,
                    //    Age = model.Age,
                    //    CreateAt = model.CreateAt,
                    //    HiringDate = model.HiringDate,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    DepartmentId = model.DepartmentId

                    //};

                   var employee = _mapper.Map<Employee>(model);

                    var count = _employeeRepository.Add(employee);
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Created !!";

                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(model);
           

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); //400

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id:{id} is Not Found" });
           
            
            return View(employee);



        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id"); //400
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee With Id:{id} is Not Found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(); //400 

                var count = _employeeRepository.Update(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Code = model.Name,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt
        //        };
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); //400

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id:{id} is Not Found" });

            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(); //400 
                var count = _employeeRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
