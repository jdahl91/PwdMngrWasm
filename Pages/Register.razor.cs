using PwdMngrWasm.DTOs;
using static PwdMngrWasm.Responses.CustomResponses;
using PwdMngrWasm.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PwdMngrWasm.Pages
{
    public partial class Register
    {
        [Inject]
#pragma warning disable CS8618
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
#pragma warning restore CS8618
        public RegisterDTO RegisterForm = new();
        private bool _registrationFailed = false;

        private async Task RegisterClicked()
        {
            var success = await AuthenticationService.RegisterAsync(RegisterForm);

            if (!success)
            {
                _registrationFailed = true;
                _ = ResetWarning();
                RegisterForm = new();
                return;
            }
            NavigationManager.NavigateTo("/PW9", forceLoad: false);
        }

        private async Task ResetWarning()
        {
            await Task.Delay(5000);
            _registrationFailed = false;
            StateHasChanged();
        }
    }
}
