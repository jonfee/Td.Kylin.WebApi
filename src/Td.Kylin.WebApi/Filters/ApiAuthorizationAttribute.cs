using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;

namespace Td.Kylin.WebApi.Filters
{
    public class ApiAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;
            var query = request.QueryString.Value.Trim(new char[] { '?', ' ' });


        }
    }
}
