using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.OpenApi.Models;
using DatabaseSettings;
using ReactWindowsAuth.Services; 
using ReactWindowsAuth.Models;
using Microsoft.Extensions.Options;

namespace ReactWindowsAuth
{
	public class Constants
	{
		public const string CORS_ORIGINS = "CorsOrigins";
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
			// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-3.1&tabs=visual-studio
			services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
			services.AddAuthorization(options =>
			{
				// By default, all incoming requests will be authorized according to the default policy.
				options.FallbackPolicy = options.DefaultPolicy;
			});
			//services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
			 services.AddSwaggerGen(c => {
									c.SwaggerDoc("v1", new OpenApiInfo {
										Title = "SwaggerDemoApplication", Version = "v1"
									});
								});
			services.AddControllers();
			services.AddCors(opt =>
			{
				opt.AddPolicy("CorsPolicy", builder => builder
					.AllowAnyHeader()
					.AllowAnyMethod()
					.WithOrigins(Configuration.GetSection(Constants.CORS_ORIGINS).Get<string[]>())
					.AllowCredentials());
			});

			services.AddTransient<Microsoft.AspNetCore.Authentication.IClaimsTransformation, ClaimsTransformer>();
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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors("CorsPolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSwagger();
       		app.UseSwaggerUI(c => {
							c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerDemoApplication V1");
							});
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
