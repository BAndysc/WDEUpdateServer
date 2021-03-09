using System;

namespace Server.Models
{
    public class ChangeLogEntry
    {
        public long Version { get; set; }
        public string VersionName { get; set; }
        public DateTime Date { get; set; }
        public string? UpdateTitle { get; set; }
        public string[] Changes { get; set; }

        public ChangeLogEntry(long version, string? versionName, DateTime date, string? updateTitle, params string[] changes)
        {
            Version = version;
            VersionName = versionName ?? $"{(version / 10000000) % 1000}.{(version / 1000) % 1000}.{version % 1000}";
            Date = date;
            UpdateTitle = updateTitle;
            Changes = changes;
        }
    }
}