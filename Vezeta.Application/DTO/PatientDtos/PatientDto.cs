using VezetaCore.ENum;

namespace Vezeta.Application.DTO.PatientDtos
{
    public class PatientDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Image { get; set; }

    }
}
