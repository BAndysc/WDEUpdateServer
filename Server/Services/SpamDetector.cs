using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Services;

public interface ISpamDetector
{
    bool TryMakeRequest(IPAddress address);
}

public class SpamDetector : ISpamDetector
{
    private Dictionary<IPAddress, DateTime> spamDetector = new();
    
    public bool TryMakeRequest(IPAddress address)
    {
        lock (spamDetector)
        {
            if (!spamDetector.TryGetValue(address, out var lastComment) ||
                (DateTime.Now - lastComment).TotalMinutes > 1)
            {
                spamDetector[address] = DateTime.Now;
                return true;
            }
            else
                return false;
        }
    }
}