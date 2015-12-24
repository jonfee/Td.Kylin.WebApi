using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi
{
    /// <summary>
    /// Kylin接口控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Kylin使用的HttpOkObjectResult结果返回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IActionResult KylinOk(object obj)
        {
            if(null== obj)
            {
                return Ok("null");
            }

            var type = obj.GetType();

            if (type == typeof(string))
            {
                return Ok(obj);
            }
            else
            {
                var data = JsonConvert.SerializeObject(obj, Formatting.Indented, Settings.SerializerSettings);

                return Ok(data);
            }
        }
    }
}
