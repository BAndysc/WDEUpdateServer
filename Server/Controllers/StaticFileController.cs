
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("Static/")]
    public class StaticFileController : ControllerBase
    {
        private readonly IStaticFileService staticFileService;

        public StaticFileController(IStaticFileService staticFileService)
        {
            this.staticFileService = staticFileService;
        }
        
        [HttpGet("{filename}")]
        public async Task<IActionResult> Index(string filename)
        {
            var physicalFile = await staticFileService.GetFilePath(filename);
            if (physicalFile == null)
                return NotFound();
            return new PhysicalFileResult(physicalFile.FullName, "application/octet-stream"){FileDownloadName = filename};
        }
    }
}