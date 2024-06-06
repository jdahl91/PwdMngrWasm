using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace PwdMngrWasm.State
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private string? _jwtToken = null;
        private string? _refreshToken = null;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // for now this breaks the app if there is a mismatch between the token in the local storage and the token in the provider
            // lets reviw this later
            _jwtToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(_jwtToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = CreateClaimsIdentityFromJwt(_jwtToken);
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string jwt, string refreshToken)
        {
            _jwtToken = jwt;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _jwtToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
            var identity = CreateClaimsIdentityFromJwt(_jwtToken);
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            _jwtToken = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity CreateClaimsIdentityFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtTokenObject = handler.ReadJwtToken(jwtToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, jwtTokenObject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? ""),
                new Claim(ClaimTypes.Email, jwtTokenObject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? ""),
                new Claim(ClaimTypes.Role, jwtTokenObject.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "")
            };

            return new ClaimsIdentity(claims, "jwt");
        }

        //private async Task SetJwtToken()
        //{
        //    _jwtToken = await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", "authToken");
        //}

        //private async Task SetRefreshToken()
        //{
        //    _refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", "refreshToken");
        //}
    }
}
