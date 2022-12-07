﻿using Stock.ServiceAPI;

namespace Stock.Services
{
    public class VersionService : IVersionService
    {
        public string Version => typeof(VersionService).Assembly.GetName().Version?.ToString() ?? "Unknown";
    }
}
