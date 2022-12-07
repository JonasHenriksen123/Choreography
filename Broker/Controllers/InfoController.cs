using Broker.ServiceAPI;
using Broker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Controllers
{
    [Route("info/[action]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private IVersionService versionService;
        public InfoController() { this.versionService = new VersionService(); }

        public IActionResult Version()
        {
            return Ok($"Version: {this.versionService.Version}");
        }
    }
}
