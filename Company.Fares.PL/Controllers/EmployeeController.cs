using AutoMapper;
using Company.Fares.BLL.Interfaces;
using Company.Fares.BLL.Repositories;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;
using Company.Fares.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Company.Fares.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeInterface _EmployeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeInterface EmployeeRepository,
            //IDepartmentRepository departmentRepository
            IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_EmployeeRepository = EmployeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
            // View ->
            // Dictionary : Key ,Data
            // 1 viewData : Transfer Extra Info. from Controller "Action" to View
            //ViewData["Message"] = "Hello From ViewData";
            // 2 ViewBag  : Transfer Extra Info. from Controller "Action" to View
            //ViewBag.Message = "Hello From ViewBag";
            // 3 TempData : 
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {

                    TempData["Message"] = "Employee is Created";
                    return RedirectToAction("Index");
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var result = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;
            ViewData["id"] = id;
            if (result is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id: {id} is Not found" });
            var dto = _mapper.Map<CreateEmployeeDto>(result);
            return View(dto);
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var result = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;

            if (result is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id: {id} is Not found" });
            var dto = _mapper.Map<CreateEmployeeDto>(result);

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                // when the Employee model
                //if (id == model.Id)
                //{
                //    if(model.ImageName is not null )
                //    {
                //        DocumentSettings.DeleteFile(model.ImageName, "images");
                //    }
                //    if(model.Image)
                //    _unitOfWork.EmployeeRepository.Update(model);
                //    var result = _unitOfWork.Complete();
                //    if (result > 0)
                //    {
                //        return RedirectToAction("Index");
                //    }
                //}
                //else
                //    return BadRequest();
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                var employee = _mapper.Map<Employee>(model);

                if (model.Image is not null)
                {
                    employee.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var result = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = department;
            if (result is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id: {id} is Not found" });
            var employee = _mapper.Map<CreateEmployeeDto>(result);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id == model.Id)
                //{
                //    _unitOfWork.EmployeeRepository.Delete(model);
                //    var result = _unitOfWork.Complete();
                //    if (result > 0)
                //    {
                //        if(model.ImageName is not null)
                //        {
                //            DocumentSettings.DeleteFile(model.ImageName, "images");
                //        }
                //        return RedirectToAction("Index");
                //    }
                //}
                //else
                //    return BadRequest();
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
    }
}
