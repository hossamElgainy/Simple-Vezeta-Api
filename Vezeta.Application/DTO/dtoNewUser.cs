using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

using VezetaCore.ENum;

namespace Vezeta.Application.DTO
{
    public class dtoNewUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        public IFormFile Image { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
