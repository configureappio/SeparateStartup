using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConfigureAppIo.Demos.SayHello.ActionFilters;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.Domain;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.EnglishInformal;
using ConfigureAppIo.Demos.SayHello.Factories;
using ConfigureAppIo.Demos.SayHello.French;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;

namespace ConfigureAppIo.Demos.SayHello
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.Filters.Add(typeof(ValidatorActionFilter)))
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<PersonValidation>());

            services.AddSingleton<IEnglishHello, EnglishHello>();
            services.AddSingleton<IFrenchHello, FrenchHello>();
            services.AddSingleton<IEnglishInformalHello, EnglishInformalHello>();
            services.AddSingleton<ISayHelloLanguage>(provider => provider.GetService<IEnglishHello>());
            services.AddSingleton<ISayHelloLanguage>(provider => provider.GetService<IFrenchHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IEnglishHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IFrenchHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IEnglishInformalHello>());
            services.AddSingleton<ISayHelloFactory, SayHelloFactory>();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
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
