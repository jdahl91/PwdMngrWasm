using Microsoft.AspNetCore.Components;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.Services;

namespace PwdMngrWasm.Pages
{
    public partial class Add
    {
        [Inject]
#pragma warning disable CS8618
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IPasswordService PasswordService { get; set; }
#pragma warning restore CS8618
        private NewPasswordEntryDTO _newPasswordEntry = new();
        private bool _addPasswordFailed = false;

        private async Task AddPasswordEntry()
        {
            if (string.IsNullOrWhiteSpace(_newPasswordEntry.Name) || string.IsNullOrWhiteSpace(_newPasswordEntry.Password))
            {
                _addPasswordFailed = true;
                _ = ResetWarning();
                return;
            }
            var success = await PasswordService.InsertPasswordEntryAsync(_newPasswordEntry);

            if (success)
            {
                // This should naviagte home, testing with PW9
                NavigationManager.NavigateTo("/PW9", forceLoad: false);
            }
            else
            {
                _addPasswordFailed = true;
                _ = ResetWarning();
            }
        }

        private async Task ResetWarning()
        {
            await Task.Delay(5000);
            _addPasswordFailed = false;
            StateHasChanged();
        }

        private void Return() => NavigationManager.NavigateTo("/PW9", forceLoad: false);
    }
}
