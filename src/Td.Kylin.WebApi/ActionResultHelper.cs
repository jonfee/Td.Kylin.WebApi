using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi
{
    public class ActionResultHelper
    {
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
