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
        public string? HashMd5 { get; set; } = "";
        public DateTime UploadDate { get; set; }
        public UserModel Uploader { get; set; } = null!;

        public List<VersionFilesModel> ReferencedVersions { get; set; } = null!;
    }
}