using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Td.AspNet.Utils.Json;

namespace Td.Kylin.WebApi.Json
{
    /// <summary>
    /// Json序列化全局配置
    /// </summary>
    public class Settings
    {
        public readonly static JsonSerializerSettings SerializerSettings;

        static Settings()
        {
            SerializerSettings = new JsonSerializerSettings();

            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                //空值处理
                SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                SerializerSettings.Converters.Add(new Int64Convert());

                return SerializerSettings;
            });
        }
    }
}
