using SpotifyTransfer.Spotify.Interfaces;
using SpotifyTransfer.Spotify.Services;

using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;

using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = builder.Configuration;


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ISpotifyOAuth2, SpotifyOAuth2>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://accounts.spotify.com/authorize");
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-urlencoded"));
});

builder.Services.AddAuthentication(options =>
{
    // to log in
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Events.OnRedirectToReturnUrl += async context =>
    {
        Console.WriteLine("Cavabanga redirect");
    };
})
.AddGoogleOpenIdConnect(options =>
{
    options.ClientId = configuration["youtube:client_id"];
    options.ClientSecret = configuration["youtube:client_secret"];
})
.AddSpotify(options =>
{
    options.ClientId = configuration["spotify:client_id"];
    options.ClientSecret = configuration["spotify:client_secret"];
    // options.CallbackPath = "/callback"; // !!
    options.Scope.Add("user-read-private playlist-modify-public");
    // used to store auth result after sign in
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    options.Events.OnCreatingTicket += async context =>
    {
        var queries = new Dictionary<string, string>();
        queries.Add("code", context.HttpContext.Request.Query["code"]);
        queries.Add("state", context.HttpContext.Request.Query["state"]);
        context.HttpContext.Response.Headers.Location = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("https://localhost7254/return_path", queries);
        Console.WriteLine(context.RefreshToken);
        Console.WriteLine(context.HttpContext.Response.Headers.Location);
    };
});

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

/*app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always,
    //CheckConsentNeeded = context=> true
});*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options => options.WithOrigins("https://accounts.spotify.com/", "https://accounts.google.com"));

app.UseEndpoints(endponts =>
{
    endponts.MapControllers();
    endponts.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
