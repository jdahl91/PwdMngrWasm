using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PwdMngrWasm.Services;
using PwdMngrWasm.State;

namespace PwdMngrWasm.Layout
{
    public partial class NavMenu
    {
#pragma warning disable CS8618
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }
        [Parameter]
        public EventCallback ToggleMethodReference { get; set; }
#pragma warning restore CS8618

        private async Task ToggleDarkMode()
        {
            if (ToggleMethodReference.HasDelegate)
                await ToggleMethodReference.InvokeAsync(null);
        }

        private async Task LogoutClicked()
        {
            var success = await AuthenticationService.LogoutAsync();
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            if (success && uri.PathAndQuery == "/PW9")
            {
                StateHasChanged();
            }
            else if (!success)
            {
                await JS.InvokeVoidAsync("alert", "Logout unsuccessful.");
            }
            else
            {
                NavigationManager.NavigateTo("/PW9", forceLoad: false);
            }
        }
    }
}
