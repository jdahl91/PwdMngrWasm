using Microsoft.AspNetCore.Components;

namespace PwdMngrWasm.Pages
{
    public partial class ConfirmEmail
    {
        protected override async Task OnInitializedAsync()
        {
            // var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            // var emailParam = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("email", out var email);
            // var tokenParam = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("confirmToken", out var token);

            // await AccountService.ConfirmEmailAddress(email!, token!);
        }

        void GoHome()
        {
            // NavigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}
