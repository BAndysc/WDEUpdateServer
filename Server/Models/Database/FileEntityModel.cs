using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class FileEntityModel
    {
        [Key]
        [Required]
        public Guid Key { get; set; }
        public string Path { get; set; } = "";

        public List<VersionFilesModel> ReferencedVersions { get; set; } = null!;
    }
}