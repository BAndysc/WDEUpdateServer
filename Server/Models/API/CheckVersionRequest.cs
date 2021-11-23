using System;

namespace Server.Models.API
{
    public class CheckVersionRequest
    {
        public long CurrentVersion { get; }
        public string Marketplace { get; }
        public string Branch { get; }
        public Platforms Platform { get; }
        public string? Key { get; }
        public PlatformID? OsPlatformId { get; }
        public int? OsMajorVersion { get; }
        public int? OsMinorVersion { get; }

        public CheckVersionRequest(long currentVersion, string marketplace, string branch, Platforms platform, string? key, PlatformID? osPlatformId, int? osMajorVersion, int? osMinorVersion)
        {
            CurrentVersion = currentVersion;
            Marketplace = marketplace;
            Branch = branch;
            Platform = platform;
            Key = key;
            OsPlatformId = osPlatformId;
            OsMajorVersion = osMajorVersion;
            OsMinorVersion = osMinorVersion;
        }
    }
}