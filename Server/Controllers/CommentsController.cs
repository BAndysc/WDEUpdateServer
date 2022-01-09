using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models.API;
using Server.Services.Database;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IDatabaseRepository repository;

    private Dictionary<IPAddress, DateTime> spamDetector = new();

    public CommentsController(IDatabaseRepository repository)
    {
        this.repository = repository;
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
        
        lock (spamDetector)
        {
            if (!spamDetector.TryGetValue(ip, out var lastComment) ||
                (DateTime.Now - lastComment).TotalMinutes > 1)
            {
                spamDetector[ip] = DateTime.Now;
            }
            else
                return StatusCode(401);
        }
        
        await repository.AddComment(request.Username, request.Text);

        return Ok();
    }
}