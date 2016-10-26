
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Td.Common;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi
{
    public class ActionResultHelper
    {
        /// <summary>
        /// 表示执行结果为<seealso cref="ResultCode.Success"/>时的HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IActionResult Success<T>(T result)
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
        public static IActionResult Error(ResultCode resultCode, string errorContent = null)
        {
            string err = EnumUtility.GetEnumDescription<ResultCode>(resultCode.ToString());

            return KylinOk((int)resultCode, err, errorContent ?? err);
        }

        /// <summary>
        /// HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">结果状态码<seealso cref="ResultCode"/></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IActionResult KylinOk<T>(ResultCode code, string message, T content)
        {
            return KylinOk((int)code, message, content);
        }

        /// <summary>
        /// HttpOkObjectResult结果返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IActionResult KylinOk<T>(int code, string message, T content)
        {
            ApiResult<T> apiResult = new ApiResult<T>
            {
                Code = code,
                Message = message,
                Content = content
            };

            string data = JsonConvert.SerializeObject(apiResult, Formatting.None, Settings.SerializerSettings);

            var result = new OkObjectResult("api");
            result.Value = data;
            result.StatusCode = 200;
            result.ContentTypes.Add(new MediaTypeHeaderValue("application/json"));

            return result;
        }
    }
}
