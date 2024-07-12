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
        public IPasswordService PasswordService { get; set; }
        private UpdatePasswordEntryDTO _localEntry;
#pragma warning restore 8618

        protected override void OnParametersSet()
        {
            _localEntry = new UpdatePasswordEntryDTO(Entry);
        }

        private async Task Submit()
        {
            var success = await PasswordService.UpdatePasswordEntryAsync(_localEntry);
            if (!success) { Cancel(); }
            MudDialog.Close(DialogResult.Ok(true));
        }
        private void Cancel() => MudDialog.Cancel();
    }
}
