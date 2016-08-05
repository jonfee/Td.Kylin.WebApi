
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Td.Common;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Json;
using Microsoft.AspNetCore.Mvc;
using Td.Serialization;
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

                int lbsArea = 0;
                double lbsLongitude = 0d;
                double lbsLatitude = 0d;

                string area = HttpContext.Items[RequestParameterNames.LBSArea].ToString();
                string longitude = HttpContext.Items[RequestParameterNames.LBSLongitude].ToString();
                string latitude = HttpContext.Items[RequestParameterNames.LBSLatitude].ToString();

                int.TryParse(area, out lbsArea);
                double.TryParse(longitude, out lbsLongitude);
                double.TryParse(latitude, out lbsLatitude);

                _location = new Location
                {
                    OperatorArea = lbsArea,
                    Longitude = lbsLongitude,
                    Latitude = lbsLatitude
                };

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
        /// 表示执行结果为<seealso cref="ResultCode.Success"/>时的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public IActionResult Success<T>(T result)
        {
            return KylinOk(ResultCode.Success, null, result);
        }

        /// <summary>
        /// 表示执行结果错误时的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">结果状态码<seealso cref="ResultCode"/></param>
        /// <param name="errorContent"></param>
        /// <returns></returns>
        public IActionResult Error(ResultCode resultCode, string errorContent = null)
        {
            string err = EnumUtility.GetEnumDescription<ResultCode>(resultCode.ToString());

            return KylinOk((int)resultCode, err, errorContent ?? err);
        }

        /// <summary>
        /// Kylin使用的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">结果状态码<seealso cref="ResultCode"/></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public IActionResult KylinOk<T>(ResultCode resultCode, string message, T result)
        {
            return KylinOk((int)resultCode, message, result);
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
            
            string data = Serializer.Json.Serialize(result, Settings.TextSerializationSetting);

            //string data1 = JsonConvert.SerializeObject(result, Formatting.Indented, Settings.SerializerSettings);

            return Ok(data);
        }
    }
}
