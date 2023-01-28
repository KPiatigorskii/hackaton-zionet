using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.SignalR;
using ZionetCompetition.Data;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;


namespace ZionetCompetition.Controllers
{
    public class EventController : Controller
    {
        private readonly IJSRuntime _jsRuntime;
        public IEnumerable<Event> messages;
        private readonly NavigationManager _navigationManager;
        public Event message;
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

            hubConnection.On<List<Event>>("ReceiveGetAll", async (events) =>
            {
                messages = events;
                isLoaded = true;
            });

            hubConnection.On<Event>("ReceiveGetOne", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("ReceiveUpdate", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("ReceiveCreate", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });

            hubConnection.On<Event>("ReceiveDelete", (@event) =>
            {
                message = @event;
                isLoaded = true;
            });
        }

        public async Task GetAll()
        {
            try
            {
                await hubConnection.InvokeAsync("GetAll");
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                GeneralErr();
            }
        }

        public async Task Get(int id)
        {
            try
            {
                await hubConnection.InvokeAsync("GetOne", id);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.ItemNotFound)) NotFoundPage();
                GeneralErr();
            }
        }

        public async Task Update(int id, Event @event)
        {
            try
            {
                await hubConnection.InvokeAsync("Update", id, @event);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.ItemNotFound)) NotFoundPage();
                if (ex.Message.Contains(Errors.Errors.BadRequest)) GeneralErr();
                GeneralErr();
            }
        }

        public async Task Post(Event @event)
        {
            try
            {
                await hubConnection.SendAsync("Create", @event);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.BadRequest)) GeneralErr();
                GeneralErr();
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await hubConnection.InvokeAsync("Delete", id);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.ItemNotFound)) NotFoundPage();
                GeneralErr();
            }
        }

        private void NotFoundPage()
        {
            _navigationManager.NavigateTo("/404");
        }

        private void GeneralErr()
        {
            _navigationManager.NavigateTo("/500");
        }
    }
}
