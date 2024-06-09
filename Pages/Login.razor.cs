using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.Services;
using PwdMngrWasm.State;

namespace PwdMngrWasm.Pages
{
    public partial class Login
    {
        [Inject]
#pragma warning disable CS8618
        public AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationService authenticationService { get; set; }
#pragma warning restore CS8618
        public LoginDTO LoginForm = new();

        async Task LoginClicked()
        {
            // LoginResponse response = await AccountService.LoginAsync(Login);
            // if (!response.Flag)
            // {
            //     await JS.InvokeVoidAsync("alert", response.Message);
            //     Login = new();
            //     return;
            // }
            var customAuthenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;

            // TODO : Implement the login logic here
            var loginResponse = await authenticationService.Login(LoginForm);
            var jwt = loginResponse.JwtToken;
            var refreshToken = loginResponse.RefreshToken;

            // await customAuthenticationStateProvider.MarkUserAsAuthenticated(jwt, refreshToken);
            // TODO?
            // NEED TO HANDLE THE REFRESH TOKEN maybe done above

            // JWT Hardcoded for testing
            // this happens here only in dev, we should make the call to MarkUserAsAuthenticated in the AuthenticationService.Login method
            await customAuthenticationStateProvider.MarkUserAsAuthenticated("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiSm9ha2ltIiwiRW1haWwiOiJqb2FraW1kYWhsQGdteC51cyIsIlJvbGUiOiJBZG1pbiJ9.xwehf7zL11t0lHZhSynNmeQsYZghMdDFoAWNPrYcfhM", "new-v9-refresh-token-test");

            NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }
}
