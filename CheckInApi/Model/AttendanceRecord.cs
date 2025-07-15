using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckInApi.Model
{
    public class AttendanceRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserData User { get; set; }  // navigation
        [Required]
        public string Email { get; set; }


        [Required]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Required]
        public TimeSpan Time { get; set; } = DateTime.Now.TimeOfDay;

        [Required]
        public string Status { get; set; }  // Present/Absent
    }

}
