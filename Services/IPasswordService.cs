using PwdMngrWasm.DTOs;

namespace PwdMngrWasm.Services
{
    public interface IPasswordService
    {
        Task<List<PasswordEntry>> GetPasswordEntriesAsync(GetAllPasswordsDTO form);
        Task<bool> InsertPasswordEntryAsync(NewPasswordEntryDTO passwordEntry);
        Task<bool> UpdatePasswordEntryAsync(UpdatePasswordEntryDTO passwordEntry);
    }
}
