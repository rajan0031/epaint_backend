using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDotNetApp.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("password")]
        public string Password { get; set; } = "";

        [Column("confirm_password")]
        public string ConfirmPassword { get; set; } = "";

        [Column("contact_number")]
        public string ContactNumber { get; set; } = "";

        [Column("address")]
        public string Address { get; set; } = "";

        [Column("pin_code")]
        public string PinCode { get; set; } = "";

        [Column("profile_image")]
        public string ProfileImage { get; set; } = "";
    }
}
