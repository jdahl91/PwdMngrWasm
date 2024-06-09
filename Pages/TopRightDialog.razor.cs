using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PwdMngrWasm.Pages
{
    public partial class TopRightDialog
    {
        [CascadingParameter]
#pragma warning disable CS8618
        public MudDialogInstance MudDialog { get; set; }
#pragma warning restore CS8618
        private string _dialogContent = "This is a top right dialog.";

        private void SubmitTRC() => MudDialog.Close(DialogResult.Ok(true));
        private void CancelTRC() => MudDialog.Cancel();
    }
}
