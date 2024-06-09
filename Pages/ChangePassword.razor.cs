using MudBlazor;
using PwdMngrWasm.DTOs;
using System.Security.Claims;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Components;

namespace PwdMngrWasm.Pages
{
    public partial class ChangePassword
    {
        [Inject]
#pragma warning disable CS8618
        public HttpClient HttpClient { get; set; }
#pragma warning restore CS8618
        private ChangePwdDTO changePasswordModel = new();
        // private CustomUserClaims? UserClaims = null;
        private bool success = false;
        private bool attempted = false;

        protected override void OnInitialized()
        {
            // UserClaims = DecryptJwtService.DecryptToken(Constants.JwtToken);
            // changePasswordModel = new() { Email = UserClaims.Email };
        }

        private async Task ChangePasswordAsync()
        {
            // RegistrationResponse result;
            // result = await Account.ChangePassword(changePasswordModel!);
            // if (!result.Flag)
            // {
            //     await JS.InvokeVoidAsync("alert", result.Message);
            //     changePasswordModel!.CurrentPassword = string.Empty;
            //     changePasswordModel!.NewPassword = string.Empty;
            //     changePasswordModel!.ConfirmNewPassword = string.Empty;
            //     return;
            // }
            // _ = result.Flag ? success = true : attempted = true;
        }
    }
}
