using System.ComponentModel.DataAnnotations;

namespace PwdMngrWasm.DTOs
{
    public class GetAllPasswordsDTO
    {
        [Required, DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}
