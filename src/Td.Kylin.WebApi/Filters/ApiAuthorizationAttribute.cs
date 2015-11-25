using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections.Generic;
using Td.AspNet.Utils;
using Td.AspNet.WebApi;
using Td.Kylin.WebApi.Cache;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi.Filters
{
    public class ApiAuthorizationAttribute : ActionFilterAttribute
    {
        public Role Code { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;
            IDictionary<string, string> queryDic = Strings.SplitUrlQuery(request.QueryString.Value);
            if (queryDic == null)
            {
                context.Result = Message(1, "参数缺失", "URL参数缺失");
                return;

            }
            string Sign = string.Empty;
            DateTime Timestamp = DateTime.Parse("1970-01-01 00:00:00");
            string PartnerId = string.Empty;

            foreach (var item in queryDic)
            {
                if (item.Key == "Sign")
                {
                    Sign = item.Value;
                }
                if (item.Key == "PartnerId")
                {
                    PartnerId = item.Value;
                }
                if (item.Key == "Timestamp")
                {
                    Timestamp = DateTime.Parse(item.Value);
                }
            }

            if (string.IsNullOrEmpty(PartnerId) || string.IsNullOrEmpty(Sign))
            {
                context.Result = Message(2, "参数不正确", "参数 PartnerId 或 Sign丢失，或者值不正常");
                return;
            }

            if (Timestamp.AddMinutes(5) < DateTime.Now)
            {
                context.Result = Message(3, "服务过期", "API请求时间超时，服务过期，请检查Timestamp或同步服务器时间");
                return;
            }

            var moduleInfo = new System_ModuleAuthorize();
            try
            {
                moduleInfo = ModuleAuthorizeCache.GetSecret(PartnerId);
            }
            catch (Exception ex)
            {
                context.Result = Message(4, "获取模块授权异常", ex.Message);
                return;
            }
            if(moduleInfo == null)
            {
                context.Result = Message(4, "获取模块授权异常", "模块授权信息不存在");
                return;
            }
            var secret = moduleInfo.AppSecret;
            if (string.IsNullOrEmpty(moduleInfo.AppSecret))
            {
                context.Result = Message(4, "授权未通过", "非法访问，授权未通过");
                return;
            }
            if (Code != 0)
            {
                if ((Role)moduleInfo.Role != Role.Admin)
                {

                    if (((Role)moduleInfo.Role & Code) != (Role)moduleInfo.Role)
                    {
                        context.Result = Message(5, "授权未通过", "模块权限不够，不允许进行操作");
                        return;
                    }
                }
            }

            if (method == "POST")
            {
                queryDic.Remove("Sign");
                try {
                    var data = request.Form;
                    var dic = new Dictionary<string, string>();
                    foreach (var item in data)
                    {
                        queryDic.Add(item.Key, item.Value[0]);
                    }
                }
                catch(Exception ex)
                {
                    context.Result = Message(12, "数据异常", "request.Form 获取表单数据异常");
                    return;
                }
                var s = Strings.SignRequest(queryDic, secret);
                if (Sign != s)
                {
                    context.Result = Message(6, "签名异常", "未通过签名验证，请检查签名的参数和顺序是否正确");
                    return;
                }

            }
            else if (method == "GET")
            {
                queryDic.Remove("Sign");
                var s = Strings.SignRequest(queryDic, secret);
                if (Sign != s)
                {
                    context.Result = Message(6, "签名异常", "未通过签名验证，请检查签名的参数和顺序是否正确");
                    return;
                }
            }
            else
            {
                context.Result = Message(10, "非法请求", "请求的模式不正确");
                return;
            }

        }

        public IActionResult Message(int code, string message, string content)
        {
            var msg = new ErrorMessage();
            msg.Code = code;
            msg.Content = content;
            msg.Message = message;
            return ActionMessage.Ok(msg);
        }
    }
}
