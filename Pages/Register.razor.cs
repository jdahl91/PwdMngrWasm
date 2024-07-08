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
        public AuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
#pragma warning restore CS8618
        public RegisterDTO RegisterForm = new();
        private bool _registrationFailed = false;

        async Task RegisterClicked()
        {
            var success = await AuthenticationService.RegisterAsync(RegisterForm);

            if (!success)
            {
                _registrationFailed = true;
                RegisterForm = new();
                return;
            }
            // TODO : Make sure we navigate to the correct page, "/" was wrong since github pages is hosted in a subdirectory
            NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
