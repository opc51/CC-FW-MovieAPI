using System.IO;
using PactNet;
using Xunit.Abstractions;
using Xunit;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet.Matchers;

namespace MovieTests.ConsumerTests
{
    public class ApiTests
    {
        private IPactBuilderV3 pact;
        private readonly ApiClient ApiClient;
        private readonly int port = 9000;
        private readonly List<object> products;

        public ApiTests(ITestOutputHelper output)
        {
            var Config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "..", "..", "pacts"),
                Outputters = new[] { new XUnitOutput(output) },
                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            pact = Pact.V3("ApiClient", "MovieService", Config).WithHttpInteractions();
            ApiClient = new ApiClient();
        }

        [Fact]
        public async void GetAllMovies()
        {
            // Arange
            pact.UponReceiving("A valid request for all movies")
                    .Given("movies exist")
                    .WithRequest(HttpMethod.Get, "/api/movies?Title=super")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new TypeMatcher(products));

            await pact.VerifyAsync(async ctx => {
                var response = await ApiClient.GetMovies();
                Assert.Equal(5, response.Count);
            });
        }
    }
}
