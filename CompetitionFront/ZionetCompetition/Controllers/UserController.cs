using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Auth0.AspNetCore.Authentication;
using System.Security.Claims;
using NuGet.Common;
using Newtonsoft.Json.Linq;

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
                .WithUrl("https://localhost:7277/users", options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(tokenString);
                        })
                .Build();


            hubConnection.On<List<User>>("ReceiveGetAll", async (users) =>
            {
                messages = users;
                isLoaded = true;
            });

            hubConnection.On<User>("ReceiveGetOne", (user) =>
            {
                message = user;
                isLoaded = true;
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

        public async Task Update(int id, User user)
        {
            try
            {
                await hubConnection.InvokeAsync("Update", id, user);
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
