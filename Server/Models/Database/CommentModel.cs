using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database;

public class CommentModel
{
    public CommentModel(string username, string text)
    {
        Username = username;
        Text = text;
    }

    [Key]
    [Required]
    public Guid Key { get; set; }
    
    public string Username { get; set; }
    
    public string Text { get; set; }
}