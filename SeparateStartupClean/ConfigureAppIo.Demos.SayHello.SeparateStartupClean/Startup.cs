using System;
using System.Diagnostics.CodeAnalysis;
using ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigureAppIo.Demos.SayHello
{
public class Startup : IStartup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
    {                        
        var mvcBuilder = ConfigureLocalServices(services);
        ConfigureExternalServices(services, mvcBuilder);
        return services.BuildAlternativeServiceProvider();
    }

    public void Configure(IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetService<IHostingEnvironment>();
        SetErrorHandling(app, env);

        app.UseStaticFiles();

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private IMvcBuilder ConfigureLocalServices(IServiceCollection services)
    {
        return services.AddMvc();
    }
    
    private void ConfigureExternalServices(IServiceCollection services, IMvcBuilder mvcBuilder)
    {            
        ServiceConfiguration.ConfigureServices(services, mvcBuilder);
    }

    [ExcludeFromCodeCoverage]
    private static void SetErrorHandling(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }
    }
}
}
