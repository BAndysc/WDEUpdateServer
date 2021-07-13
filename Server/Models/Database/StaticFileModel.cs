using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class StaticFileModel
    {
        [Key]
        [Required]
        public uint Key { get; set; }
        public string FileName { get; set; } = "";
        public string Path { get; set; } = "";
    }
}