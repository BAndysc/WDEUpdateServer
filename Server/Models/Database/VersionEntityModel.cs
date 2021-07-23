using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class VersionEntityModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Marketplace { get; set; } = "";
        public string Branch { get; set; } = "";
        public long Version { get; set; }
        public string TextVersion { get; set; } = "";
        public string? UpdateTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ChangeLogEntryModel> Changes { get; set; } = null!;

        public List<VersionFilesModel> Files { get; set; } = null!;
    }
}