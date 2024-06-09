using Microsoft.AspNetCore.Components;
using MudBlazor;
using PwdMngrWasm.DTOs;

namespace PwdMngrWasm.Pages
{
    public partial class PasswordEntryDialog
    {
        [CascadingParameter]
#pragma warning disable 8618
        public MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public PasswordEntry Entry { get; set; }
        private PasswordEntry _localEntry;
#pragma warning restore 8618

        protected override void OnParametersSet()
        {
            _localEntry = new PasswordEntry(Entry);
        }

        private void Submit()
        {
            // TODO 
            // save the entry to the database!

            // await PasswordService.UpdatePasswordEntryAsync(_localEntry);
            MudDialog.Close(DialogResult.Ok(true));
        }
        private void Cancel() => MudDialog.Cancel();
    }
}
