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
        private string twitterUserName;

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

        public async void setTokensToStorage(CoreTweet.Tokens tokensToStorage) 
        {
            await _localStorage.SetItemAsync("TwitterUserName", tokensToStorage.ScreenName);//  put this id to db
            await _localStorage.SetItemAsync("TwitterRequestToken", tokensToStorage.AccessToken);
            await _localStorage.SetItemAsync("TwitterRequestTokenSecret", tokensToStorage.AccessTokenSecret);
        }

        public CoreTweet.OAuth.OAuthSession getTwitterSession()
        {
            twitterSession = OAuth.Authorize(_config.GetSection("Twitter:TWITTER_API_KEY").Value,
                                _config.GetSection("Twitter:TWITTER_API_SECRET").Value);
            return twitterSession;
        }
    }
}
