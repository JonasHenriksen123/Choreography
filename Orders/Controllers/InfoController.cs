using Microsoft.AspNetCore.Mvc;
using Orders.IServiceAPI;
using Orders.Services;

namespace Orders.Controllers
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
