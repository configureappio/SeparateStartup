using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.Domain;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.EnglishInformal;
using ConfigureAppIo.Demos.SayHello.French;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.InfraBaseTests
{
    public abstract class WebApiTestsBase
    {
        private static readonly Lazy<object> ThreadLock = new Lazy<object>(() => new object());

        private readonly Dictionary<string, string[]> _allErrors = new Dictionary<string, string[]>
        {
            {
                "Name", new[] {"Name is required."}
            },
            {
                "Language", new[] {"Language is required."}
            }
        };

        protected abstract IWebHostBuilder GetWebHostBuilder(string[] args);

        protected abstract ISayHelloFactory GetSayHelloFactory(IEnumerable<ISayHello> instances);

        protected abstract string GetAllMessagesFromController(ISayHelloFactory helloFactory,
            IEnumerable<ISayHelloLanguage> languageHellos,
            IEnumerable<ISayHello> allHellos,
            IEnglishHello englishHello,
            IFrenchHello frenchHello);


        [Fact]
        public async Task SayHelloToMe_Should_Return_ErrorDictionary_When_Unpopulated_Object_Submitted()
        {
            var firstAvailablePort = GetFirstAvailablePort(5000);
            using (var server = new TestServer(GetWebHostBuilder(null)))
            {
                server.BaseAddress = new Uri($"https://localhost{firstAvailablePort}");
                using (var client = server.CreateClient())
                {
                    client.BaseAddress = server.BaseAddress;

                    var paramToTest = new Person();
                    var content = JsonConvert.SerializeObject(paramToTest);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/SayHelloToMe", stringContent);
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var resultAsDictionary = JsonConvert.DeserializeObject(responseString, _allErrors.GetType());
                    resultAsDictionary.Should().BeEquivalentTo(_allErrors);
                }
            }
        }

        [Fact]
        public async Task SayHelloToMe_Should_Return_Message_In_English()
        {
            var expectedResult = "Hello Test";
            var person = new Person { Language = "English", Name = "Test" };
            await TestResponseFromSayHelloToMe(expectedResult, person, GetWebHostBuilder(null));
        }

        [Fact]
        public async Task SayHelloToMe_Should_Return_Message_In_French()
        {
            var expectedResult = "Bonjour Test";
            var person = new Person { Language = "French", Name = "Test" };
            await TestResponseFromSayHelloToMe(expectedResult, person, GetWebHostBuilder(null));
        }

        [Fact]
        public async Task SayHelloToMe_Should_Return_Message_In_Engish_When_Unknown_Language()
        {
            var person = new Person { Language = "German", Name = "Test" };
            var expectedResult = $"I'm sorry, I don't speak {person.Language}. I can talk to you in English. Hello Test";
            await TestResponseFromSayHelloToMe(expectedResult, person, GetWebHostBuilder(null));
        }




        private static async Task TestResponseFromSayHelloToMe(string expectedResult, Person person, IWebHostBuilder webHost)
        {
            var firstAvailablePort = GetFirstAvailablePort(5000);
            using (var server = new TestServer(webHost))
            {
                server.BaseAddress = new Uri($"https://localhost{firstAvailablePort}");
                using (var client = server.CreateClient())
                {
                    client.BaseAddress = server.BaseAddress;

                    var paramToTest = person;
                    var content = JsonConvert.SerializeObject(paramToTest);
                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/SayHelloToMe", stringContent);
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString.Should().Be(expectedResult);
                }
            }
        }

        [Fact]
        public async Task GetMessages_Should_Return_List_Of_Languages()
        {
            var firstAvailablePort = GetFirstAvailablePort(5000);
            using (var server = new TestServer(GetWebHostBuilder(null)))
            {
                server.BaseAddress = new Uri($"https://localhost{firstAvailablePort}");
                using (var client = server.CreateClient())
                {
                    client.BaseAddress = server.BaseAddress;

                    var response = await client.GetAsync("/GetMessages");
                    var responseString = await response.Content.ReadAsStringAsync();
                    var resultAsDictionary = JsonConvert.DeserializeObject<string[]>(responseString);
                    resultAsDictionary.Should().BeEquivalentTo(GetMessagesApiExpectedResult());
                }
            }
        }


        [Fact]
        public async Task Get_HomePage_Should_Return_List_Of_Languages()
        {
            var firstAvailablePort = GetFirstAvailablePort(5000);
            using (var server = new TestServer(GetWebHostBuilder(null)))
            {
                server.BaseAddress = new Uri($"https://localhost{firstAvailablePort}");
                using (var client = server.CreateClient())
                {
                    client.BaseAddress = server.BaseAddress;

                    var response = await client.GetAsync("/");
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString.Should().BeEquivalentTo(GetAllMessages());
                }
            }
        }

        private string GetAllMessages()
        {
            var englishLang = new EnglishHello();
            var frenchHello = new FrenchHello();
            var englishInformat = new EnglishInformalHello();
            var allHellos = new ISayHello[] { englishLang, frenchHello, englishInformat };
            var allLangs = allHellos.Where(l => l is ISayHelloLanguage).Cast<ISayHelloLanguage>();
            var factory = GetSayHelloFactory(allHellos);
            return GetAllMessagesFromController(factory, allLangs, allHellos, englishLang, frenchHello);
        }

        private string[] GetMessagesApiExpectedResult()
        {
            return GetAllMessages().Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        private static int GetFirstAvailablePort(int startingPort)
        {
            lock (ThreadLock.Value)
            {
                var portArray = new List<int>();

                var properties = IPGlobalProperties.GetIPGlobalProperties();

                portArray.AddRange(
                    properties.GetActiveTcpConnections().Where(n => n.LocalEndPoint.Port >= startingPort).Select(p => p.LocalEndPoint.Port));

                portArray.AddRange(
                    properties.GetActiveTcpListeners().Where(n => n.Port >= startingPort).Select(n => n.Port));

                portArray.AddRange(
                    properties.GetActiveUdpListeners().Where(n => n.Port >= startingPort).Select(n => n.Port));

                return Enumerable.Range(startingPort, short.MaxValue - startingPort).Except(portArray).FirstOrDefault();
            }
        }
    }
}
