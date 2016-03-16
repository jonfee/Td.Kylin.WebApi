using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
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
            if (null == obj)
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

        /// <summary>
        /// Kylin使用的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public IActionResult KylinOk<T>(int code, string message, T result)
        {
            ApiResult<T> apiResult = new ApiResult<T>
            {
                Code = code,
                Message = message,
                Content = result
            };

            string data = JsonConvert.SerializeObject(apiResult, Formatting.Indented, Settings.SerializerSettings);

            return Ok(data);
        }
    }
}
