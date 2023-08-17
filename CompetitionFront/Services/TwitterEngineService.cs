using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ZionetCompetition.Models;

namespace ZionetCompetition.Services
{
    public class TwitterEngineService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TwitterEngineService(IHttpContextAccessor HttpContextAccessor, IHttpClientFactory clientFactory) 
        {
            _httpContextAccessor= HttpContextAccessor;
            _clientFactory= clientFactory;
        }

        public async Task runTweetSearchAsync(long authorId, string teamName, int userId, string eventName) 
        {
            var url = "http://localhost:6978/CronSchedule/start";

            var json = JsonConvert.SerializeObject(new CronProperties {
                userId = userId,
                authorId = authorId,
                teamName = teamName,
                eventName = eventName
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(url, content);
        }

        public async Task SendTweet(string statusMessage)
        {
            var url = "http://localhost:6978/twitter/sendTweet";

            var json = JsonConvert.SerializeObject(new TwitterMessageModel {
                tweetString = statusMessage
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(url, content);
        }


    }
}
