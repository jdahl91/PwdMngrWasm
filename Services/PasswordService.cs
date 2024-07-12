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
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly string _baseAddress = "https://834627.xyz/api/password";

        public PasswordService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<PasswordEntry>> GetPasswordEntriesAsync(GetAllPasswordsDTO form)
        {
            if (string.IsNullOrEmpty(form.Email))
                return [];

            var passwordEntries = new List<PasswordEntry>();
            try
            {
                var jwt = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                form.UserId = await GetUserGuid();

                var response = await _httpClient.PostAsJsonAsync(_baseAddress + "/get-all", form);

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
            passwordEntry.UserId = await GetUserGuid();
            var response = await _httpClient.PostAsJsonAsync(_baseAddress + "/insert", passwordEntry);           
            var jsonString = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(jsonString);
            var flag = document.RootElement.GetProperty("value").GetProperty("flag").GetBoolean();

            if (flag == true)
                return true;
            return false;
        }

        public async Task<bool> UpdatePasswordEntryAsync(UpdatePasswordEntryDTO passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseAddress + "/update", passwordEntry);
            var jsonString = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(jsonString);
            var flag = document.RootElement.GetProperty("value").GetProperty("flag").GetBoolean();

            if (flag == true)
                return true;
            return false;
        }

        private async Task<Guid> GetUserGuid()
        {
            var userGuidString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userGuid");

            if (!Guid.TryParseExact(userGuidString, "D", out Guid userGuid))
                return new Guid();
            else
                return userGuid;
        }
    }
}

