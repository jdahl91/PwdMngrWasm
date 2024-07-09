using Microsoft.AspNetCore.Components;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.Services;

namespace PwdMngrWasm.Pages
{
    public partial class Add
    {
        [Inject]
#pragma warning disable CS8618
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public PasswordService PasswordService { get; set; }
#pragma warning restore CS8618
        private NewPasswordEntryDTO _newPasswordEntry = new();
        private bool _addPasswordFailed = false;
        private Guid _userGuid;

        protected override async Task OnInitializedAsync()
        {
            _userGuid = await PasswordService.GetUserGuid();
        }
        private async Task AddPasswordEntry()
        {
            _newPasswordEntry.UserId = _userGuid;

            // success is not a boolean, it needsto be a response object from the server which we deserialize
            // insertpasswordentryasync works 
            var success = await PasswordService.InsertPasswordEntryAsync(_newPasswordEntry);

            if (success)
            {
                NavigationManager.NavigateTo("/", forceLoad: false);
            }
            else
            {
                _addPasswordFailed = true;
                //_newPasswordEntry = new();
            }
        }
    }
}
