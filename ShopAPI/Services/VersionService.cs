using ShopAPI.ServiceAPI;

namespace ShopAPI.Services
{
    public class VersionService : IVersionService
    {
        public string Version => typeof(VersionService).Assembly.GetName().Version?.ToString() ?? "Unknown";
    }
}
