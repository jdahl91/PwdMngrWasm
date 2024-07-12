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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
        private readonly string _baseAddress = "https://834627.xyz/api/account";

        public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authenticationStateProvider = (CustomAuthenticationStateProvider)authenticationStateProvider;
        }

        // revisit guid after changing the api
        public async Task<bool> LoginAsync(LoginDTO form)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(_baseAddress + "/login", form);
            
            if (!httpResponse.IsSuccessStatusCode)
                return false;
            
            var loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();

            var jwtToken = loginResponse?.JwtToken;
            var refreshToken = loginResponse?.RefreshToken;
            var userGuidString = loginResponse?.UserGuid;
            //var userGuid = Guid.Parse(userGuidString);

            // for debugging
            if (!Guid.TryParseExact(userGuidString, "D", out Guid userGuid)) return false;
            else Console.WriteLine(userGuid.ToString());

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                return false;

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(jwtToken, refreshToken, userGuid);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await _httpClient.PostAsync(_baseAddress + "/logout", null);
            if (!response.IsSuccessStatusCode)
                return false;
            
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDTO form)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseAddress + "/register", form);
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

        // Need to revisit userGuid here
        public async Task<bool> RefreshTokensAsync()
        {
            var oldRefreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");
            var userGuidString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userGuid");
            var userGuid = Guid.Parse(userGuidString);

            if (string.IsNullOrEmpty(oldRefreshToken))
                return false;

            var response = await _httpClient.PostAsJsonAsync(_baseAddress + "/refresh-token", oldRefreshToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var refreshResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (refreshResponse == null || refreshResponse.Flag == false)
                return false;

            var jwtToken = refreshResponse.JwtToken;
            var newRefreshToken = refreshResponse.RefreshToken;

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(newRefreshToken))
                return false;

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(jwtToken, newRefreshToken, userGuid);

            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return true;
        }
    }
}
