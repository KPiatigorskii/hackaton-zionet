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
           //tokenString =
           // hubConnection.User = HttpContextAccessor.HttpContext.User;
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

            hubConnection.On<User>("ReceiveGetOne", async (user) =>
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

        public async void GetAll() 
        {
            await hubConnection.InvokeAsync("GetAll");
            while (!isLoaded) { }
            isLoaded= false;
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
                if (ex.Message.Contains("Not found")) NotFoundPage();
            }
            //while (!isLoaded) { }
            isLoaded = false;
        }

        public async void Update(int id, User user)
        {
            await hubConnection.SendAsync("Update", id, user);
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async void Delete(int id)
        {
            await hubConnection.SendAsync("Delete", id);
            while (!isLoaded) { }
            isLoaded = false;
        }

        private void NotFoundPage()
        {
            _navigationManager.NavigateTo("/404");
        }
    }
}
