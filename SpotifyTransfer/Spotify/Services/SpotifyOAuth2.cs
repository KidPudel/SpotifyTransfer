using Microsoft.AspNetCore.Mvc;
using SpotifyTransfer.Spotify.Interfaces;
using SpotifyTransfer.Spotify.Models;

using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.WebUtilities;

namespace SpotifyTransfer.Spotify.Services
{
    public class SpotifyOAuth2 : ISpotifyOAuth2
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public SpotifyOAuth2(HttpClient httpClient, IConfiguration configuration)
        {
           _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task GetAuthorized()
        {
            var queries = new Dictionary<string, string>();
            queries.Add("client_id", _configuration["spotify:client_id"]);
            queries.Add("scope", "user-read-private");
            queries.Add("response_type", "code");
            queries.Add("redirect_uri", "https://localhost:7254/callback");
            queries.Add("state", "CfDJ8Cq0wPCDBCZBn-rxDr1K-9ZNWCr4zar5OqB5ywRbsNyGG7DdXtHKz9xT7RodJEkDJdpBkR9DdQkC4EKR6F1a7b_0nzJNXNjoJfhwteHM0m8L9FMBgChbQTMxrh-OustOjjAC80MhVNz2ohJamZFwfs2siya_va8IaPWSzrkN-Xhu5KPBZBCMY-3VBN-Sw383d2pzn58Lw231rvtt8IswOebsj-6vbfVe6sCpU5qZEgzL");
            var path = _httpClient.BaseAddress.ToString();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(QueryHelpers.AddQueryString(path, queries))
            };
        }

        public async Task<SpotifyOAuth2TokenModel> GetToken()
        {
            throw new NotImplementedException();
        }

        public Task<SpotifyOAuth2RefreshModel> RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
