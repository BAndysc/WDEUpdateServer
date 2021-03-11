using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Database
{
    [Table("versions")]
    public class FileEntityModel
    {
        [Key]
        [Required]
        public Guid Key { get; set; }
        public string Path { get; set; } = "";
    }
}