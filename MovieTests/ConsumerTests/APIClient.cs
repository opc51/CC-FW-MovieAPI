using MovieAPI.Models.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieTests.ConsumerTests
{
    public class ApiClient
    {
        private readonly HttpClient client;

        public ApiClient()
        {
            client = new HttpClient { BaseAddress = new Uri("https://localhost:44365/") };
        }

        public async Task<List<Movie>> GetMovies()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api//Movies?Title=super");
            request.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                           ? JsonConvert.DeserializeObject<List<Movie>>(content)
                           : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}
