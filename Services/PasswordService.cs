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
            Console.WriteLine("Entered get pwd method");
            System.Diagnostics.Debug.WriteLine("Entered get pwd method");
            if (string.IsNullOrEmpty(email))
                return [];

            Console.WriteLine(email);
            System.Diagnostics.Debug.WriteLine(email);
            var jwt = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            Console.WriteLine(jwt);
            System.Diagnostics.Debug.WriteLine(jwt);

            var header = new AuthenticationHeaderValue("Bearer", jwt);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt); // new AuthenticationHeaderValue("Bearer", jwt);
            Console.WriteLine(header.ToString());
            System.Diagnostics.Debug.WriteLine(header.ToString());

            Console.WriteLine("About to call api");
            System.Diagnostics.Debug.WriteLine("About to call api");


            //var stringContent = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/get-all", email);

            //var response = await _httpClient.PostAsync("https://834627.xyz/api/password/get-all", stringContent);

            Console.WriteLine($"{response.ToString()}");
            System.Diagnostics.Debug.WriteLine($"{response.ToString()}");

            var result = await response.Content.ReadFromJsonAsync<GetAllPasswordsApiResonse>();

            Console.WriteLine($"{result.ToString()}");
            System.Diagnostics.Debug.WriteLine($"{result.ToString()}");

            // only for debugging
            if (result?.Flag == true)
            {
                Console.WriteLine("About to return success");
                System.Diagnostics.Debug.WriteLine("About to return success");
                return result.PasswordEntries;
            }
            Console.WriteLine("About to return fail");
            System.Diagnostics.Debug.WriteLine("About to return fail");
            return [];
        }

        public async Task<bool> InsertPasswordEntryAsync(NewPasswordEntryDTO passwordEntry)
        {
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/password/insert", passwordEntry);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result?.Flag == true)
                return true;
            return false;
        }

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
