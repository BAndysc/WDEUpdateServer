using System;
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
        public async Task<IActionResult> Post(IFormCollection data, IList<IFormFile> files)
        {
            if (!data.TryGetValue("branch", out var branch))
                return BadRequest();
            if (!data.TryGetValue("marketplace", out var marketplace))
                return BadRequest();
            if (!data.TryGetValue("version", out var versionString) || !long.TryParse(versionString, out var version))
                return BadRequest();
            if (!data.TryGetValue("versionName", out var versionName))
                return BadRequest();
            if (!data.TryGetValue("user", out var user))
                return BadRequest();
            if (!data.TryGetValue("key", out var key))
                return BadRequest();
            if (!data.TryGetValue("platform", out var platformString) ||
                !Enum.TryParse(typeof(Platforms), platformString, true, out var platfomEnum) ||
                platfomEnum is not Platforms platform)
                return BadRequest();
            
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

            var userModel = await databaseRepository.GetUserByName(request.User);
            
            var updateVersion = await databaseRepository.GetOrCreateVersion(request.Marketplace, request.Branch, request.Version,
                request.VersionName);
            
            var file = await fileService.AddFile(userModel, request.Platform, request.Marketplace, request.Branch, request.Version, files[0]);

            var oldFiles = await databaseRepository.GetOldFiles(request.Marketplace, request.Branch, request.Version, request.Platform);

            foreach (var oldFile in oldFiles)
                await fileService.RemoveFile(oldFile.File);

            await databaseRepository.InsertVersionFile(updateVersion, request.Platform, file);

            return Ok(new UploadVersionResponse() {Id = file.Key});
        }
    }
}