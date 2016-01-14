using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Td.Kylin.WebApi.Website.WebApi
{
    [Route("api/jsontest")]
    public class JsonTestController : BaseController
    {
        [HttpGet("showlong")]
        public IActionResult Int64()
        {
            TestModel model = new TestModel
            {
                Name = "测试",
                ItemID = long.MaxValue
            };

            return KylinOk(model);
        }

        [HttpGet("shownullablelong")]
        public IActionResult NullableInt64()
        {
            TestNullableModel model = new TestNullableModel
            {
                Name = "测试",
                ItemID = long.MaxValue
            };

            return KylinOk(model);
        }

        [HttpGet("html")]
        public IActionResult HtmlString()
        {
            TestNullableModel model = new TestNullableModel
            {
                Name = "测试",
                ItemID = long.MaxValue,
                Content = @"<h1 style=""font - size:16px; font - family:arial, 'microsoft yahei'; color:#666666;background-color:#FFFFFF;"">
	鸿星尔克男鞋冬季运动鞋男跑步鞋男款跑鞋男慢跑鞋子 正黑 41
</ h1 >
< div id = ""p-ad"" class=""p-ad J-ad-1629975686"" style=""margin:0px;padding:0px;font-family:arial, 'microsoft yahei';color:#E3393C;font-size:14px;background-color:#FFFFFF;"">
	拍下只要175元2015冬季新品，支持货到付款。
</div>"
            };

            return KylinOk(model);
        }

        public class TestModel
        {
            public string Name { get; set; }

            public long ItemID { get; set; }
        }

        public class TestNullableModel
        {
            public string Name { get; set; }

            public long? ItemID { get; set; }

            public string Content { get; set; }
        }
    }
}
