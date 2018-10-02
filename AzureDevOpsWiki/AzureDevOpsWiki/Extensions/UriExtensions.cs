using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.Extensions
{
    public static class UriExtensions
    {
        public static async Task<byte[]> GetImageRestAsync(
            this Uri baseAddress, string requestPath, Action<HttpClient> configureClient)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddress;
                configureClient(client);
                var response = await client.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }

            return default(byte[]);
        }

        public static async Task<TPayload> GetRestAsync<TPayload>(
            this Uri baseAddress, string requestPath, Action<HttpClient> configureClient)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddress;
                configureClient(client);
                var response = await client.GetAsync(requestPath);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadContentAsync<TPayload>();
                }
            }

            return default(TPayload);
        }

        public static async Task<TResponseType> PostRestAsync<TResponseType>(
            this Uri baseAddress, string requestPath, object payload, Action<HttpClient> configureClient)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddress;
                configureClient(client);
                var jsonString = JsonConvert.SerializeObject(payload,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestPath, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadContentAsync<TResponseType>();
                }
            }

            return default(TResponseType);
        }

        public static async Task<TPayload> ReadContentAsync<TPayload>(this HttpContent content)
        {
            var contentString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TPayload>(contentString);
        }
    }
}
