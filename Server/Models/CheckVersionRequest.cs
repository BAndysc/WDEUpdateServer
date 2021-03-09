namespace Server.Models
{
    public class CheckVersionRequest
    {
        public long CurrentVersion { get; set; }
        public string? Marketplace { get; set; }
        public string? Branch { get; set; }
        public string? Flavour { get; set; }
        public string? Key { get; set; }
    }
}