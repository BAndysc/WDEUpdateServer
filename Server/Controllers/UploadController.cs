using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private IRequestVerifier requestVerifier;
        private IFileStore fileStore;
        
        public UploadController(IRequestVerifier requestVerifier, IFileStore fileStore)
        {
            this.requestVerifier = requestVerifier;
            this.fileStore = fileStore;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(string branch, 
            string marketplace, 
            long version,
            string versionName, 
            string key,
            Platforms platform,
            IList<IFormFile> files)
        {
            var request = new UploadVersionRequest()
            {
                Branch = branch,
                Marketplace = marketplace,
                Version = version,
                VersionName = versionName,
                Platform = platform,
                Key = key
            };
            if (files.Count != 1 || !MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", $"The request couldn't be processed (Error 1).");
                return BadRequest(ModelState);
            }

            if (!requestVerifier.VerifyUploadRequest(request, request.Key))
            {
                ModelState.AddModelError("Key", $"Invalid key");
                return BadRequest(ModelState);
            }

            var fileId = await fileStore.AddFile(request.Platform, request.Marketplace, request.Branch, request.Version, files[0]);

            return Ok(new UploadVersionResponse() {Id = fileId});
        }
    }
}