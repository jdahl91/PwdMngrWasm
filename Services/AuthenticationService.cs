using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public async Task<LoginResponse> Login(LoginDTO form)
        {
            //var response = await _httpClient.PostAsJsonAsync("https://example.com/api/auth/login", new { username, password });
            //if (!response.IsSuccessStatusCode) return false;

            //var token = await response.Content.ReadAsStringAsync();
            //await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            //((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(username);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return new LoginResponse();
        }

        public async Task Logout()
        {
            //var response = await _httpClient.PostAsJsonAsync("https://example.com/api/auth/logout");
            //await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

            //((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            //_httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
