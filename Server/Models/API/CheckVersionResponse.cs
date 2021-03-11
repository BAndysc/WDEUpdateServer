namespace Server.Models.API
{
    public class CheckVersionResponse
    {
        public CheckVersionResponse(long version, string? downloadUrl, params ChangeLogEntry[] changeLog)
        {
            ChangeLog = changeLog;
            LatestVersion = version;
            DownloadUrl = downloadUrl;
        }

        public long LatestVersion { get; set; }
        public string? DownloadUrl { get; set; }
        public ChangeLogEntry[] ChangeLog { get; set; }
    }
}