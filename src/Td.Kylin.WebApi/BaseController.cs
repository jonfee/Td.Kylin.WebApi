using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi
{
    public class RequestParameter
    {
        public int LBSArea
        {
            get;
            internal set;

        }

        public double LBSLongitude
        {
            get;
            internal set;

        }

        public double LBSLatitude
        {
            get;
            internal set;
        }
    }

    /// <summary>
    /// Kylin接口控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        private static RequestParameter _parameter = new RequestParameter();

        protected RequestParameter Parameter
        {
            get
            {
                _parameter.LBSArea = int.Parse(HttpContext.Items["LBSArea"].ToString());
                _parameter.LBSLongitude = double.Parse(HttpContext.Items["LBSLongitude"].ToString());
                _parameter.LBSLatitude = double.Parse(HttpContext.Items["LBSLatitude"].ToString());

                return _parameter;
            }
        }

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
