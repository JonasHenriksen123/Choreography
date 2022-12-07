using Accounts.ServiceAPI;
using Accounts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    [Route("info/[action]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IVersionService versionService;
        public InfoController(IVersionService versionService) { this.versionService = versionService; }

        public IActionResult Version()
        {
            return Ok($"Version: {this.versionService.Version}");
        }
    }
}
