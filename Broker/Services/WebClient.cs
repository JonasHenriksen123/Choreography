using Broker.Model;
using Broker.ServiceAPI;
using System.Text.Json;

namespace Broker.Services
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient client;

        private const string AccountsBaseUrl = "http://localhost/Accounts/";
        private const string OrdersBaseUrl = "http://localhost/Orders/";
        private const string StockBaseUrl = "http://localhost/Stock/";

        public WebClient() 
        {
            client = new HttpClient();
        }

        public async Task<bool> SendEvent(Event @event, IWebClient.Reciever reciever)
        {
            var baseUrl = reciever switch
            {
                IWebClient.Reciever.Orders => OrdersBaseUrl,
                IWebClient.Reciever.Stock => StockBaseUrl,
                IWebClient.Reciever.Accounts => AccountsBaseUrl,
                _ => throw new Exception("Unknown")
            };

            return await this.postAsync($"{baseUrl}Event", @event);
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
