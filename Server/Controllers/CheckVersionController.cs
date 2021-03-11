using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Server.Models;
using Server.Models.API;
using Server.Models.Database;
using Server.Services;
using Server.Services.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckVersionController : ControllerBase
    {
        private readonly IDatabaseRepository repository;
        private readonly IRequestVerifier requestVerifier;

        public CheckVersionController(IDatabaseRepository repository, IRequestVerifier requestVerifier)
        {
            this.repository = repository;
            this.requestVerifier = requestVerifier;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(CheckVersionRequest request)
        {
            if (!requestVerifier.VerifyCheckVersionRequest(request, request.Key))
            {
                ModelState.AddModelError("errors", $"The request couldn't be authorized.");
                return BadRequest(ModelState);
            }
            
            var updates = await repository.GetChangelog(request.Marketplace, request.Branch, request.CurrentVersion, request.Platform);
            var latestVersion = updates.Count > 0 ? updates[0].version.Version : request.CurrentVersion;
            var downloadFile = updates.Count > 0 ? (Guid?)updates[0].file.File.Key : null;
            
            return Ok(new CheckVersionResponse(latestVersion, downloadFile == null ? null : $"/Download/{downloadFile}/{request.Key}",
                updates.Select(version => new ChangeLogEntry(version.version.Version, version.version.TextVersion, version.version.ReleaseDate, "Release")).ToArray()));
        }
    }
}