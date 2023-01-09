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
using ZionetCompetition.Errors;
using ZionetCompetition.Services;

namespace ZionetCompetition.Controllers
{
    public class UserController : Controller
    {
        private HubConnection hubConnection;
        private readonly IJSRuntime _jsRuntime;
        private readonly ErrorService _errorService;
        public IEnumerable<User> messages = new List<User> { };
        public User message;
        private bool isLoaded = false;

        public UserController(IJSRuntime jsRuntime, ErrorService errorService) {
            _jsRuntime = jsRuntime;
            _errorService = errorService;
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


            hubConnection.On<List<User>>("ReceiveGetAll", (users) =>
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
                _errorService.Redirect(ex.Message);
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
                _errorService.Redirect(ex.Message);
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
                _errorService.Redirect(ex.Message);
            }
        }

        public async Task Create(User user)
        {
            try
            {
                await hubConnection.InvokeAsync("Create", user);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                _errorService.Redirect(ex.Message);
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
                _errorService.Redirect(ex.Message);
            }
        }
    }
}
