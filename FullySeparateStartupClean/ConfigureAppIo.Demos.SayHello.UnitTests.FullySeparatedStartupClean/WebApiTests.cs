using System.Collections.Generic;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.French;
using ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean;
using ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean.Controllers;
using ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean.Infra;
using ConfigureAppIo.Demos.SayHello.UnitTests.Common.InfraBaseTests;
using Microsoft.AspNetCore.Hosting;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.FullySeparatedStartup
{
    public class WebApiTests : WebApiTestsBase
    {
        protected override IWebHostBuilder GetWebHostBuilder(string[] args) => Program.ConfigureWebHostBuilder(args);


        protected override ISayHelloFactory GetSayHelloFactory(IEnumerable<ISayHello> instances)
        {
            return new SayHelloFactory(instances);
        }

        protected override string GetAllMessagesFromController(ISayHelloFactory helloFactory, IEnumerable<ISayHelloLanguage> languageHellos, IEnumerable<ISayHello> allHellos,
            IEnglishHello englishHello, IFrenchHello frenchHello)
        {
            return HomeController.BuildAllMessagesAsSingleMessage(helloFactory, languageHellos, allHellos, englishHello, frenchHello);
        }
    }
}
