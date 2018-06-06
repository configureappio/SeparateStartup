using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.Controllers;
using ConfigureAppIo.Demos.SayHello.Domain;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.EnglishInformal;
using ConfigureAppIo.Demos.SayHello.Factories;
using ConfigureAppIo.Demos.SayHello.French;
using ConfigureAppIo.Demos.SayHello.UnitTests.Common.InfraBaseTests;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Xunit;
using Microsoft.AspNetCore.TestHost;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.BeforeSeparation
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
