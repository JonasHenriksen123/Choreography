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

        public async Task<Account[]> GetAllAccounts()
        {
            return await this.getAsync<Account[]>($"{AccountsBaseUrl}Account") ?? Array.Empty<Account>();
        }

        public async Task<IPartialItem[]> GetAllItems()
        {
            return await this.getAsync<Item[]>($"{StockBaseUrl}Item") ?? Array.Empty<Item>();
        }

        public async Task<Item?> GetItem(Guid id)
        {
            return await this.getAsync<Item>($"{StockBaseUrl}Item/{id}");
        }

        public async Task<Item?> CreateItem(Item item)
        {
            return await this.postAsync<Item>($"{StockBaseUrl}Item", item);
        }

        public async Task<Account?> GetAccount(Guid id)
        {
            return await this.getAsync<Account>($"{AccountsBaseUrl}Account/{id}");
        }

        public async Task<Account?> CreateAccount(Account account)
        {
            return await this.postAsync<Account>($"{AccountsBaseUrl}Account", account);
        }

        public async Task<Invoice?> GetInvoice(Guid id)
        {
            return await this.getAsync<Invoice>($"{AccountsBaseUrl}Invoice/{id}");
        }

        public async Task<Invoice[]> GetInvoicesForAccount(Guid id)
        {
            return await this.getAsync<Invoice[]>($"{AccountsBaseUrl}Account/{id}/invoices") ?? Array.Empty<Invoice>();
        }

        public async Task<Order[]> GetOrders()
        {
            return await this.getAsync<Order[]>($"{OrdersBaseUrl}order") ?? Array.Empty<Order>();
        }

        public async Task<Order?> GetOrder(Guid id)
        {
            return await this.getAsync<Order>($"{OrdersBaseUrl}order/{id}");
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            return await this.postAsync<Order>($"{OrdersBaseUrl}Order", order);
        }

        public async Task<Event[]> GetEventsForOrder(Guid id)
        {
            return await this.getAsync<Event[]>($"{BrokerBaseUrl}event/{id}") ?? Array.Empty<Event>();
        }

        public async Task<OrderLine[]> GetOrderLinesForOrder(Guid id)
        {
            return await this.getAsync<OrderLine[]>($"{OrdersBaseUrl}order/{id}/orderlines") ?? Array.Empty<OrderLine>();
        }

        public async Task<bool> ConfirmOrder(Guid id)
        {
            return await this.getAsync($"{OrdersBaseUrl}order/{id}/confirm");
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

        private async Task<bool> getAsync(string url)
        {
            var resp = await client.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                return true;
            }

            return false;

        }

        private async Task<T?> postAsync<T>(string url, object data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var resp = await client.PostAsync(url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return default;
        }
        #endregion
    }
}
