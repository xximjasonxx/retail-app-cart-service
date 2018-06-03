
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CartApi.Common;
using CartApi.Models;
using RestSharp;

namespace CartApi.Extensions
{
    public static class StringExtensions
    {
        public static async Task<User> ValidateToken(this string token)
        {
            var client = new RestClient(Environment.GetEnvironmentVariable("LOGIN_API_URL"));
            var request = new RestRequest("validate", Method.GET);
            request.AddHeader("Authorization", token);

            var response = await client.ExecuteGetTaskAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.Content);
                return null;
            }

            return JsonConvert.DeserializeObject<User>(response.Content);
        }
    }
}