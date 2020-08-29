using HockeyApi.Common;
using HockeyApi.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HockeyApi
{
	public class Startup
	{
		readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_configuration = configuration;
			_environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			string connStr = _configuration.GetConnectionString("Default");
			services.AddScoped<IDb>(_ => new Db(_configuration.GetConnectionString("Default")));
			services.AddScoped<ITeamService, TeamService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			if (_environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			})
			.Run(_notFoundHander);
		}

		private readonly RequestDelegate _notFoundHander = async ctx =>
		{
			ctx.Response.StatusCode = 404;
			await ctx.Response.WriteAsync("Page not found.");
		};
	}
}