using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.OpenApi.Models;
using DatabaseSettings;
using MsoRegistrationApi.Services; 
using MsoRegistrationApi.Models;
using Microsoft.Extensions.Options;
using MsoRegistrationApi.Middleware;

namespace MsoRegistrationApi
{
	public class Constants
	{
		public const string CORS_ORIGINS = "CorsOrigins";
		public const string CORS_HEADERS="AllowedHeaders";
		public const string DatabaseSetting_Section ="MsoRegistrationDatabase";
	}

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
		    //services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
			 services.AddSwaggerGen(c => {
									c.SwaggerDoc("v1", new OpenApiInfo {
										Title = "MsoRegistration Api", Version = "v1"
									});
								});
			services.AddControllers();
			services.AddCors(opt =>
			{
				opt.AddPolicy(Constants.CORS_ORIGINS, builder => builder
					.WithHeaders(Configuration.GetSection(Constants.CORS_HEADERS).Get<string[]>())
					.AllowAnyMethod()
					.WithOrigins(Configuration.GetSection(Constants.CORS_ORIGINS).Get<string[]>())
					.AllowCredentials());
			});
			services.Configure<DatabaseSetting>(Configuration.GetSection(Constants.DatabaseSetting_Section));
			services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                } );
			services.AddSingleton<RegistrationService<RegistrationDetails>>(x=> new RegistrationService<RegistrationDetails>
			(
				services.BuildServiceProvider().GetService<IOptions<DatabaseSetting>>(),"RegistrationDetails"
			));
			services.AddSingleton<RegistrationService<FcaDetails>>(x=> new RegistrationService<FcaDetails>
			(
				services.BuildServiceProvider().GetService<IOptions<DatabaseSetting>>(),"FcaDetails"
			));
			services.AddSingleton<RegistrationService<Role>>(x=> new RegistrationService<Role>
			(
				services.BuildServiceProvider().GetService<IOptions<DatabaseSetting>>(),"Role"
			));
			services.AddSingleton<RegistrationService<Title>>(x=> new RegistrationService<Title>
			(
				services.BuildServiceProvider().GetService<IOptions<DatabaseSetting>>(),"Title"
			));
			services.AddSingleton<RegistrationService<Brand>>(x=> new RegistrationService<Brand>
			(
				services.BuildServiceProvider().GetService<IOptions<DatabaseSetting>>(),"Brand"
			));
			services.AddHealthChecks()
			.AddMongoDb(Configuration.GetSection(Constants.DatabaseSetting_Section)["ConnectionString"]);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			// app.UseCors((configurePolicy)=> configurePolicy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000","https://pankaj9057.github.io")
			// .WithHeaders("Content-Type","Accept","ApiKey"));
			if (env.IsProduction())
			{
				app.UseMiddleware<ApiKeyMiddleware>();
			}
			app.UseRouting();		
			app.UseSwagger();
       		app.UseSwaggerUI(c => {
							c.SwaggerEndpoint("/swagger/v1/swagger.json", "MsoRegistration Api");
							});
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/healthcheck");
			});
		}
	}
}
