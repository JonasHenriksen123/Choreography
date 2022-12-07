using ShopAPI.Model;

namespace ShopAPI.ServiceAPI
{
    public interface IWebClient
    {
        Task<Account[]> GetAllAccounts();

        Task<Account?> GetAccount(int id);
    }
}
