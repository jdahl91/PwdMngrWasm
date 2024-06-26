using Microsoft.AspNetCore.Components;
using MudBlazor;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.Services;

namespace PwdMngrWasm.Pages
{
    public partial class PasswordEntryDialog
    {
        [CascadingParameter]
#pragma warning disable 8618
        public MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public PasswordEntry Entry { get; set; }
        [Inject]
        public PasswordService PasswordService { get; set; }
        private PasswordEntry _localEntry;
#pragma warning restore 8618

        protected override void OnParametersSet()
        {
            _localEntry = new PasswordEntry(Entry);
        }

        private async Task Submit()
        {
            var success = await PasswordService.UpdatePasswordEntryAsync(_localEntry);
            if (!success)
            {
                System.Diagnostics.Debug.WriteLine("Failed to update password entry");
                Console.WriteLine("Failed to update password entry");
                Cancel();
            }
            System.Diagnostics.Debug.WriteLine("Success password entry update!");
            Console.WriteLine("Success password entry update!");
            MudDialog.Close(DialogResult.Ok(true));
        }
        private void Cancel() => MudDialog.Cancel();
    }
}
