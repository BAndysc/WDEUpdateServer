namespace Server.Models.API
{
    public class UploadVersionRequest
    {
        public VersionKey Version { get; set; }
        public Authentication User { get; set; }
        public Platforms Platform { get; set; }
        public string VersionName { get; set; }

        public UploadVersionRequest(VersionKey version, Authentication user, Platforms platform, string versionName)
        {
            Version = version;
            Platform = platform;
            VersionName = versionName;
            User = user;
        }
    }

    public class Authentication
    {
        public Authentication(string user, string key)
        {
            User = user;
            Key = key;
        }

        public string User { get; set; }
        public string Key { get; set; }
    }

    public class VersionKey
    {
        public VersionKey(string branch, string marketplace, long version)
        {
            Branch = branch;
            Marketplace = marketplace;
            Version = version;
        }

        public string Branch { get; set; }
        public string Marketplace { get; set; }
        public long Version { get; set; }
    }
    
    public class AddChangelogEntryRequest
    {
        public VersionKey Version { get; set; }
        public Authentication User { get; set; }
        public string Entry { get; set; }

        public AddChangelogEntryRequest(VersionKey version, Authentication user, string entry)
        {
            Version = version;
            User = user;
            Entry = entry;
        }
    }
}