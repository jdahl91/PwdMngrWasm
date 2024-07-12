using PwdMngrWasm.DTOs;

namespace PwdMngrWasm.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(LoginDTO form);
        Task<bool> LogoutAsync();
        Task<bool> RegisterAsync(RegisterDTO form);
        Task<bool> RefreshTokensAsync();

    }
}
