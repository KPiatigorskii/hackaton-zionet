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
    public class UserController : Controller, IAsyncDisposable
    {

        private readonly IJSRuntime _jsRuntime;
        public IEnumerable<User> messages = new List<User> { };
        public User message;
        private HubConnection hubConnection;
        private bool isLoaded = false;

        public UserController(IJSRuntime jsRuntime) {
            _jsRuntime = jsRuntime;
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7277/users")
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
            hubConnection.On<List<User>>("ReceiveUsers", async (users) =>
            {
                messages = users;
                isLoaded = true;
            });

            hubConnection.On<User>("ReceiveUser", async (user) =>
            {
                message = user;
                isLoaded = true;
            });

            hubConnection.On<User>("CreateUser", (user) =>
            {
                message = user;
                isLoaded = true;
            });

            hubConnection.On<User>("DeleteUser", (user) =>
            {
                message = user;
                isLoaded = true;
            });

            hubConnection.On<User>("UpdateUser", (user) =>
            {
                message = user;
                isLoaded = true;
            });
        }

        public async void GetAll() 
        {
            await hubConnection.SendAsync("GetAll");
            while (!isLoaded) { }
            isLoaded= false;
        }

        public async void Get(int id)
        {
            await hubConnection.SendAsync("GetOne", id);
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async void Update(int id, User user)
        {
            await hubConnection.SendAsync("UpdateOne", id, user);
            while (!isLoaded) { }
            isLoaded = false;
        }

        public async void Delete(int id)
        {
            await hubConnection.SendAsync("ForceDeleteOne", id);
            while (!isLoaded) { }
            isLoaded = false;
        }













        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }



    }
}
