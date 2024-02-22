using Microsoft.AspNetCore;

[assembly: CLSCompliant(false)]


namespace Innovt.AspNetCore.Application.Tests;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(WebHost.CreateDefaultBuilder(args)).Build().RunAsync();
    }

    public static IWebHostBuilder CreateHostBuilder(IWebHostBuilder hostBuilder)
    {
        if (hostBuilder == null) throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder
            .CaptureStartupErrors(true).UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, builder) =>
            {
                builder.AddJsonFile("appsettings.json");
                builder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json");

                builder.AddEnvironmentVariables();
            }).UseStartup<Startup>();
    }
}