namespace Server.Models.API
{
    public class UploadVersionRequest
    {
        public string Branch { get; set; }
        public string Marketplace { get; set; }
        public long Version { get; set; }
        public Platforms Platform { get; set; }
        public string VersionName { get; set; }
        public string Key { get; set; }

        public UploadVersionRequest(string branch, string marketplace, long version, Platforms platform, string versionName, string key)
        {
            Branch = branch;
            Marketplace = marketplace;
            Version = version;
            Platform = platform;
            VersionName = versionName;
            Key = key;
        }
    }
}