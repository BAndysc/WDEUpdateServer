using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class MarketplaceModel
    {
        [Key]
        public string Name { get; set; } = null!;
        public string? Key { get; set; } = null;
    }
}