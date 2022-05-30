using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using AspNet.Security.OAuth.Spotify;
using SpotifyTransfer.Spotify.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace SpotifyTransfer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        public async Task<IActionResult> Secured()
        {
            // to not stuck in loop
            /*if (!HttpContext.User.Identity.IsAuthenticated)
            {
                
                await HttpContext.ChallengeAsync("Spotify", new AuthenticationProperties() {RedirectUri="/return_path"});
                await HttpContext.AuthenticateAsync("Spotify");
            }
            else
            {
                await HttpContext.SignInAsync(HttpContext.User);
            }
            Console.WriteLine("Header location: " + HttpContext.Response.Headers.Location);*/
            var properties = new AuthenticationProperties()
            {
                RedirectUri = "return_path",
                AllowRefresh = true
            };
            return Challenge(properties, "Spotify");
        }

        [HttpGet("login_page")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            
            ViewData["ReturnUrl"] = returnUrl;
            Console.WriteLine(nameof(Login));
            return View("");
        }   

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromQuery] string returnUrl)
        {
            Console.WriteLine(nameof(Validate));
            
            return Redirect(System.Net.WebUtility.UrlDecode("https%3A%2F%2Flocalhost%3A7254%2Freturn_path?code=ahoy"));
        }

        [HttpGet("return_path")]
        public async Task ReturnPath([FromQuery] SpotifyAuthorizationResponseModel authorizationResponseModel)
        {
            Console.WriteLine($"{authorizationResponseModel.code}, {authorizationResponseModel.state}, {SpotifyAuthenticationDefaults.TokenEndpoint}");
        }

        [HttpGet("signin-spotify")]
        public async Task SignIn([FromQuery] SpotifyAuthorizationResponseModel authorizationResponseModel)
        {
            Console.WriteLine("CAVABANGA BITCHESS");
            Console.WriteLine($"{authorizationResponseModel.code}, {authorizationResponseModel.state}, {HttpContext.Request.Query["response_type"]}");
            // call to service for gaining access token
        }


    }
}