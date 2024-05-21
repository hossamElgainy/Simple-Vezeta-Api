
using System.ComponentModel.DataAnnotations;


namespace Vezeta.Application.DTO.AccountDtos
{
    public class dtoLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
