using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PwdMngrWasm.Responses;
using PwdMngrWasm.State;
using System.Net.Http;
using PwdMngrWasm.DTOs;
using System.Net.Http.Json;

namespace PwdMngrWasm.Services
{
    // class that will act as the data access layer for the PasswordEntry class
    public class PasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
        
        public PasswordService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        }

        // TODO
        // method to get all the passwords for the user
        public async Task<List<PasswordEntry>> GetPasswordEntriesAsync(string email)
        {
            // the SQL query will be something like "SELECT * FROM Passwords WHERE UserId = (SELECT UserId FROM Users WHERE Email = email)" on the server
            var response = await _httpClient.PostAsJsonAsync("https://example.com/api/pwd/get-all", email);
            var result = await response.Content.ReadFromJsonAsync<List<PasswordEntry>>();
            return result!;
        }

        // TODO
        // method to create a new password entry
        public async Task<bool> InsertPasswordEntryAsync(PasswordEntry passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://example.com/api/pwd/insert-pwd", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result!;
        }

        // TODO
        // method to update a password entry
        public async Task<bool> UpdatePasswordEntryAsync(PasswordEntry passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://example.com/api/pwd/update", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result!;
        }

        // TODO ?? may not be neeeded
        // method to get a single password entry by id
        //public async Task<bool> GetPasswordEntryAsync(string entryId)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("https://example.com/api/pwd/get-one", entryId);
        //    var result = await response.Content.ReadFromJsonAsync<bool>();
        //    return result!;
        //}
    }
}
