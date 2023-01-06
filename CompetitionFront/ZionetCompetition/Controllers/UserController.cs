using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR.Client;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Components;
using ZionetCompetition.Errors;

namespace ZionetCompetition.Controllers
{
    public class UserController : Controller
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;
        public IEnumerable<User> messages = new List<User> { };
        public User message;
        private HubConnection hubConnection;
        private bool isLoaded = false;

        public UserController(IJSRuntime jsRuntime, NavigationManager navigationManager) {
            _jsRuntime = jsRuntime;
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7277/users")
                .Build();
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
        public async Task ConfigureHub()
        {
            hubConnection.On<List<User>>("ReceiveGetAll", async (users) =>
            {
                messages = users;
            });

            hubConnection.On<User>("ReceiveGetOne", async (user) =>
            {
                message = user;
            });

            hubConnection.On<User>("ReceiveUpdate", (user) =>
            {
                message = user;
                isLoaded = true;
            });

            hubConnection.On<User>("ReceiveCreate", (user) =>
            {
                message = user;
                isLoaded = true;
            });

            hubConnection.On<User>("ReceiveDelete", (user) =>
            {
                message = user;
                isLoaded = true;
            });
        }

        public async Task GetAll() 
        {
            try
            {
                await hubConnection.InvokeAsync("GetAll");
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
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.ItemNotFound)) NotFoundPage();
                GeneralErr();
            }
        }

        public async Task Update(int id, User user)
        {
            try
            {
                await hubConnection.SendAsync("Update", id, user);
            }
            catch (HubException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.Contains(Errors.Errors.ItemNotFound)) NotFoundPage();
                if (ex.Message.Contains(Errors.Errors.BadRequest)) GeneralErr();
                GeneralErr();
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await hubConnection.SendAsync("Delete", id);
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
