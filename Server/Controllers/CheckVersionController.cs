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
            if (!await requestVerifier.VerifyCheckVersionRequest(request, request.Key))
            {
                ModelState.AddModelError("errors", $"The request couldn't be authorized.");
                return BadRequest(ModelState);
            }

            var platform = request.Platform;
            if (platform == Platforms.Windows &&
                (!request.OsMajorVersion.HasValue ||
                 !request.OsMinorVersion.HasValue ||
                 !request.OsPlatformId.HasValue))
                platform = Platforms.Windows7; // no version <-- firstly provide win7 version

            var downloads = await repository.GetLatestVersion(request.Marketplace, request.Branch,
                request.CurrentVersion, platform);

            if (downloads.Count == 0)
            {
                return Ok(new CheckVersionResponse(request.CurrentVersion, null, null));
            }

            var latestVersion = downloads[0].version.Version;
            var downloadFile = downloads[0].file.Key;
            var updates = await repository.GetChangelog(request.Marketplace, request.Branch, request.CurrentVersion, request.Platform);
            
            return Ok(new CheckVersionResponse(latestVersion,  $"/Download/{downloadFile}/{request.Key}",
                    downloads[0].file.HashMd5,
                    updates
                    .Where(u => u.Version <= latestVersion)
                    .Select(version => new ChangeLogEntry(version.Version, version.TextVersion, version.ReleaseDate, version.UpdateTitle, version.Changes.Select(change => change.Change).ToArray())).ToArray()));
        }
    }
}