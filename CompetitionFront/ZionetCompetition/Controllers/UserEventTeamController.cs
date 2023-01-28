using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;

namespace ZionetCompetition.Controllers
{
	public class UserEventTeamController : Controller
	{
		private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;
        public IEnumerable<EventParticipantTeam> messages = new List<EventParticipantTeam> { };
		public EventParticipantTeam message;
		private HubConnection hubConnection;
		private bool isLoaded = false;

		public UserEventTeamController(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
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
            .WithUrl("https://localhost:7277/eventparticipantteams", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(tokenString);
            })
            .WithAutomaticReconnect()
                .Build();

            hubConnection.On<List<EventParticipantTeam>>("ReceiveGetAll", (eventParticipantTeams) =>
			{
				messages = eventParticipantTeams;
				isLoaded = true;
			});

			hubConnection.On<EventParticipantTeam>("ReceiveGetOne", (eventParticipantTeam) =>
			{
				message = eventParticipantTeam;
				isLoaded = true;
			});

            hubConnection.On<EventParticipantTeam>("ReceiveUpdate", (eventParticipantTeam) =>
            {
                message = eventParticipantTeam;
                isLoaded = true;
            });

            hubConnection.On<EventParticipantTeam>("ReceiveCreate", (eventParticipantTeam) =>
			{
				message = eventParticipantTeam;
				isLoaded = true;
			});

			hubConnection.On<EventParticipantTeam>("ReceiveDelete", (eventParticipantTeam) =>
			{
				message = eventParticipantTeam;
				isLoaded = true;
			});
		}

		public async Task GetAll()
		{

            try
            {
                await hubConnection.SendAsync("GetAll");
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
                await hubConnection.SendAsync("GetOne", id);
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

		public async Task Update(int id, EventParticipantTeam eventParticipantTeam)
		{
            try
            {
                await hubConnection.InvokeAsync("Update", id, eventParticipantTeam);
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

		public async Task Post(EventParticipantTeam eventParticipantTeam)
		{
            try
            {
                await hubConnection.SendAsync("Create", eventParticipantTeam);
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
                await hubConnection.SendAsync("Delete", id);
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
