using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using CP380_B1_BlockList.Models;

namespace CP380_B3_BlockBlazor.Data
{
    public class BlockService
    {
        // TODO: Add variables for the dependency-injected resources
        //       - httpClient
        //       - configuration
        //
        static HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        public BlockService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = configuration.GetSection("BlockService");
        }

        public async Task<IEnumerable<Block>> GetBlock()
        {
            var responce = await _httpClient.GetAsync(_config["url"]);

            if (responce.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                return await JsonSerializer.DeserializeAsync<IEnumerable<Block>>(
                        await responce.Content.ReadAsStreamAsync() , options
                    );
            }

            return Array.Empty<Block>();
        }
        public async Task<Block> SubmitNewBlockAsync(Block block)
        {
            var content = new StringContent(
               JsonSerializer.Serialize(block, JsonSerializerOptions),
               System.Text.Encoding.UTF8,
               "application/json"
               );

            var res = await _httpClient.PostAsync(_config["url"], content);
            if (res.IsSuccessStatusCode)
                return block;
            else
                return null;
        }
    }
}

