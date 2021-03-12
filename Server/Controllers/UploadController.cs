using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models.API;
using Server.Services;
using Server.Services.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IRequestVerifier requestVerifier;
        private readonly IFileService fileService;
        private readonly IDatabaseRepository databaseRepository;

        public UploadController(IRequestVerifier requestVerifier, IFileService fileService, IDatabaseRepository databaseRepository)
        {
            this.requestVerifier = requestVerifier;
            this.fileService = fileService;
            this.databaseRepository = databaseRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(string branch, 
            string marketplace, 
            long version,
            string versionName, 
            string user,
            string key,
            Platforms platform,
            IList<IFormFile> files)
        {
            var request = new UploadVersionRequest(branch, marketplace, version, platform, versionName, user, key);
            
            if (files.Count != 1 || !MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("errors", $"The request couldn't be processed.");
                return BadRequest(ModelState);
            }

            if (!await requestVerifier.VerifyUploadRequest(request, request.User, request.Key))
            {
                ModelState.AddModelError("errors", $"Invalid key");
                return BadRequest(ModelState);
            }

            var updateVersion = await databaseRepository.GetOrCreateVersion(request.Marketplace, request.Branch, request.Version,
                request.VersionName);

            var file = await fileService.AddFile(request.Platform, request.Marketplace, request.Branch, request.Version, files[0]);

            await databaseRepository.InsertVersionFile(updateVersion, request.Platform, file);
            
            return Ok(new UploadVersionResponse() {Id = file.Key});
        }
    }
}