using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ConfigureAppIo.Demos.SayHello
{
    public static class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            ConfigureWebHostBuilder(args).Build().Run();
        }

        internal static IWebHostBuilder ConfigureWebHostBuilder(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            hostBuilder.UseStartup<Startup>();
            return hostBuilder;
        }
    }
}
