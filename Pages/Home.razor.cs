﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.State;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Microsoft.JSInterop;
using PwdMngrWasm.Services;

namespace PwdMngrWasm.Pages
{
    public partial class Home
    {            
#pragma warning disable CS8618
        private string _searchText = string.Empty;
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IPasswordService PasswordService { get; set; }
        private List<PasswordEntry> _entries;
        private List<PasswordEntry>? _filteredEntries = null;
        private string? _userEmail = null;
        private string? _userName = null;
        private GetAllPasswordsDTO _getAllPasswordsDTO = new();
#pragma warning restore CS8618

        protected override async Task OnInitializedAsync()
        {
            await GetUser();
            _getAllPasswordsDTO.Email = _userEmail!;
            _entries = await PasswordService.GetPasswordEntriesAsync(_getAllPasswordsDTO);
            _filteredEntries = new(_entries);
        }

        private async Task OpenDialog(PasswordEntry entry)
        {
            var options = new DialogOptions() { CloseButton = true, CloseOnEscapeKey = true, Position = DialogPosition.Center };
            var parameters = new DialogParameters<PasswordEntry> { { "Entry", (object)entry } };

            var dialog = await Dialog.ShowAsync<PasswordEntryDialog>("Password", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                _entries = await PasswordService.GetPasswordEntriesAsync(_getAllPasswordsDTO);
                _searchText = string.Empty;
                FilterEntries();
                StateHasChanged();
            }
        }

        // Testing the navigation in production
        private void AddEntry() => NavigationManager.NavigateTo("/PW9/add", forceLoad: false);

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
                    entry.UserId.ToString().Contains(searchText) ||
                    entry.Url!.ToLower().Contains(searchText) ||
                    entry.Name!.ToLower().Contains(searchText) ||
                    entry.Note!.ToLower().Contains(searchText) ||
                    entry.Username!.ToLower().Contains(searchText) ||
                    entry.Password!.ToLower().Contains(searchText)
                ).ToList();
            }
        }

        private async Task<string> GetUser()
        {
            try
            {
                var authenticationState = await ((CustomAuthenticationStateProvider)authenticationStateProvider).GetAuthenticationStateAsync();
                var userClaims = authenticationState.User.Claims;
                var userEmail = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value ?? "";
                var userName = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value ?? "";
                _userEmail = userEmail;
                _userName = userName;
                return userEmail;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Method used in development to get hardcoded entries to simulate a database query.
        /// </summary>
        /// <param name="uniqueness"></param>
        /// <returns></returns>
        private static List<PasswordEntry> GetHardcodedEntries(string uniqueness)
        {
            var entries = new List<PasswordEntry>();

            for (int i = 1; i <= 20; i++)
            {
                entries.Add(new PasswordEntry
                {
                    EntryId = new Guid(),
                    UserId = new Guid(),
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
    }
}
