using System;
using System.ComponentModel.DataAnnotations;
using Server.Models.API;

namespace Server.Models.Database
{
    public class VersionFilesModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public VersionEntityModel Version { get; set; } = null!;
        
        [Required]
        public Platforms Platform { get; set; }
        
        [Required]
        public FileEntityModel File { get; set; } = null!;
    }
}