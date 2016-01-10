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

        public class TestModel
        {
            public string Name { get; set; }

            public long ItemID { get; set; }
        }

        public class TestNullableModel
        {
            public string Name { get; set; }

            public long? ItemID { get; set; }
        }
    }
}
