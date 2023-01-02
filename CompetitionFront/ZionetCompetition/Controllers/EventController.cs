using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR.Client;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;


namespace ZionetCompetition.Controllers
{
    public class EventController : Controller
    {

        private readonly IJSRuntime _jsRuntime;
        public IEnumerable<Event> messages = new List<Event> { };
        public Event message;
        private HubConnection hubConnection;
        private bool isLoaded = false;

        public EventController(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7277/events")
                .Build();
        }

        public async Task StartConnection()
        {
            await hubConnection.StartAsync();
        }

        public async Task StopConnection()
        {
            await hubConnection.StopAsync();
        }
        public async Task ConfigureHub()
        {
            hubConnection.On<List<Event>>("ReceiveEvents", async (events) =>
            {
                messages = events;
                isLoaded = true;
            });

            hubConnection.On<Event>("ReceiveEvent", async (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("CreateEvent", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("DeleteEvent", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("UpdateEvent", (@event) =>
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

        public async Task Update(int id, Event @event)
        {
            await hubConnection.SendAsync("UpdateOne", id, @event);
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
