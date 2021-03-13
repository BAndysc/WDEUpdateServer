using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class ChangeLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public string Change { get; set; } = null!;

        public VersionEntityModel Version { get; set; } = null!;
    }
}