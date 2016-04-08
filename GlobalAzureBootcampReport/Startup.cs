﻿using System.Collections.Generic;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GlobalAzureBootcampReport.Models;
using GlobalAzureBootcampReport.Services;
using Tweetinvi;
using GlobalAzureBootcampReport.Data;
using GlobalAzureBootcampReport.Data.Impl;
using GlobalAzureBootcampReport.Azure;
using Webpack;

namespace GlobalAzureBootcampReport {
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile("connectionSecrets.json", optional: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddEntityFramework()
				.AddSqlServer()
				.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddMvc();
			services.AddSignalR();

			services.AddWebpack();

			services.AddSingleton<IConfiguration>(sp => { return Configuration; });

			// Add application services.
			services.AddTransient<IEmailSender, AuthMessageSender>();
			services.AddTransient<ISmsSender, AuthMessageSender>();
			services.AddSingleton<AzureHelper>();
			services.AddSingleton<ITwitterManager, TwitterManager>();
			services.AddTransient<ICache, Cache>();
			services.AddTransient<ITweetsRepository, TweetsRepository>();
			services.AddTransient<IDocumentDbManager, DocumentDbManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
				//app.UseWebpack(new WebpackOptions() {
				//	HandleJsxFiles = true,
				//	EnableHotLoading = true,
				//	StylesTypes = new List<StylesType> { StylesType.Css},
				//	HandleStaticFiles = true,
				//	StaticFileTypes = new List<StaticFileType> { StaticFileType.Png}
				//});
			}
			//else
			//{
			//    app.UseExceptionHandler("/Home/Error");
			//}
			// For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
			try {
				using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
					.CreateScope()) {
					serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
						 .Database.Migrate();
				}
			}
			catch { }

			app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

			app.UseStaticFiles();

			app.UseIdentity();

			// To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseSignalR();


			Auth.SetUserCredentials(
				Configuration["TwitterConsumerKey"],
				Configuration["TwitterConsumerSecret"],
				Configuration["TwitterUserAccessToken"],
				Configuration["TwitterUserAccessSecret"]
			);
			app.ApplicationServices.GetRequiredService<ITwitterManager>().Connect();
		}

		// Entry point for the application.
		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
