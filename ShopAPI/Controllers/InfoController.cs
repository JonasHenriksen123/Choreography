using Microsoft.AspNetCore.Mvc;
using ShopAPI.ServiceAPI;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [Route("info/[action]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IVersionService versionService;
        public InfoController(IVersionService versionService) { this.versionService = versionService; }

        public ActionResult Version()
        {
            return Ok($"Version: {this.versionService.Version}");
        }
    }
}
