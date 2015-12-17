using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Td.AspNet.WebApi;
using Td.Kylin.WebApi.Website.Models;
using Td.AspNet.Utils;

namespace Td.Kylin.WebApi.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", "天道新创");
            //Users user = new Users();
            //user.UserId = 1;
            //user.Name = "Test";
            //user.Password = Strings.PasswordEncrypt("admin");
            //dic = DicMapper.ToMap(user);
            //var txt = DefaultClient.DoPost("http://localhost:36177/api/values", dic, "A0001", "8C44B1F5-CC25-46C9-AA1E-BB2BB0123E66").Result;
            var txt = DefaultClient.DoGet("http://localhost:36177/api/values", dic, "A0001", "8C44B1F5-CC25-46C9-AA1E-BB2BB0123E66").Result;
            //var txt = DefaultClient.DoGet("http://localhost:2025/v1/admin/1", dic, "A0001", "8C44B1F5-CC25-46C9-AA1E-BB2BB0123E66").Result;
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
