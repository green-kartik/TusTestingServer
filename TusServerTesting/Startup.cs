using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tusdotnet;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Stores;

namespace TusServerTesting
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseSimpleExceptionHandler();
			app.UseTus(httpContext => new DefaultTusConfiguration
			{
				// c:\tusfiles is where to store files
				Store = new TusDiskStore(@"C:\tusfiles\"),
				// On what url should we listen for uploads?
				UrlPath = "/files",

				Events = new Events
				{
					OnFileCompleteAsync = async eventContext =>
					{
						ITusFile file = await eventContext.GetFileAsync();
						await DoSomeProcessing(file);
					}
				},


			});
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
		public static Task DoSomeProcessing(ITusFile file)
		{
			return Task.CompletedTask;
		}
	}
}
