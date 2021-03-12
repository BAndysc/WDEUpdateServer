namespace Server.Models.API
{
    public class UploadVersionRequest
    {
        public string Branch { get; set; }
        public string Marketplace { get; set; }
        public long Version { get; set; }
        public Platforms Platform { get; set; }
        public string VersionName { get; set; }
        public string User { get; set; }
        public string Key { get; set; }

        public UploadVersionRequest(string branch, string marketplace, long version, Platforms platform, string versionName, string user, string key)
        {
            Branch = branch;
            Marketplace = marketplace;
            Version = version;
            Platform = platform;
            VersionName = versionName;
            User = user;
            Key = key;
        }
    }
}