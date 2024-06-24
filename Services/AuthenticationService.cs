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
            //var customAuthenticationStateProvider = _authenticationStateProvider;
            //await customAuthenticationStateProvider.MarkUserAsAuthenticated("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiSm9ha2ltIiwiRW1haWwiOiJqb2FraW1kYWhsQGdteC51cyIsIlJvbGUiOiJBZG1pbiJ9.xwehf7zL11t0lHZhSynNmeQsYZghMdDFoAWNPrYcfhM", "new-v9-refresh-token-test");
            //return true;

            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/account/login", form);
            
            if (!response.IsSuccessStatusCode)
                return false;
            
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var jwtToken = loginResponse?.JwtToken;
            var refreshToken = loginResponse?.RefreshToken;

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                return false;

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(jwtToken, refreshToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _httpClient.PostAsync("https://834627.xyz/api/account/logout", null);
            if (!response.IsSuccessStatusCode)
                return false;
            
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDTO form)
        {
            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/account/register", form);
            if (!response.IsSuccessStatusCode)
                return false;

            var registrationResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();
            
            // hopefully this wont throw an exception, it should return false
            // when no response or false flag indicating unsuccessful registration
            if (!registrationResponse?.Flag ?? true)
                return false;
            // else successfull registration
            return true;
        }

        public async Task<bool> RefreshTokensAsync()
        {
            var oldRefreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (string.IsNullOrEmpty(oldRefreshToken))
                return false;

            var response = await _httpClient.PostAsJsonAsync("https://834627.xyz/api/account/refresh-token", oldRefreshToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var refreshResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (refreshResponse == null || refreshResponse.Flag == false)
                return false;

            var jwtToken = refreshResponse.JwtToken;
            var newRefreshToken = refreshResponse.RefreshToken;

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(newRefreshToken))
                return false;

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(jwtToken, newRefreshToken);

            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return true;
        }
    }
}
