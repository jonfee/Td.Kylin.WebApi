using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Website
{
    public static class KylinWebApiExtensions
    {
        public static IApplicationBuilder UseKylinWebApi(this IApplicationBuilder builder, KylinWebApiOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.Use(next => new KylinWebApiMiddleware(next, options).Invoke);
        }

        public static IApplicationBuilder UseKylinWebApi(this IApplicationBuilder builder, IConfigurationRoot config)
        {
            return UseKylinWebApi(builder, new KylinWebApiOptions { Configuration = config });
        }
    }
}
