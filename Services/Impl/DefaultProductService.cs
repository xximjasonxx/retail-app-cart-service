
using System;
using System.Net;
using System.Threading.Tasks;
using CartApi.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CartApi.Services.Impl
{
    public class DefaultProductService : IProductService
    {
        private readonly RestClient restClient;

        public DefaultProductService(string apiEndpoint)
        {
            this.restClient = new RestClient(apiEndpoint);
        }

        public async Task<Product> GetProductAsync(string id)
        {
            var request = new RestRequest($"product/{id}");
            var response = await this.restClient.ExecuteGetTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var contentString = response.Content;
            return JsonConvert.DeserializeObject<Product>(contentString);
        }
    }
}