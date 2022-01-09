namespace Server.Models.API;

public class AddCommentRequest
{
    public string Username { get; set; }
    public string Text { get; set; }

    public AddCommentRequest(string username, string text)
    {
        Username = username;
        Text = text;
    }
}