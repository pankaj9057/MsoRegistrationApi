using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System.Net;
using System;
namespace MsoRegistrationApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					//  webBuilder.ConfigureKestrel(serverOptions =>
                    // {
                    //     serverOptions.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("PORT")));
                    // }).UseStartup<Startup>();
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureLogging((ctx, lb) =>
				{
					lb.AddNLog(new NLog.Extensions.Logging.NLogLoggingConfiguration(ctx.Configuration.GetSection("NLog")));
				});
	}
}