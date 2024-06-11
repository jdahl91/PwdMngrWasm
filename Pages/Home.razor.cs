﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.State;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace PwdMngrWasm.Pages
{
    public partial class Home
    {            
#pragma warning disable CS8618
        private string _searchText = string.Empty;
        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        private List<PasswordEntry> _entries;
        private List<PasswordEntry> _filteredEntries;
        private string _userEmail;
#pragma warning restore CS8618

        protected override async Task OnInitializedAsync()
        {
            _entries = GetHardcodedEntries("original"); // await PasswordService.GetEntriesFromDatabase(email);
            _filteredEntries = new(_entries);
        }

        private async Task OpenDialog(PasswordEntry entry)
        {
            var options = new DialogOptions() { CloseButton = true, CloseOnEscapeKey = true, Position = DialogPosition.Center };
            var parameters = new DialogParameters<PasswordEntry> { { "Entry", (object)entry } };

            var dialog = await Dialog.ShowAsync<PasswordEntryDialog>("Password", parameters, options);
            var result = await dialog.Result; // GetReturnValueAsync<PasswordEntry>();
                                              // entry = result; // in my mind this inserts the object back into the list of entries, but needs testing

            if (!result.Canceled)
            {
                // var userClaims = ((CustomAuthenticationStateProvider)authenticationStateProvider).GetAuthenticationStateAsync().Result.User.Claims;
                // _userEmail = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value ?? ""; // We can revisit this later, but I think it works, it will be null if the claim is not found
                // await fetch new data from the database here and update the list of entries
                // _entries = await PasswordService.GetEntriesFromDatabase(_userEmail);
                _entries = GetHardcodedEntries("updated"); // await PasswordService.GetEntriesFromDatabase(_userEmail);
                _searchText = string.Empty;
                FilterEntries();
                StateHasChanged(); // the database should be updated here from the dialog so we have one point of truth
            }
            // Else the dialog was canceled

            // this is where we should update the database with the new entry
            // we should have an update method which should pass the updated entry back to the parent component
        }

        // method to get the hardcoded entries to simulate a database
        private static List<PasswordEntry> GetHardcodedEntries(string uniqueness)
        {
            var entries = new List<PasswordEntry>();

            for (int i = 1; i <= 20; i++)
            {
                entries.Add(new PasswordEntry
                {
                    EntryId = i,
                    UserId = 1,
                    Url = $"https://{uniqueness}.com/{i}",
                    Name = $"{uniqueness}Example {i}",
                    Note = $"This is an {uniqueness}example note for entry {i}.",
                    Username = $"user{i}{uniqueness}",
                    Password = $"password{i}{uniqueness}",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            return entries;
        }

        private void FilterEntries()
        {
            var searchText = _searchText.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredEntries = _entries;
            }
            else
            {
                _filteredEntries = _entries.Where(entry =>
                    //entry.EntryId.ToString().Contains(searchText) ||
                    //entry.UserId.ToString().Contains(searchText) ||
                    entry.Url!.ToLower().Contains(searchText) ||
                    entry.Name!.ToLower().Contains(searchText) // ||
                    //entry.Note!.ToLower().Contains(searchText) ||
                    //entry.Username!.ToLower().Contains(searchText) ||
                    //entry.Password!.ToLower().Contains(searchText) ||
                    //entry.CreatedAt.ToString().Contains(searchText) ||
                    //entry.UpdatedAt.ToString().Contains(searchText)
                ).ToList();
            }
        }

        //private async Task GetUserTest()
        //{
        //    try
        //    {
        //        var authenticationState = await ((CustomAuthenticationStateProvider)authenticationStateProvider).GetAuthenticationStateAsync();
        //        var userClaims = authenticationState.User.Claims;
        //        _userEmail = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value ?? "";
        //        _testString = string.IsNullOrEmpty(_userEmail) ? "No email found." : _userEmail;
        //    }
        //    catch (Exception ex)
        //    {
        //        _testString = ex.Message;
        //    }
        //}
    }
}
