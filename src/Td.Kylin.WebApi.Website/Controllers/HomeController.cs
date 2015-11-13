using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Td.AspNet.WebApi;

namespace Td.Kylin.WebApi.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var dic = new Dictionary<string, string>();
            var txt = DefaultClient.DoGet("http://localhost:1844/api/values", dic, "T0001", "AB0BD2410612B2850779F3081206AE95").Result;
            ViewBag.Txt = txt;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
