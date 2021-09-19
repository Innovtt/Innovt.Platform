// Company: Antecipa
// Project: AppSample
// Solution: Innovt.Contrib.Authorization
// Date: 2021-08-07

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AppSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}