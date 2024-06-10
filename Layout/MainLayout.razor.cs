using Microsoft.AspNetCore.Components;
using MudBlazor;
using PwdMngrWasm.Pages;
using System.Net.NetworkInformation;

namespace PwdMngrWasm.Layout
{
    public partial class MainLayout
    {
        // trying to set these properties as public static for the navmenu to access
        public static bool _drawerOpen = false;

        // we can make the dark mode toggleable for the user
        public static bool _isDarkMode = true;

        public void DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;
            StateHasChanged();
        }

        public static void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private async Task OpenTopCornerDialog()
        {
            var options = new DialogOptions() { CloseButton = true, CloseOnEscapeKey = true, Position = DialogPosition.TopRight };
            // var parameters = new DialogParameters<PasswordEntry> { { "Entry", (object)entry } };

            var dialog = await Dialog.ShowAsync<TopRightDialog>(string.Empty, options); // parameters,
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                // _entries = GetHardcodedEntries("updated");
                // _searchText = string.Empty;
                // FilterEntries();
                // StateHasChanged();
            }
        }
    }
}
