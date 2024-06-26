using PwdMngrWasm.DTOs;

namespace PwdMngrWasm.Responses
{
    public class CustomResponses
    {
        public record RegistrationResponse(bool Flag = false, string Message = null!);
        public record LoginResponse(bool Flag = false, string Message = null!, string JwtToken = null!, string RefreshToken = null!);
        public record ApiResponse(bool Flag = false, string Message = null!, object payload = null!);
        public record GetAllPasswordsApiResponse(bool Flag = false, string Message = null!, List<PasswordEntry> PasswordEntries = null!);

    }
}
