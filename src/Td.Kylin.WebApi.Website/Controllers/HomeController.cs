using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Td.AspNet.WebApi;
using Td.Kylin.WebApi.Website.Models;
using Td.AspNet.Utils;
using Td.Diagnostics;
using Td.Web.Filters;

namespace Td.Kylin.WebApi.Website.Controllers
{
    public class HomeController : BaseController
    {
		private readonly IHostingEnvironment _hostingEnvironment;

		public HomeController(IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

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

        public IActionResult StringResult()
        {
            return KylinOk("这是字符串");
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

        public IActionResult TestArgument(string args)
        {
            return KylinOk(args);
        }

	    public IActionResult TestLogger()
	    {
			Logger.Info("info...");
			Logger.Debug("debug...");
			return KylinOk("日志写入成功...");
		}

	    public IActionResult TestError()
	    {
		    throw new InvalidOperationException("exception test...");
	    }
    }
}
