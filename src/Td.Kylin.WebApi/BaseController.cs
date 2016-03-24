using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi
{
    /// <summary>
    /// Kylin接口控制器基类
    /// </summary>  
    public class BaseController : Controller
    {
        private Location _location;

        /// <summary>
        /// 当前位置
        /// </summary>
        protected Location Location
        {
            get
            {
                if (null == _location)
                {
                    int lbsArea = 0;
                    double lbsLongitude = 0d;
                    double lbsLatitude = 0d;

                    string area = HttpContext.Items[RequestParameterNames.LBSArea] as string;
                    string longitude = HttpContext.Items[RequestParameterNames.LBSLongitude] as string;
                    string latitude = HttpContext.Items[RequestParameterNames.LBSLatitude] as string;

                    int.TryParse(area, out lbsArea);
                    double.TryParse(longitude, out lbsLongitude);
                    double.TryParse(latitude, out lbsLatitude);

                    _location = new Location
                    {
                        OperatorArea = lbsArea,
                        Longitude = lbsLongitude,
                        Latitude = lbsLatitude
                    };
                }

                return _location;
            }
        }

        /// <summary>
        /// Kylin使用的HttpOkObjectResult结果返回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete("即将删除，请使用KylinOk<T>(int,string,T)")]
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

            return KylinOk(apiResult);
        }

        /// <summary>
        /// Kylin使用的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public IActionResult KylinOk<T>(ApiResult<T> result)
        {
            if (null == result)
            {
                result = new ApiResult<T>
                {
                    Code = 0,
                    Message = null,
                    Content = default(T)
                };
            }

            string data = JsonConvert.SerializeObject(result, Formatting.Indented, Settings.SerializerSettings);

            return Ok(data);
        }
    }
}
