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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id: {id} is Not found" });
            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,
            };
            return View(dto);
        }
        [HttpGet]

        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation");
                var roleResult = await _roleManager.FindByNameAsync(role.Name);
                if (roleResult is not null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Operation");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation");
                var roleResult = await _roleManager.FindByNameAsync(role.Name);
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Invalid Operation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddorRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewData["RoleId"] = role.Id;
            ViewData["RoleName"] = role.Name;
            var usersInRole = new List<UserInRoleDto>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var UserInRole = new UserInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    UserInRole.IsSelected = true;
                }
                else
                {
                    UserInRole.IsSelected = false;

                }
                usersInRole.Add(UserInRole);
            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddorRemoveUsers(string roleId, List<UserInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
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
                return RedirectToAction(nameof(Edit), new { id = roleId });
            }
            return View(users);
        }
    }
}
