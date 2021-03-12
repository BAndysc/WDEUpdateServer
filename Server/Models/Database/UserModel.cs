using System.ComponentModel.DataAnnotations;

namespace Server.Models.Database
{
    public class UserModel
    {
        [Key]
        public string User { get; set; } = null!;
        public string KeyHash { get; set; } = null!;
    }
}