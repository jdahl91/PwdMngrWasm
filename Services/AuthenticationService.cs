using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PwdMngrWasm.DTOs;
using PwdMngrWasm.State;
using static PwdMngrWasm.Responses.CustomResponses;

namespace PwdMngrWasm.Services
{
    // this class will communicate with the server to authenticate the user
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDTO form)
        {
            var customAuthenticationStateProvider = _authenticationStateProvider;
            await customAuthenticationStateProvider.MarkUserAsAuthenticated("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiSm9ha2ltIiwiRW1haWwiOiJqb2FraW1kYWhsQGdteC51cyIsIlJvbGUiOiJBZG1pbiJ9.xwehf7zL11t0lHZhSynNmeQsYZghMdDFoAWNPrYcfhM", "new-v9-refresh-token-test");
            return true;

            //var response = await _httpClient.PostAsJsonAsync("https://example.com/api/auth/login", new { username, password });
            //if (!response.IsSuccessStatusCode) return false;

            //var token = await response.Content.ReadAsStringAsync();
            //await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            //((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(username);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        public async Task LogoutAsync()
        {
            //var response = await _httpClient.PostAsJsonAsync("https://example.com/api/auth/logout");
            //await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

            //((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            //_httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
