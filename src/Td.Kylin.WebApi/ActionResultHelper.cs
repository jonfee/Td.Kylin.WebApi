using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
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
        public IActionResult KylinOk<T>(T result)
        {
            return KylinOk(ResultCode.Success, null, result);
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

            string data = JsonConvert.SerializeObject(apiResult, Formatting.Indented, Settings.SerializerSettings);

            var result = new HttpOkObjectResult("api");
            result.Value = data;
            result.StatusCode = 200;
            result.ContentTypes.Add(new MediaTypeHeaderValue("application/json"));

            return result;
        }
    }
}
