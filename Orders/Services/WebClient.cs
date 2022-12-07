using Orders.Model;
using Orders.ServiceAPI;
using System.Text.Json;

namespace Orders.Services
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient client;

        public const string BrokerBaseUrl = "http://localhost/Broker/";

        public WebClient() 
        {
            client = new HttpClient();
        }

        public async Task<bool> PostEvent(Event @event)
        {
            return await this.postAsync($"{ BrokerBaseUrl }event", @event);
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

        #endregion
    }
}
