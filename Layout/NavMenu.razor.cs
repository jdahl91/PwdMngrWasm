using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using PwdMngrWasm.Services;
using PwdMngrWasm.State;

namespace PwdMngrWasm.Layout
{
    public partial class NavMenu
    {
#pragma warning disable CS8618
        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        public AuthenticationService authenticationService { get; set; }
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
            var customAuthenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
            await customAuthenticationStateProvider.MarkUserAsLoggedOut();

            // TODO:
            // there should also be a call to the server to invalidate the refresh token
            // await authenticationService.LogoutAsync();

            MainLayout.DrawerToggle(); // if the drawer is open, close it on small screens, not desktop
            navigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
