using System.Diagnostics;
using System.Text;
using Company.Fares.PL.Models;
using Company.Fares.PL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Company.Fares.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scopedServic01;
        private readonly IScopedService scopedService02;
        private readonly ITransentService transentService01;
        private readonly ITransentService transentService02;
        private readonly ISingletonService singletonService01;
        private readonly ISingletonService singletonService02;

        public HomeController(
            ILogger<HomeController> logger,
            IScopedService scopedServic01,
            IScopedService scopedService02,
            ITransentService transentService01,
            ITransentService transentService02,
            ISingletonService singletonService01,
            ISingletonService singletonService02

            )
        {
            _logger = logger;
            this.scopedServic01 = scopedServic01;
            this.scopedService02 = scopedService02;
            this.transentService01 = transentService01;
            this.transentService02 = transentService02;
            this.singletonService01 = singletonService01;
            this.singletonService02 = singletonService02;
        }


        public string  TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"scopedServic01 :: {scopedServic01.GetGuid()}\n");
            builder.AppendLine($"scopedServic02 :: {scopedService02.GetGuid()}\n\n");
            builder.AppendLine($"transentService01 :: {transentService01.GetGuid()}\n");
            builder.AppendLine($"transentService02 :: {transentService02.GetGuid()}\n\n");
            builder.AppendLine($"singletonService01 :: {singletonService01.GetGuid()}\n");
            builder.AppendLine($"singletonService02 :: {singletonService02.GetGuid()}\n\n");

            return builder.ToString();
         
        }

        public IActionResult Index()
        {
            return View();
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
