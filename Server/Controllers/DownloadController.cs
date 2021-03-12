using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly IRequestVerifier requestVerifier;

        public DownloadController(IFileService fileService, IRequestVerifier requestVerifier)
        {
            this.fileService = fileService;
            this.requestVerifier = requestVerifier;
        }
        
        [HttpGet("{id}/{key}")]
        public async Task<IActionResult> Get(string id, string key)
        {
            if (!await requestVerifier.VerifyDownloadRequest(id, key))
            {
                ModelState.AddModelError("errors", $"The request couldn't be authorized.");
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(id, out var guid))
            {
                ModelState.AddModelError("errors", $"Invalid guid");
                return BadRequest(ModelState);
            }

            if (!await fileService.FileExists(guid))
            {
                ModelState.AddModelError("errors", $"File doesn't exist.");
                return BadRequest(ModelState);
            }

            var physicalFile = await fileService.GetFile(Guid.Parse(id));
            
            if (physicalFile == null)
            {
                ModelState.AddModelError("errors", $"File doesn't exist.");
                return BadRequest(ModelState);
            }
            
            return new PhysicalFileResult(physicalFile.FullName, "application/octet-stream"){FileDownloadName = "WowDatabaseEditor.zip"};
        }
    }
}