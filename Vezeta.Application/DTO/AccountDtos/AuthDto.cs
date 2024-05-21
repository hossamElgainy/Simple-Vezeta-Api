

namespace Vezeta.Application.DTO.AccountDtos
{
    public class AuthDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
