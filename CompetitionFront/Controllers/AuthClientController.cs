using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using ZionetCompetition.Services;

namespace ZionetCompetition.Controllers
{
    public class AuthClientController<User> : Controller
    {
        public string connectionUrl = $"https://mssqlaccessor/Users";
        private HubConnection hubConnection;
        private readonly ErrorService _errorService;
        public User message;
        private bool isLoaded = false;

        public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

        public AuthClientController(ErrorService errorService) {
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
                .WithUrl(connectionUrl, options =>
                        {
                            options.AccessTokenProvider = () => Task.FromResult(tokenString);
                        })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<User>("ReceiveGetOneByEmail", (receiveObj) =>
            {
                message = receiveObj;
                isLoaded = true;
            });

            hubConnection.On<User>("ReceiveRegister", (receiveObj) =>
            {
                message = receiveObj;
                isLoaded = true;
            });

        }

        public async Task Get(string Email)
        {
            try
            {
                await hubConnection.InvokeAsync("GetOneByEmail", Email);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                _errorService.Redirect(ex.Message);
            }
        }

        public async Task Register(User item)
        {
            try
            {
                await hubConnection.InvokeAsync("Register", item);
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
