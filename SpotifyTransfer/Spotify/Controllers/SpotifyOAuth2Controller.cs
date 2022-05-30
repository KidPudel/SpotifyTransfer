using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SpotifyTransfer.Spotify.Models;
using SpotifyTransfer.Spotify.Interfaces;

using AspNet.Security.OAuth.Spotify;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SpotifyTransfer.Spotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyOAuth2Controller : ControllerBase
    {
        private readonly ISpotifyOAuth2 _spotifyOAuth2;
        public SpotifyOAuth2Controller(ISpotifyOAuth2 spotifyOAuth2)
        {
            _spotifyOAuth2 = spotifyOAuth2;
        }

        [HttpGet("/login")]
        public async Task<IActionResult> Login()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = "/callback",
                AllowRefresh = true
            };
            return Challenge(properties, "Spotify");
        }

        [HttpGet("/callback")]
        public async Task<string> Callback([FromQuery] SpotifyAuthorizationResponseModel authorizationResponseModel)
        {
            Console.WriteLine($"{authorizationResponseModel.code}, {authorizationResponseModel.state}");

            return "ha";
        }

        [HttpGet("/hey")]
        public async Task<string> Hey([FromQuery] SpotifyAuthorizationResponseModel authorizationResponseModel)
        {
            return "ahoy";
        }
    }
}
