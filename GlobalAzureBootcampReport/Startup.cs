using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
			services.AddMvc();
			services.AddSignalR();

			services.AddWebpack();

			services.AddSingleton<IConfiguration>(sp => { return Configuration; });

			// Add application services.
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
				app.UseWebpack("webpack.config.js", "bundle.js", new WebpackDevServerOptions {Host= "localhost", Port = 4000 });
			}

			app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

			app.UseStaticFiles();

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
			//app.ApplicationServices.GetRequiredService<ITwitterManager>().Connect();
		}

		// Entry point for the application.
		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
