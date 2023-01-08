using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR.Client;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using ZionetCompetition.Data;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;


namespace ZionetCompetition.Controllers
{
    public class EventController : Controller
    {

        private readonly IJSRuntime _jsRuntime;
        public IEnumerable<EventModel> messages;
        private readonly NavigationManager _navigationManager;
        public EventModel message;
        private HubConnection hubConnection;
        private bool isLoaded = false;

        public EventController(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        public async Task StartConnection()
        {
            await hubConnection.StartAsync();
        }

        public async Task StopConnection()
        {
            await hubConnection.StopAsync();
        }
        public async Task ConfigureHub(string tokenString)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7277/events", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(tokenString);
                })
                .Build();

            hubConnection.On<List<EventModel>>("ReceiveEvents", async (events) =>
            {
                messages = events;
                isLoaded = true;
            });

            hubConnection.On<EventModel>("ReceiveEvent", async (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<EventModel>("CreateEvent", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<EventModel>("DeleteEvent", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<EventModel>("UpdateEvent", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });
        }

        public async Task GetAll()
        {
            await hubConnection.SendAsync("GetAll");
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async Task Get(int id)
        {
            await hubConnection.SendAsync("GetOne", id);
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async Task Update(int id, EventModel @event, int userId)
        {
            await hubConnection.SendAsync("UpdateOne", id, message, userId);
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async Task Delete(int id)
        {
            await hubConnection.SendAsync("ForceDeleteOne", id);
            while (!isLoaded) { }
            isLoaded = false;
        }
    }
}
