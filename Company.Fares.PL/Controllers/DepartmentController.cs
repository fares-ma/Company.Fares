using AutoMapper;
using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Fares.PL.Controllers
{
    // MVC Controller for Department
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        //Ask CLR Craete object from departmentRepository
        // Dependency Injection
        public DepartmentController(/*IDepartmentRepository departmentRepository*/
            IUnitOfWork unitOfWork,
            IMapper mapper) //implement against interface not concrete class instead of "DepartmentRepository" use "IDepartmentRepository"
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet] // GET : /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server side Validation for data coming from form
            {
                //var department = new Department() // Mapping
                //{
                //    Code=model.Code,
                //    Name=model.Name,
                //    CreateAt=model.CreateAt,
                //};
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var result = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (result is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id: {id} is Not found" });
            ViewBag.DepartmentId = id;

            return View(viewName, result);
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");
            //var result = _departmentRepository.Get(id.Value);

            //if (result is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id: {id} is Not found" });

            return await Details(id, "Edit");
        }

        //[HttpPost]
        //public IActionResult Edit([FromRoute]int id,Department model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == model.Id)
        //        {
        //            var result = _departmentRepository.Update(model);
        //            if (result > 0)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        else
        //            return BadRequest();
        //    }

        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken] // htmn3 ay 7ad y3ml request mngheer elForm ya3ni PostMan Cannot use
        public async Task<IActionResult> Edit([FromRoute] int id, UpdateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.DepartmentRepository.Update(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null) return BadRequest("Invalid Id"); //400

            var departments = await _unitOfWork.DepartmentRepository.GetAsync(Id.Value);

            if (departments is null) return NotFound(new { statusCode = 404, message = $"Department With Id : {Id} is not Found" });

            var dto = new CreateDepartmentDto()
            {
                Name = departments.Name,
                Code = departments.Code,
                CreateAt = departments.CreateAt
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                _unitOfWork.DepartmentRepository.Delete(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
