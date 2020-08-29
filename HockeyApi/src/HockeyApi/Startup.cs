using System.Text.Json.Serialization;
using HockeyApi.Common;
using HockeyApi.Features;
using HockeyApi.Features.Player;
using HockeyApi.Features.Team;
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
			services
				.AddControllers()
				.AddJsonOptions(jsonOptions =>
				{
					jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

					//add this just as a visual for myself...would not likely want to format a DateTime like this for an API unless there was a reason the API was responsible for dictating a consistent format
					jsonOptions.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
				});

			string connStr = _configuration.GetConnectionString("Default");
			services.AddScoped<IDb>(_ => new Db(_configuration.GetConnectionString("Default")));
			services.AddScoped<ITeamService, TeamService>();
			services.AddScoped<IPlayerService, PlayerService>();
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