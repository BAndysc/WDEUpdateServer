using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckVersionController : ControllerBase
    {
        [HttpPost]
        public CheckVersionResponse Post(CheckVersionRequest request)
        {
            return new CheckVersionResponse(request.CurrentVersion, "abc",
                new ChangeLogEntry(121231, 
                    "1.0 beta",
                    DateTime.Now, "Fixed bug in xyz", "Abc now works as intended"));
        }
        
    }
}