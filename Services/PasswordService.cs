using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PwdMngrWasm.Responses;
using PwdMngrWasm.State;
using System.Net.Http;
using PwdMngrWasm.DTOs;
using System.Net.Http.Json;
using static PwdMngrWasm.Responses.CustomResponses;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace PwdMngrWasm.Services
{
    public class PasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public PasswordService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<Guid> GetUserGuid()
        {
            var userGuidString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userGuid");

            if (!Guid.TryParseExact(userGuidString, "D", out Guid userGuid)) 
                return new Guid();
            else 
                return userGuid;
        }

        public async Task<List<PasswordEntry>> GetPasswordEntriesAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return [];

            var passwordEntries = new List<PasswordEntry>();
            try
            {
                var jwt = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/get-all", email);

                var jsonString = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(jsonString);
                var passwordEntriesElement = document.RootElement.GetProperty("value").GetProperty("passwordEntries");

                foreach (var entryElement in passwordEntriesElement.EnumerateArray())
                {
                    var entry = new PasswordEntry
                    {
                        EntryId = entryElement.GetProperty("entryId").GetGuid(),
                        UserId = entryElement.GetProperty("userId").GetGuid(),
                        Url = entryElement.GetProperty("url").GetString(),
                        Name = entryElement.GetProperty("name").GetString(),
                        Note = entryElement.GetProperty("note").GetString(),
                        Username = entryElement.GetProperty("username").GetString(),
                        Password = entryElement.GetProperty("password").GetString(),
                        CreatedAt = entryElement.GetProperty("createdAt").GetDateTime(),
                        UpdatedAt = entryElement.GetProperty("updatedAt").GetDateTime()
                    };
                    passwordEntries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return passwordEntries;
        }

        public async Task<bool> InsertPasswordEntryAsync(NewPasswordEntryDTO passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/insert", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result?.Flag == true)
                return true;
            return false;
        }

        public async Task<bool> UpdatePasswordEntryAsync(UpdatePasswordEntryDTO passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/update", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result?.Flag == true)
                return true;
            return false;
        }
    }
}

