using ZionetCompetition.Models;

namespace ZionetCompetition.Services
{
	public class FlagService
	{
		private Microsoft​.AspNetCore​.Http.IHttpContextAccessor HttpContextAccessor;
		private ZionetCompetition.Controllers.GenClientController<Team> TeamController;
		private ZionetCompetition.Controllers.GenClientController<EventParticipantTeam> UserEventTeamController;
		private ZionetCompetition.Services.TokenService TokenService;
		private List<EventParticipantTeam> allParticipantEvents = new List<EventParticipantTeam> { };
		public int eventId { get; set; }

		public FlagService() { }

		public async Task ResetActive(int userId) 
		{
			var filtersUET = new Dictionary<string, object>() { { "ParticipantId", userId } };
			var token = await TokenService.GetToken();

			await UserEventTeamController.ConfigureHub(token);
			await UserEventTeamController.StartConnection();
			await UserEventTeamController.GetAll();
			await UserEventTeamController.GetAllWithConditions(filtersUET);
			allParticipantEvents = UserEventTeamController.messages.ToList();

			if(allParticipantEvents.Count > 0)
			{
				foreach(var participantEvent in allParticipantEvents)
				{
					participantEvent.IsActive = false;
					participantEvent.IsApplied = false;
					await UserEventTeamController.Update(participantEvent.Id, participantEvent);
				}
			}


		}
	}
}
