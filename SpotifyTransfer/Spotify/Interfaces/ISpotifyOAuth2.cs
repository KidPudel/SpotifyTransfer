using Microsoft.AspNetCore.Mvc;
using SpotifyTransfer.Spotify.Models;

namespace SpotifyTransfer.Spotify.Interfaces
{
    public interface ISpotifyOAuth2
    {
        /// <summary>
        /// method that requests authorization from the user to access the spotify resources in behalf that user
        /// </summary>
        /// <returns></returns>
        Task GetAuthorized();

        /// <summary>
        /// Method to get OAuth 2.0 access to make requests to spotify API
        /// </summary>
        /// <returns></returns>
        Task<SpotifyOAuth2TokenModel> GetToken();

        /// <summary>
        /// Method that refreshes expired access token and generates a new one
        /// </summary>
        /// <returns></returns>
        Task<SpotifyOAuth2RefreshModel> RefreshToken();
    }
}
