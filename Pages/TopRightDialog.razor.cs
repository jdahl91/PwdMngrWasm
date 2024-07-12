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
        private string _dialogContent = "This browser client was developed in Blazor WebAssembly. The back-end web server runs an ASP.NET Core API behind Nginx as a reverse proxy, the datastore used is PostgreSQL.";

        //private void SubmitTRC() => MudDialog.Close(DialogResult.Ok(true));
        //private void CancelTRC() => MudDialog.Cancel();
        private void Submit() => MudDialog.Cancel();
    }
}
