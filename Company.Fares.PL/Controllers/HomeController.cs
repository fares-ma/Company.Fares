using System.Diagnostics;
using System.Text;
using Company.Fares.DAL.Models;
using Company.Fares.PL.Models;
using Company.Fares.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Company.Fares.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService _scopedService1;
        private readonly IScopedService _scopedService2;
        private readonly ITransentService _transientService1;
        private readonly ITransentService _transientService2;
        private readonly ISingletonService _singeltonService1;
        private readonly ISingletonService _singeltonService2;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger
            , IScopedService scopedService1
            , IScopedService scopedService2
            , ITransentService transientService1
            , ITransentService transientService2
            , ISingletonService singeltonService1
            , ISingletonService singeltonService2
            , UserManager<AppUser> userManager)
        {
            _logger = logger;
            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;
            _transientService1 = transientService1;
            _transientService2 = transientService2;
            _singeltonService1 = singeltonService1;
            _singeltonService2 = singeltonService2;
            _userManager = userManager;
        }

        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"scopedService1 :: {_scopedService1.GetGuid()}\n");
            builder.Append($"scopedService2 :: {_scopedService2.GetGuid()}\n\n");
            builder.Append($"transientService1 :: {_transientService1.GetGuid()}\n");
            builder.Append($"transientService2 :: {_transientService2.GetGuid()}\n\n");
            builder.Append($"singeltonService1 :: {_scopedService1.GetGuid()}\n");
            builder.Append($"singeltonService :: {_scopedService2.GetGuid()}\n\n");
            return builder.ToString();
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // Get the logged-in user
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
