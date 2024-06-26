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
    // class that will act as the data access layer for the PasswordEntry class
    public class PasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        //private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

        public PasswordService(HttpClient httpClient, IJSRuntime jsRuntime) // , IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            //_authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        }

        public async Task<List<PasswordEntry>> GetPasswordEntriesAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return [];

            var jwt = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                      
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/get-all", email);

            var jsonString = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(jsonString);
            var passwordEntriesElement = document.RootElement.GetProperty("value").GetProperty("passwordEntries");
            var passwordEntries = new List<PasswordEntry>();

            foreach (var entryElement in passwordEntriesElement.EnumerateArray())
            {
                var entry = new PasswordEntry
                {
                    EntryId = entryElement.GetProperty("entryId").GetInt32(),
                    UserId = entryElement.GetProperty("userId").GetInt32(),
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

        // returns error 
        public async Task<bool> UpdatePasswordEntryAsync(PasswordEntry passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/update", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result?.Flag == true)
                return true;
            return false;
        }
    }
}

//var result = await JsonSerializer.DeserializeAsync<GetAllPasswordsApiResonse>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//using var responseStream = await response.Content.ReadAsStreamAsync();

//// Parse the JSON document
//var jsonDocument = await JsonDocument.ParseAsync(responseStream);

//// Deserialize the JSON document into the response model
//var getAllPasswordsResponse = JsonSerializer.Deserialize<GetAllPasswordsApiResponse>(jsonDocument.RootElement.GetRawText());

//// Access the deserialized data
//Console.WriteLine($"Flag: {getAllPasswordsResponse.Flag}");
//Console.WriteLine($"Message: {getAllPasswordsResponse.Message}");

//// Access each password entry
//foreach (var entry in getAllPasswordsResponse.PasswordEntries)
//{
//    Console.WriteLine($"EntryId: {entry.EntryId}, Name: {entry.Name}, URL: {entry.Url}");
//    // Access other properties as needed
//}

// *** NEW WAY OF DESERIALIZING JSON ***
//using var responseStream = await response.Content.ReadAsStreamAsync();
