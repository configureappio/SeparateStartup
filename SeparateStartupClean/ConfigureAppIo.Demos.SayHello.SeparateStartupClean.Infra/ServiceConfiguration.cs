using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.Domain;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.EnglishInformal;
using ConfigureAppIo.Demos.SayHello.French;
using ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure.Internal;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            services.AddTransient(provider => new ValidatorActionFilter());

            mvcBuilder.AddMvcOptions(o => o.Filters.AddService<ValidatorActionFilter>());
                

            mvcBuilder.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<PersonValidation>());

            services.AddSingleton<IEnglishHello, EnglishHello>();
            services.AddSingleton<IFrenchHello, FrenchHello>();
            services.AddSingleton<IEnglishInformalHello, EnglishInformalHello>();
            services.AddSingleton<ISayHelloLanguage>(provider => provider.GetService<IEnglishHello>());
            services.AddSingleton<ISayHelloLanguage>(provider => provider.GetService<IFrenchHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IEnglishHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IFrenchHello>());
            services.AddSingleton<ISayHello>(provider => provider.GetService<IEnglishInformalHello>());
            services.AddSingleton<ISayHelloFactory>(provider =>
                new SayHelloFactory(provider.GetService<IEnumerable<ISayHello>>()));           
        }

        public static IServiceProvider BuildAlternativeServiceProvider(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }
    }
}
