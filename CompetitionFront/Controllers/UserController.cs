using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using ZionetCompetition.Models;
using ZionetCompetition.Services;

namespace ZionetCompetition.Controllers
{
    public class UserController : Controller
    {
        private HubConnection hubConnection;
        private readonly ErrorService _errorService;
        public IEnumerable<User> messages = new List<User> { };
        public User message;
        private bool isLoaded = false;

        public UserController(ErrorService errorService) {
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

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
        public async Task ConfigureHub(string tokenString)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://mssqlaccessor:7277/users", options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(tokenString);
                        })
				.WithAutomaticReconnect()
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
