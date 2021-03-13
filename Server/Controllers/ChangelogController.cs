using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models.API;
using Server.Services;
using Server.Services.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangelogController : ControllerBase
    {
        private readonly IDatabaseRepository repository;
        private readonly IRequestVerifier requestVerifier;

        public ChangelogController(IDatabaseRepository repository, IRequestVerifier requestVerifier)
        {
            this.repository = repository;
            this.requestVerifier = requestVerifier;
        }
        
        [HttpPost("Add")]
        public async Task<IActionResult> Post(AddChangelogEntryRequest request)
        {
            if (!await requestVerifier.VerifyModifyChangelogRequest(request.User))
            {
                ModelState.AddModelError("errors", $"The request couldn't be authorized.");
                return BadRequest(ModelState);
            }

            var version = await repository.GetVersion(request.Version.Marketplace, request.Version.Branch, request.Version.Version);
            
            if (version == null)
            {
                ModelState.AddModelError("errors", $"Version not found");
                return BadRequest(ModelState);
            }

            await repository.AddChangelogEntry(version, request.Entry);

            return Ok();
        }
    }
}