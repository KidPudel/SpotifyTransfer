using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SpotifyTransfer.YouTube.Models;

using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

namespace SpotifyTransfer.YouTube.Services
{
    [Route("[controller]")]
    [ApiController]
    public class YouTubePlaylistItemsController : ControllerBase
    {
        [GoogleScopedAuthorize(YouTubeService.ScopeConstants.YoutubeReadonly)]
        [HttpGet]
        public async Task<IActionResult> ListItems([FromServices] IGoogleAuthProvider auth)
        {
            // client credentials for the current user
            var credentials = await auth.GetCredentialAsync();

            // construct a youtube service
            var service = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials
            });

            // get a channel id
            var channelsRequest = service.Channels.List("contentDetails");
            channelsRequest.Mine = true;

            var channelResponse = await channelsRequest.ExecuteAsync();

            Google.Apis.YouTube.v3.Data.PlaylistItemListResponse playlistResponse = new Google.Apis.YouTube.v3.Data.PlaylistItemListResponse();

            foreach (var channel in channelResponse.Items)
            {
                // get a channel's playlists ids
                var playlistsRequest = service.Playlists.List("snippet");
                playlistsRequest.ChannelId = channel.Id;

                var playlistsResponse = await playlistsRequest.ExecuteAsync();

                foreach (var playlist in playlistsResponse.Items)
                {
                    if (playlist.Snippet.Title == "I Hope We Make It To The Other Side")
                    {
                        var playlistRequest = service.PlaylistItems.List("snippet");
                        playlistRequest.PlaylistId = playlist.Id;
                        playlistRequest.MaxResults = 25;

                        playlistResponse = await playlistRequest.ExecuteAsync();

                        foreach (var playlistItem in playlistResponse.Items)
                        {
                            Console.WriteLine(playlistItem.Snippet.Title);
                        }

                    }
                }
            }
            return Ok(playlistResponse);

        }
    }
}
