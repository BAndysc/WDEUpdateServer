using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database;

public class CommentModel
{
    public CommentModel(Guid key, string username, string text, string userAgent, DateTime date)
    {
        Username = username;
        Text = text;
        Key = key;
        Date = date;
        UserAgent = userAgent;
    }

    [Key]
    [Required]
    public Guid Key { get; set; }
    
    public string Username { get; set; }
    
    public string UserAgent { get; set; }
    
    public string Text { get; set; }
    
    public DateTime Date { get; set; }
    
    public bool Approved { get; set; }
}