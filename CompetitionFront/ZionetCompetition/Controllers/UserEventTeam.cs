using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR.Client;
using ZionetCompetition.Models;
using BlazorBootstrap;
using Blazorise.DataGrid;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;


namespace ZionetCompetition.Controllers
{
	public class UserEventTeam : Controller
	{
		private readonly IJSRuntime _jsRuntime;
		public IEnumerable<EventParticipantTeam> messages = new List<EventParticipantTeam> { };
		public EventParticipantTeam message;
		private HubConnection hubConnection;
		private bool isLoaded = false;

		public UserEventTeam(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
			hubConnection = new HubConnectionBuilder()
				.WithUrl("https://localhost:7277/eventparticipantteams")
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
			hubConnection.On<List<EventParticipantTeam>>("ReceiveGetAll", async (eventParticipantTeams) =>
			{
				messages = eventParticipantTeams;
				isLoaded = true;
			});

			hubConnection.On<EventParticipantTeam>("ReceiveGetOne", async (eventParticipantTeam) =>
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

		public async void GetAll()
		{
			await hubConnection.SendAsync("GetAll");
			while (!isLoaded) { }
			isLoaded = false;
		}

		public async void Get(int id)
		{
			await hubConnection.SendAsync("GetOne", id);
			while (!isLoaded) { }
			isLoaded = false;
		}

		public async void Update(int id, EventParticipantTeam eventParticipantTeam)
		{
			await hubConnection.SendAsync("Update", id, eventParticipantTeam);
			while (!isLoaded) { }
			isLoaded = false;
		}

		public async void Post(EventParticipantTeam eventParticipantTeam)
		{
			await hubConnection.SendAsync("Create", eventParticipantTeam);
			while (!isLoaded) { }
			isLoaded = false;
		}

		public async void Delete(int id)
		{
			await hubConnection.SendAsync("Delete", id);
			while (!isLoaded) { }
			isLoaded = false;
		}
	}
}
