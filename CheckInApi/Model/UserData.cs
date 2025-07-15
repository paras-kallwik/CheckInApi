using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckInApi.Model
{
    public class UserData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ensures auto-increment
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Fname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lname { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
