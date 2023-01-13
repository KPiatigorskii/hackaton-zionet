using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using CoreTweet;
using Azure.Core;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using ZionetCompetition.Models;

namespace ZionetCompetition.Services
{
    public class TwitterService
    {
        private CoreTweet.OAuth.OAuthSession twitterSession;
        private CoreTweet.Tokens tokens;

        private readonly IConfiguration _config;
        private readonly ILocalStorageService _localStorage;

        public TwitterService(IConfiguration config, ILocalStorageService localStorage) // 
        {
            _config = config;
            _localStorage = localStorage;
        }

        public CoreTweet.Tokens getTokens(string accessToken, string accessTokenSecret)
        {
            tokens = Tokens.Create(_config.GetSection("Twitter:TWITTER_API_KEY").Value,
                _config.GetSection("Twitter:TWITTER_API_SECRET").Value, accessToken, accessTokenSecret);
            return tokens;
        }

        public CoreTweet.Tokens getTokensByPIN(CoreTweet.OAuth.OAuthSession twitterSession, string PINCode)
        {
            return tokens = OAuth.GetTokens(twitterSession, PINCode);
        }

        public async Task setTokensToStorage(CoreTweet.Tokens tokensToStorage) 
        {
            await _localStorage.SetItemAsync("TwitterUserName", tokensToStorage.ScreenName);//  put this id to db
            await _localStorage.SetItemAsync("TwitterRequestToken", tokensToStorage.AccessToken);
            await _localStorage.SetItemAsync("TwitterRequestTokenSecret", tokensToStorage.AccessTokenSecret);
        }

        public async Task<bool> isTwitterAuthorized() 
        {
            var twitterUserName = await _localStorage.GetItemAsync<string>("TwitterUserName");
            var accessToken = await _localStorage.GetItemAsync<string>("TwitterRequestToken");
            var accessTokenSecret = await _localStorage.GetItemAsync<string>("TwitterRequestTokenSecret");
            return (!string.IsNullOrEmpty(twitterUserName) && !string.IsNullOrEmpty(accessToken)
                && !string.IsNullOrEmpty(accessTokenSecret));
        }

        public async Task<string> getTwitterUserName()
        {
            return await _localStorage.GetItemAsync<string>("TwitterUserName");
        }

        public async Task<string> getTwitterRequestToken()
        {
            return await _localStorage.GetItemAsync<string>("TwitterRequestToken");
        }

        public async Task<string> getTwitterRequestTokenSecret()
        {
            return await _localStorage.GetItemAsync<string>("TwitterRequestTokenSecret");
        }

        public CoreTweet.OAuth.OAuthSession getTwitterSession()
        {
            twitterSession = OAuth.Authorize(_config.GetSection("Twitter:TWITTER_API_KEY").Value,
                                _config.GetSection("Twitter:TWITTER_API_SECRET").Value);
            return twitterSession;
        }

        public void startEventTweet(string eventName)
        {
            tokens.Statuses.Update(status => $"Challenge {eventName} was started!");
        }

        public void stopEventTweet(string eventName)
        {
            tokens.Statuses.Update(status => $"Challenge {eventName} was stopped!");
        }
    }
}
