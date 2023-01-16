using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using ZionetCompetition.Services;

namespace ZionetCompetition.Controllers
{
    public class GenClientController<Tmodel> : Controller where Tmodel : class
    {
        public string connectionUrl = $"https://localhost:7277/" + typeof(Tmodel).Name + "s";
        private HubConnection hubConnection;
        private readonly ErrorService _errorService;
        public IEnumerable<Tmodel> messages = new List<Tmodel> { };
        public Tmodel message;
        private bool isLoaded = false;

        public bool IsConnected =>
    hubConnection?.State == HubConnectionState.Connected;

        public GenClientController(ErrorService errorService) {
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


            hubConnection.On<List<Tmodel>>("ReceiveGetAll", (receiveObj) =>
            {
                messages = receiveObj;
                isLoaded = true;
            });

			hubConnection.On<Tmodel>("ReceiveGetOne", (receiveObj) =>
            {
                message = receiveObj;
                isLoaded = true;
            });

            hubConnection.On<Tmodel>("ReceiveUpdate", (receiveObj) =>
            {
                message = receiveObj;
                isLoaded = true;
            });

            hubConnection.On<Tmodel>("ReceiveCreate", (receiveObj) =>
            {
                message = receiveObj;
                isLoaded = true;
            });

            hubConnection.On<Tmodel>("ReceiveDelete", (receiveObj) =>
            {
                message = receiveObj;
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

		public async Task GetAllWithConditions(Dictionary<string, object> filters)
		{
			try
			{
				await hubConnection.InvokeAsync("GetAllWithConditions", filters);
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

		public async Task GetOneWithConditions(Dictionary<string, object> filters)
		{
			try
			{
				await hubConnection.InvokeAsync("GetOneWithConditions", filters);
				while (!isLoaded) { }
				isLoaded = false;
			}
			catch (HubException ex)
			{
				_errorService.Redirect(ex.Message);
			}
		}

		public async Task Update(int id, Tmodel item)
        {
            try
            {
                await hubConnection.InvokeAsync("Update", id, item);
                while (!isLoaded) { }
                isLoaded = false;
            }
            catch (HubException ex)
            {
                _errorService.Redirect(ex.Message);
            }
        }

        public async Task Create(Tmodel item)
        {
            try
            {
                await hubConnection.InvokeAsync("Create", item);
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
