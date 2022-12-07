using ShopAPI.Model;
using ShopAPI.ServiceAPI;
using System.Text.Json;

namespace ShopAPI.Services
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient client;

        private const string AccountsBaseUrl = "http://localhost/Accounts/";
        private const string OrdersBaseUrl = "http://localhost/Orders/";
        private const string StockBaseUrl = "http://localhost/Stock/";
        private const string BrokerBaseUrl = "http://localhost/Broker/";

        public WebClient() 
        {
            client = new HttpClient();
        }

        public async Task<Account?> GetAccount(int id)
        {
            return await this.getAsync<Account>($"{AccountsBaseUrl}Account/{id}");
        }

        public async Task<Account[]> GetAllAccounts()
        {
            return await this.getAsync<Account[]>($"{AccountsBaseUrl}Account") ?? Array.Empty<Account>();
        }

        #region private helpers
        private async Task<T?> getAsync<T>(string url)
        {
            var resp = await client.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                return await  resp.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            
            return default;
            
        }
        #endregion
    }
}
