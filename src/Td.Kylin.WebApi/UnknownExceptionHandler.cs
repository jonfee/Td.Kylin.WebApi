using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Td.Diagnostics;

using Newtonsoft.Json;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Td.AspNet.Utils;
using Td.Kylin.WebApi.Models;

namespace Td.Kylin.WebApi
{
    public class UnknownExceptionHandler : ExceptionHandlerBase
    {
        #region 私有字段

        private string _master;
        private string _view;

        #endregion

        #region 公共属性

        public string Master
        {
            get
            {
                return (this._master ?? string.Empty);
            }
            set
            {
                this._master = value;
            }
        }

        public string View
        {
            get
            {
                if (string.IsNullOrEmpty(this._view))
                {
                    return "Error";
                }

                return this._view;
            }
            set
            {
                this._view = value;
            }
        }

        public override bool CanHandle(Type exceptionType)
        {
            return true;
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化 UnknownExceptionHandler 类的新实例。
        /// </summary>
        public UnknownExceptionHandler() : this(null)
        {

        }

        /// <summary>
        /// 初始化 UnknownExceptionHandler 类的新实例。
        /// </summary>
        /// <param name="view">异常视图。</param>
        public UnknownExceptionHandler(string view) : this(view, null)
        {

        }

        /// <summary>
        /// 初始化 UnknownExceptionHandler 类的新实例。
        /// </summary>
        /// <param name="view">异常视图。</param>
        /// <param name="master">模板视图。</param>
        public UnknownExceptionHandler(string view, string master) : base(new Type[] { typeof(Exception) })
        {
            this.View = view;
            this.Master = master;
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 处理指定的异常。
        /// </summary>
        /// <param name="exception">要处理的异常对象。</param>
        /// <param name="context">请求上下文信息。</param>
        /// <returns>如果当前处理器已经对参数<paramref name="exception"/>指定的异常对象，处理完毕则返回为空，如果当前异常处理器还需要后续的其他处理器对返回的新异常对象继续处理的话，则返回一个新异常。</returns>
        public override Exception Handle(Exception exception, object context)
        {
            //写入异常日志。
            this.WriteLog(exception, context);

            //获取上下文。
            var actionContext = context as ActionExecutedContext;

            if (actionContext != null)
            {
                //创建返回视图。
                actionContext.Result = this.CreateResult(exception, actionContext);
                actionContext.HttpContext.Response.Clear();
                actionContext.HttpContext.Response.StatusCode = 200;

                return null;
            }
            else
            {
                return exception;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建一个异常响应。
        /// </summary>
        /// <param name="exception">异常实例。</param>
        /// <param name="context">异常上下文。</param>
        /// <returns>响应。</returns>
        private ActionResult CreateResult(Exception exception, ActionExecutedContext context)
        {
            var result = new ApiResult<string>
            {
                Code = (int)ResultCode.Error,
                Message = "Sorry, the server has encounter an error.",
                Content = exception.Message
            };

            return new JsonResult(result, Settings.SerializerSettings);
        }

        /// <summary>
        /// 将异常信息写入日志。
        /// </summary>
        /// <param name="exception">异常实例。</param>
        private void WriteLog(Exception exception, object context)
        {

            //获取上下文。
            var actionContext = context as ActionExecutedContext;
            var controllerName = actionContext.RouteData.Values["controller"].ToString();
            var actionName = actionContext.RouteData.Values["action"].ToString();
            #region 像服务器发送错误日志
            /*
            var list = new List<LogsModel>();

            list.Add(new LogsModel
            {
                Click = string.Format("{0}_{1}", controllerName, actionName),
                Content = exception.StackTrace,
                CreateTime = DateTime.Now,
                ProgramVersion = WebApiConfig.Options.Log.ProgramVersion,
                RunEnvironmental = WebApiConfig.Options.Log.RunEnvironmental,
                Source = 1,
                SystemVersion = WebApiConfig.Options.Log.SystemVersion,
                Title = exception.Message,
                Type = 3
            });

            Dictionary<string, string> txtParams = new Dictionary<string, string>();
            txtParams.Add("list", Newtonsoft.Json.JsonConvert.SerializeObject(list));
            var urlTemp = Strings.BuildGetUrl(WebApiConfig.Options.Log.LogUrl, txtParams);

            HttpContent result = new FormUrlEncodedContent(txtParams);


            using (var client = new HttpClient())
            {
                var response = client.PostAsync(urlTemp, result);
                var responseResult = response.Result;
                var responseResultReadAsync = responseResult.Content.ReadAsStringAsync();
                var jsonResult = responseResultReadAsync.Result;
            }

    */
            #endregion

            //写入日志。
            Logger.Error(exception);
        }

        #endregion
    }
}
