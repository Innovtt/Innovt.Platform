namespace SampleWebApiTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(Environment.GetCommandLineArgs()).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder.AddEnvironmentVariables();
                    builder.AddJsonFile($"appsettings.json");
                    builder.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}