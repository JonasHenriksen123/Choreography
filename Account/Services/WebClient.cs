using Accounts.Model;
using Accounts.ServiceAPI;
using System.Text.Json;

namespace Accounts.Services
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient client;

        public const string BrokerBaseUrl = "http://localhost/Broker/";
        public const string OrderBaseUrl = "http://localhost/Orders";

        public WebClient() 
        {
            client = new HttpClient();
        }

        public async Task<bool> PostEvent(Event @event)
        {
            return await this.postAsync($"{ BrokerBaseUrl }event", @event);
        }

        public async Task<Order?> GetOrder(Guid id)
        {
            return await this.getAsync<Order>($"{OrderBaseUrl}Order/{id}");
        }

        #region private helpers

        private async Task<bool> postAsync(string url, object data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var resp = await client.PostAsync(url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

            if (resp.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        private async Task<T?> getAsync<T>(string url)
        {
            var resp = await client.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return default;
        }
        #endregion
    }
}
