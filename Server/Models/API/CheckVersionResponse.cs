namespace Server.Models.API
{
    public class CheckVersionResponse
    {
        public CheckVersionResponse(long version, string? downloadUrl, string? hash, params ChangeLogEntry[] changeLog)
        {
            ChangeLog = changeLog;
            DownloadMd5 = hash;
            LatestVersion = version;
            DownloadUrl = downloadUrl;
        }

        public long LatestVersion { get; set; }
        public string? DownloadUrl { get; set; }
        public string? DownloadMd5 { get; set; }
        public ChangeLogEntry[] ChangeLog { get; set; }
    }
}