using ZionetCompetition.Controllers;
using ZionetCompetition.Models;

namespace ZionetCompetition.Services
{
	public class FlagService
	{
		private Microsoft​.AspNetCore​.Http.IHttpContextAccessor _httpContextAccessor;
		private ZionetCompetition.Controllers.GenClientController<Team> _teamController;
		private ZionetCompetition.Controllers.GenClientController<EventParticipantTeam> _userEventTeamController;
		private ZionetCompetition.Services.TokenService _tokenService;
		private List<EventParticipantTeam> allParticipantEvents = new List<EventParticipantTeam> { };
		public int eventId { get; set; }

		public FlagService(IHttpContextAccessor HttpContextAccessor, TokenService TokenService, GenClientController<Team> TeamController, GenClientController<EventParticipantTeam> UserEventTeamController )

        {
            _httpContextAccessor = HttpContextAccessor;
            _tokenService = TokenService;
            _teamController = TeamController;
            _userEventTeamController = UserEventTeamController;
        }

		public async Task ResetActive(int userId) 
		{
			var filtersUET = new Dictionary<string, object>() { { "ParticipantId", userId } };
			var token = await _tokenService.GetToken();

			await _userEventTeamController.ConfigureHub(token);
			await _userEventTeamController.StartConnection();
			await _userEventTeamController.GetAllWithConditions(filtersUET);
			allParticipantEvents = _userEventTeamController.messages.ToList();

			if(allParticipantEvents.Count > 0)
			{
				foreach(var participantEvent in allParticipantEvents)
				{
					participantEvent.IsActive = false;
					participantEvent.IsApplied = false;
					await _userEventTeamController.Update(participantEvent.Id, participantEvent);
				}
			}


		}
	}
}
