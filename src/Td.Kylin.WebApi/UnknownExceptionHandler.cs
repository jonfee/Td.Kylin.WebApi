using System;
using System.Collections.Generic;

using Td.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Newtonsoft.Json;
using Td.Kylin.EnumLibrary;
using Td.Kylin.WebApi.Json;

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
				if(string.IsNullOrEmpty(this._view))
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

#if DNX451
		/// <summary>
		/// 初始化 UnknownExceptionHandler 类的新实例。
		/// </summary>
		/// <param name="view">异常视图。</param>
		/// <param name="master">模板视图。</param>
		public UnknownExceptionHandler(string view, string master) : base(new Type[] { typeof(ApplicationException), typeof(SystemException), typeof(Exception) })
		{
			this.View = view;
			this.Master = master;
		}
#endif

#if DNXCORE50
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
#endif

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
			this.WriteLog(exception);

			//获取上下文。
			var actionContext = context as ActionExecutedContext;

			if(actionContext != null)
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
		private void WriteLog(Exception exception)
		{
			//写入日志。
			Logger.Error(exception);
		}

		#endregion
    }
}
