using System;

namespace Server.Models
{
    public class CheckVersionResponse
    {
        public CheckVersionResponse(long version, string downloadUrl, params ChangeLogEntry[] changeLog)
        {
            ChangeLog = changeLog;
            LatestVersion = version;
            DownloadUrl = downloadUrl;
        }

        public long LatestVersion { get; set; }
        public string DownloadUrl { get; set; }
        public ChangeLogEntry[] ChangeLog { get; set; }
    }

    public enum Platforms
    {
        Windows,
        MacOs,
        Linux,
        Universal
    }

    public class UploadVersionRequest
    {
        public string Branch { get; set; } = "";
        public string Marketplace { get; set; } = "default";
        public long Version { get; set; }
        public Platforms Platform { get; set; }
        public string VersionName { get; set; } = "";
        public string Key { get; set; } = "";
    }

    public class UploadVersionResponse
    {
        public Guid Id { get; set; }
    }
}