﻿using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Filters;
using Td.Diagnostics;
using Td.Caching;
using Td.AspNet.Utils;
using Newtonsoft.Json;
using Td.Kylin.WebApi.Json;

namespace Td.Kylin.WebApi.Filters
{
    public class HandleArgumentFilter : ActionFilterAttribute
    {
        private static readonly ICache _cache = new MemoryCache("ARGUMENT_LOGGERS");
    
        #region 重写方法

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.WriteLog(context);

            base.OnActionExecuting(context);
        }

        #endregion

        #region 私有方法

        private void WriteLog(ActionExecutingContext context)
        {
            if(context.RouteData.Values.Keys.Contains("controller") && context.RouteData.Values.Keys.Contains("action"))
            {
                var arguments = Strings.SplitUrlQuery(context.HttpContext.Request.QueryString.Value);
                var controllerName = context.RouteData.Values["controller"].ToString();
                var actionName = context.RouteData.Values["action"].ToString();

                var content = new
                {
                    Controller = controllerName,
                    Action = actionName,
                    Arguments = arguments
                };

                var data = JsonConvert.SerializeObject(content, Formatting.Indented, Settings.SerializerSettings);

                var handler = GetLoggerHandler(controllerName, actionName);

                // 写入日志。
                handler.Handle(new LogEntry(LogLevel.Info, null, "", data));
            }
        }

        private static LoggerHandler GetLoggerHandler(string controllerName, string actionName)
        {
            if (string.IsNullOrWhiteSpace(controllerName))
                throw new ArgumentNullException(nameof(controllerName));

            if (string.IsNullOrWhiteSpace(actionName))
                throw new ArgumentNullException(nameof(actionName));

            var key = GetCacheKey(controllerName, actionName);
            var handler = _cache.GetValue(key) as LoggerHandler;

            if (handler == null)
            {
                handler = new LoggerHandler(key, new TextFileLogger()
                {
                    FilePath = "logs/args/" + controllerName + "_" + actionName + "/${binding:timestamp#yyyyMM}/${binding:source}[{sequence}].log"
                }, new LoggerHandlerPredication()
                {
                    MinLevel = LogLevel.Debug,
                    MaxLevel= LogLevel.Info
                });

                // 将日志记录处理程序加入缓存。
                _cache.SetValue(key, handler);
            }

            return handler;
        }

        private static string GetCacheKey(string controllerName, string actionName)
        {
            return string.Format("{0}_{1}", controllerName.ToUpper(), actionName.ToUpper());
        }

        #endregion
    }
}