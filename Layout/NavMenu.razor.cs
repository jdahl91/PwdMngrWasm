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
        public AuthenticationService AuthenticationService { get; set; }
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

            if (success && uri.PathAndQuery == "/")
            {
                StateHasChanged();
            }
            else if (!success)
            {
                await JS.InvokeVoidAsync("alert", "Logout unsuccessful.");
            }
            else
            {
                // TODO : Make sure we navigate to the correct page, "/" was wrong since github pages is hosted in a subdirectory
                NavigationManager.NavigateTo("/", forceLoad: false);
            }
            //MainLayout.DrawerToggle(); 
            //NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
