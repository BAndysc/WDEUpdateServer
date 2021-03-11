using System;
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
        public DateTime ReleaseDate { get; set; }
    }
}