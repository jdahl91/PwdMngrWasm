using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PwdMngrWasm;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using PwdMngrWasm.Services;
using PwdMngrWasm.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore(o =>
{
    o.AddPolicy("User", policy => policy.RequireRole("User"));
    o.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

await builder.Build().RunAsync();
