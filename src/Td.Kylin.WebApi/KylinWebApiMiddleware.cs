using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Website
{
    public class KylinWebApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KylinWebApiOptions _options;
        /// <summary>
        /// Creates a default web page for new applications.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        public KylinWebApiMiddleware(RequestDelegate next, KylinWebApiOptions options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options;
            _next = next;
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            WebApiConfig.Configuration = _options.Configuration;

            return _next(context);
        }
    }
}
