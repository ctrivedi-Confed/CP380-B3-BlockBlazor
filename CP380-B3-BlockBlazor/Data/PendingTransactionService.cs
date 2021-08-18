using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CP380_B1_BlockList.Models;

namespace CP380_B3_BlockBlazor.Data
{
    public class PendingTransactionService
    {
        // TODO: Add variables for the dependency-injected resources
        //       - httpClient
        //       - configuration
        //

        //
        // TODO: Add a constructor with IConfiguration and IHttpClientFactory arguments
        //

        //
        // TODO: Add an async method that returns an IEnumerable<Payload> (list of Payloads)
        //       from the web service
        //
        static HttpClient _httpClient;
        private readonly IConfiguration _config;

        private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        public PendingTransactionService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = configuration.GetSection("PayloadService");
        }

        public async Task<IEnumerable<Payload>> GetPayloads()
        {
            var responce = await _httpClient.GetAsync(_config["url"]);
            //JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            if (responce.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<Payload>>(
                        await responce.Content.ReadAsStreamAsync(), JsonSerializerOptions
                    );
            }

            return Array.Empty<Payload>();
        }

        public async Task<HttpResponseMessage> SubmitPayload(Payload payload)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload, JsonSerializerOptions),
                System.Text.Encoding.UTF8,
                "application/json"
                );

            var res = await _httpClient.PostAsync(_config["url"], content);
            return res;
        }
    }
}
