using Company.Fares.BLL.Interfaces;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Company.Fares.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {


                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result

                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result

                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }


            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? Id)
        {

            if (Id is null) return BadRequest("Invalid Id"); //400

            var user = await _userManager.FindByIdAsync(Id);

            if (user is null) return NotFound(new { statusCode = 404, message = $"User With Id : {Id} is not Found" });

            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            ViewBag.EmployeeId = Id;

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? Id, string ViewName = "Edit")
        {


            if (Id is null) return BadRequest("Invalid Id"); //400

            var user = await _userManager.FindByIdAsync(Id);



            if (user is null) return NotFound(new { statusCode = 404, message = $"employee With Id : {Id} is not Found" });

            //var employeesDto = new CreateEmployeeDto()
            //{

            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Age = employee.Age,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary

            //};
            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(ViewName, dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string Id, UserToReturnDto model, string ViewName = "Edit")
        {

            if (ModelState.IsValid)
            {
                if (Id != model.Id) return BadRequest("Invalid Operation");

                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return BadRequest("Invalid Operation");

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }


            }

            return View(ViewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? Id)
        {


            return await Edit(Id, "Delete");
        }

        [HttpPost]

        public async Task<IActionResult> Delete([FromRoute] string Id, UserToReturnDto model)
        {

            if (ModelState.IsValid)
            {


                if (Id != model.Id) return BadRequest("Invalid Operation");

                var user = await _userManager.FindByIdAsync(Id);

                if (user is null) return BadRequest("Invalid Operation");



                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
