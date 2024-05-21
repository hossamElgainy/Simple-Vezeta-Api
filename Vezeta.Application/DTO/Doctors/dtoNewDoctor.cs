
using System.ComponentModel.DataAnnotations;


namespace Vezeta.Application.DTO.Doctors
{
    public class dtoNewDoctor : dtoNewUser
    {
        [Required]

        public int SpecializationId { get; set; }
    }
}
