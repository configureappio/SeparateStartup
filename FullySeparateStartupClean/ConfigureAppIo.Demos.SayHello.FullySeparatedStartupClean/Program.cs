using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean.Infra;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean
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

            // from https://github.com/aspnet/Hosting/issues/903#issuecomment-269103645

            hostBuilder
                .UseSetting(WebHostDefaults.ApplicationKey,
                    typeof(Program).GetTypeInfo().Assembly
                        .FullName) // Ignore the startup class assembly as the "entry point" and instead point it to this app
                ;

            return hostBuilder;
        }
    }
}
