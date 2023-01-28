using Microsoft.Build.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Interfaces;
using MsSqlAccessor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Managers
{
    public class EventLogicManager
    {
        private readonly GenDbController<Event, EventDTO> _dbController;
        private readonly TwitterHelper _twitterHelper;
        private readonly IConfiguration _config;
        private readonly ILoggerManager _logger;

        public EventLogicManager(GenDbController<Event, EventDTO> dbController, TwitterHelper twitterHelper, IConfiguration configuration, ILoggerManager logger)
        {
            _dbController = dbController;
            _twitterHelper = twitterHelper;
            _config = configuration;
            _logger = logger;
        }
        public async Task startEvent(Dictionary<string, object> arguments) {
            EventDTO eventItem;
            string tweet_message;
            string userEmail = "";
            int eventId = 0;

            try // unpack needed arguments
            {
                foreach (var item in arguments)
                {
                    if (item.Key == "Id")
                    {
                        eventId = int.Parse(item.Value.ToString());
                    }
                    if (item.Key == "email")
                    {
                        userEmail = item.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: can't parse dictionary for startEvent function");
                throw ex;
            }

            try
            {
                eventItem = await _dbController.GetOne(eventId);
                eventItem.StartTime = DateTime.UtcNow;
                eventItem.EventStatusId = 3; // running status
                await _dbController.Update(eventItem.Id, eventItem, userEmail);

                tweet_message = _config.GetSection("Twitter:TWUTTER_RUN_PHRASE")?.Value.ToString()
                    .Replace("{eventName}", eventItem.Title);
                _twitterHelper.SendTweet(tweet_message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: execute startEvent function");
                throw ex;
            }
        }
    }
}
