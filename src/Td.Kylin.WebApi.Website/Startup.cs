﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

using Td.Web;
using Td.Diagnostics;

namespace Td.Kylin.WebApi.Website
{
	public class Startup
	{
		public IConfigurationRoot Configuration
		{
			get;
			set;
		}

		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			// 启动应用程序。
			Application.Start(new ApplicationContext(env, appEnv), null);

			var builder = new ConfigurationBuilder().SetBasePath(appEnv.ApplicationBasePath).AddJsonFile("appsettings.json").AddEnvironmentVariables();

			this.Configuration = builder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// 初始化异常处理程序。
			this.InitExceptionHandler(services);

			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if(env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseKylinWebApi(Configuration);
			app.UseIISPlatformHandler();
			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(name : "default", template : "{controller=Home}/{action=Index}/{id?}");
			});
		}

		private void InitExceptionHandler(IServiceCollection services)
		{
			// 添加异常拦截处理程序。
			ExceptionHandlerManager.Instance.Handlers.Add(new UnknownExceptionHandler());

			// 添加全局异常过滤器。
			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new Td.Web.Filters.HandleExceptionFilter());
			});
		}
	}
}