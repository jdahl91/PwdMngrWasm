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
        public AuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
        public RegisterDTO RegisterForm = new();

        async Task RegisterClicked()
        {
            var response = await AuthenticationService.RegisterAsync(RegisterForm);

            if (!response)
            {
                await JS.InvokeVoidAsync("alert", "Unsuccessfull.");
                RegisterForm = new();
                return;
            }
            await JS.InvokeVoidAsync("alert", "Success.");
            RegisterForm = new();

            NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
