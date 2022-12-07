using Broker.Model;

namespace Broker.ServiceAPI
{
    public interface IWebClient
    {
        public enum Reciever 
        {
            Accounts = 0,
            Orders = 1,
            Stock = 2
        }

        Task<bool> SendEvent(Event @event, Reciever reciever); 
    }
}
