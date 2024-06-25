using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using PwdMngrWasm.DTOs;
using static PwdMngrWasm.Responses.CustomResponses;
using PwdMngrWasm.Services;
using PwdMngrWasm.State;
using Microsoft.JSInterop;

namespace PwdMngrWasm.Pages
{
    public partial class Login
    {
#pragma warning disable CS8618
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationService AuthenticationService { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
#pragma warning restore CS8618
        public LoginDTO LoginForm = new();
        private bool _loginFailed = false;

        private async Task LoginClicked()
        {
            var success = await AuthenticationService.LoginAsync(LoginForm);

            if (!success)
            {
                _loginFailed = true;
                LoginForm = new();
                return;
            }
            NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
