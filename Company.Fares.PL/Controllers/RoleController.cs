using Company.Fares.DAL.Models;
using Company.Fares.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Fares.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManger = roleManager;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {


                roles = _roleManger.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name


                });
            }
            else
            {
                roles = _roleManger.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name

                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }


            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {

                var role = await _roleManger.FindByNameAsync(model.Name);

                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };

                    var result = await _roleManger.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? Id)
        {

            if (Id is null) return BadRequest("Invalid Id"); //400

            var role = await _roleManger.FindByIdAsync(Id);

            if (role is null) return NotFound(new { statusCode = 404, message = $"User With Id : {Id} is not Found" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name
            };

            ViewBag.EmployeeId = Id;

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? Id, string ViewName = "Edit")
        {


            if (Id is null) return BadRequest("Invalid Id"); //400

            var role = await _roleManger.FindByIdAsync(Id);



            if (role is null) return NotFound(new { statusCode = 404, message = $"employee With Id : {Id} is not Found" });


            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(ViewName, dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string Id, RoleToReturnDto model, string ViewName = "Edit")
        {

            if (ModelState.IsValid)
            {
                if (Id != model.Id) return BadRequest("Invalid Operation");

                var role = await _roleManger.FindByIdAsync(Id);

                if (role is null) return BadRequest("Invalid Operation");

                var roleResult = await _roleManger.FindByNameAsync(model.Name);

                if (roleResult is null)
                {

                    role.Name = model.Name;

                    var result = await _roleManger.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Operations !");
            }

            return View(ViewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? Id)
        {


            return await Edit(Id, "Delete");
        }

        [HttpPost]

        public async Task<IActionResult> Delete([FromRoute] string Id, RoleToReturnDto model)
        {

            if (ModelState.IsValid)
            {


                if (Id != model.Id) return BadRequest("Invalid Operation");

                var role = await _roleManger.FindByIdAsync(Id);

                if (role is null) return BadRequest("Invalid Operation");

                var roleResult = await _roleManger.FindByNameAsync(model.Name);



                var result = await _roleManger.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Invalid Operations !");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {

            var role = await _roleManger.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;


            var usersInRole = new List<UserInRoleDto>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleDto> users)
        {
            var role = await _roleManger.FindByIdAsync(roleId);

            if (role is null) return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);

                    if (appUser is not null)
                    {

                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                        }
                    }

                }

                return RedirectToAction(nameof(Edit), new { Id = roleId });


            }

            return View(users);
        }
    }
}
