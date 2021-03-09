using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IFileStore fileStore;
        private readonly IRequestVerifier requestVerifier;

        public DownloadController(IFileStore fileStore, IRequestVerifier requestVerifier)
        {
            this.fileStore = fileStore;
            this.requestVerifier = requestVerifier;
        }
        
        [HttpGet("{id}/{key}")]
        public IActionResult Get(string id, string key)
        {
            if (!requestVerifier.VerifyDownloadRequest(id, key))
            {
                ModelState.AddModelError("Key", $"The request couldn't be authorized.");
                return BadRequest(ModelState);
            }

            if (!fileStore.FileExists(id))
            {
                ModelState.AddModelError("Id", $"File doesn't exist.");
                return BadRequest(ModelState);
            }

            var physicalFile = fileStore.GetFile(Guid.Parse(id));
            
            return new PhysicalFileResult(physicalFile.FullName, "application/octet-stream"){FileDownloadName = "WowDatabaseEditor.zip"};
        }
    }
}