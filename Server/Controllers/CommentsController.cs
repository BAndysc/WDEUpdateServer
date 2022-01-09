using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models.API;
using Server.Services;
using Server.Services.Database;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IDatabaseRepository repository;
    private readonly ISpamDetector spamDetector;

    public CommentsController(IDatabaseRepository repository, ISpamDetector spamDetector)
    {
        this.repository = repository;
        this.spamDetector = spamDetector;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Post(AddCommentRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) ||
            string.IsNullOrEmpty(request.Text))
            return BadRequest();

        var ip = this.Request.HttpContext.Connection.RemoteIpAddress;

        if (ip == null)
            return StatusCode(403);
        
        if (!spamDetector.TryMakeRequest(ip))
            return StatusCode(401);
        
        await repository.AddComment(ip.ToString(), request.Username, request.Text, Request.Headers.UserAgent.ToString());

        return Ok();
    }
}