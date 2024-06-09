using PwdMngrWasm.DTOs;

namespace PwdMngrWasm.Pages
{
    public partial class Register
    {
        public RegisterDTO RegisterForm = new();

        async Task RegisterClicked()
        {
            // RegistrationResponse response = await AccountService.RegisterAsync(Register);
            // if (!response.Flag)
            // {
            //     await JS.InvokeVoidAsync("alert", response.Message);
            //     Register = new();
            //     return;
            // }
            // await JS.InvokeVoidAsync("alert", response.Message);
            // Register = new();

            // NavigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}
