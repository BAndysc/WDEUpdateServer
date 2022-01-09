using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database;

public class LogModel
{
    public LogModel(string? ip, string method, string text, string userAgent)
    {
        Key = Guid.NewGuid();
        Ip = ip;
        Method = method;
        Text = text;
        UserAgent = userAgent;
        Date = DateTime.Now;

        if (userAgent.StartsWith("WoWDatabaseEditor/"))
        {
            int space = userAgent.IndexOf(' ');
            if (space > 0)
            {
                string version = userAgent.Substring("WoWDatabaseEditor/".Length, space - "WoWDatabaseEditor/".Length);
                if (int.TryParse(version, out var intVersion))
                    Version = intVersion;
            }
        }

        var parts = userAgent.Split(",");

        foreach (var part in parts)
        {
            var keyValue = part.Split(':');

            if (keyValue.Length != 2)
                continue;

            var key = keyValue[0].Trim();
            var value = keyValue[1].Trim();
            if (value.EndsWith(')'))
                value = value.Substring(0, value.Length - 1);

            if (key.IndexOf('(') != -1)
                key = key.Substring(key.IndexOf('(') + 1).Trim();

            key = key.ToLower();

            if (key == "branch")
                Branch = value;
            else if (key == "run" && int.TryParse(value, out var intRun))
                Run = intRun;
            else if (key == "laststart" && DateTime.TryParse(value, out var date))
                LastStart = date;
            else if (key == "core")
                Core = value;
            else if (key == "theme")
                Theme = value;
            else if (key == "marketplace")
                Marketplace = value;
            else if (key == "platform")
                Platform = value;
            else if (key == "os")
                OS = value;
            else if (key == "dbc")
                Dbc = value == "Yes";
            else if (key == "db")
                Db = value == "Yes";
            else if (key == "soap")
                Soap = value == "Yes";
        }
    }

    [Key]
    public Guid Key { get; set; } 

    public string? Ip { get; set; }
    
    public string Method { get; set; }
    
    public string Text { get; set; }
    
    public DateTime Date { get; set; }
    
    public string? Branch { get; set; }
    
    public int? Version { get; set; }
    
    public int? Run { get; set; }

    public DateTime? LastStart { get; set; }
    
    public string? Marketplace { get; set; }
    
    public string? Core { get; set; }
    
    public string? Theme { get; set; }
    
    public string? Platform { get; set; }
    
    public string? OS { get; set; }
    
    public bool? Dbc { get; set; }
    
    public bool? Db { get; set; }
    
    public bool? Soap { get; set; }
    
    public string UserAgent { get; set; }

}