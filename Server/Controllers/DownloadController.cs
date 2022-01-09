using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly IRequestVerifier requestVerifier;
        private readonly IDatabaseRepository repository;

        public DownloadController(IFileService fileService, IRequestVerifier requestVerifier,
            IDatabaseRepository repository)
        {
            this.fileService = fileService;
            this.requestVerifier = requestVerifier;
            this.repository = repository;
        }
        
        [HttpGet("{id}/{key?}")]
        public async Task<IActionResult> Get(string id, string? key = null)
        {
            if (!await requestVerifier.VerifyDownloadRequest(id, key))
            {
                ModelState.AddModelError("errors", $"The request couldn't be authorized.");
                await repository.Log(Request, $"/Download (NOT AUTHORIZED {id}, {key})");
                return BadRequest(ModelState);
            }

            if (!Guid.TryParse(id, out var guid))
            {
                ModelState.AddModelError("errors", $"Invalid guid");
                await repository.Log(Request, "/Download (INVALID GUID)");
                return BadRequest(ModelState);
            }

            if (!await fileService.FileExists(guid))
            {
                ModelState.AddModelError("errors", $"File doesn't exist.");
                await repository.Log(Request, "/Download (FILE DOESN'T EXIST)");
                return BadRequest(ModelState);
            }

            var physicalFile = await fileService.GetFile(Guid.Parse(id));
            
            if (physicalFile == null)
            {
                ModelState.AddModelError("errors", $"File doesn't exist.");
                await repository.Log(Request, "/Download (FILE DOESN'T EXIST)");
                return BadRequest(ModelState);
            }
            
            await repository.Log(Request, "/Download");
            
            return new PhysicalFileResult(physicalFile.FullName, "application/octet-stream"){FileDownloadName = "WowDatabaseEditor.zip"};
        }
    }
}