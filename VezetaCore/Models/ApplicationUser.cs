using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using VezetaCore.ENum;

namespace VezetaCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public byte[] Image { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Booking> PatientRequests { get; set; } = new HashSet<Booking>();

    }
}