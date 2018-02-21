using Localization.Routing.Mvc.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Reflection;

namespace Localization.Routing.Mvc.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly IStringLocalizerFactory _localizerFactory;

        public HomeController(
            IStringLocalizer<Resource> localizer,
            IStringLocalizerFactory localizerFactory)
        {
            _localizer = localizer;
            _localizerFactory = localizerFactory;
        }

        public IActionResult Index()
        {
            ViewData["Welcome"] = _localizer["Welcome"];
            return View();
        }

        public IActionResult About()
        {
            var type = typeof(Resource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            var localizer = _localizerFactory.Create("Resource", assemblyName.Name);
            ViewData["Title"] = _localizer["About"];
            ViewData["Message"] = localizer["About"];

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
