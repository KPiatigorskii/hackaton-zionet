using ZionetCompetition.Controllers;
using ZionetCompetition.Models;

namespace ZionetCompetition.Services
{
		public class AuthorizationService : IUserClaims, IManagerClaims
	{
		private Microsoft​.AspNetCore​.Http.IHttpContextAccessor _httpContextAccessor;
		private ZionetCompetition.Services.TokenService _tokenService;
		private ZionetCompetition.Controllers.AuthClientController<User> _authController;
		private ZionetCompetition.Controllers.GenClientController<EventManager> _EventTaskController;
		private ZionetCompetition.Controllers.GenClientController<EventParticipantTeam> _EventParticipantTeamController;
        public string Email { get; set; }
        public int CurrentEventId { get; set; }
		public string CurrentEventName { get; set; }
		public bool IsActive { get; set; }
		public bool IsApplied { get; set; }
		public int CurrentTeamtId { get; set; }
		public string CurrentTeamName { get; set; }
		public bool IsLeader { get; set; }

		public AuthorizationService(
			IHttpContextAccessor httpContextAccessor,
			TokenService tokenService,
			AuthClientController<User> authController,
			GenClientController<EventManager> eventTaskController,
			GenClientController<EventParticipantTeam> eventParticipantTeamController)
		{
			_httpContextAccessor = httpContextAccessor;
			_tokenService = tokenService;
			_authController = authController;
			_EventTaskController = eventTaskController;
			_EventParticipantTeamController = eventParticipantTeamController;
		}

		public async Task<IUserClaims> GetParticipantClaims()
		{
			var claims = _httpContextAccessor.HttpContext.User.Claims;
			IUserClaims userClaims = new UserClaims
			
			{	
				Email = claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email")?.Value ?? "",
				CurrentEventId = int.Parse(claims.FirstOrDefault(e => e.Type == "currentEventId")?.Value ?? ""),
				CurrentEventName = claims.FirstOrDefault(e => e.Type == "currentEventName")?.Value ?? "",
				IsActive = Boolean.Parse(claims.FirstOrDefault(e => e.Type == "isActive")?.Value ?? "false"),
				IsApplied = Boolean.Parse(claims.FirstOrDefault(e => e.Type == "isApplied")?.Value ?? "false"),
				CurrentTeamtId = int.Parse(claims.FirstOrDefault(e => e.Type == "currentTeamId")?.Value ?? ""),
				CurrentTeamName = claims.FirstOrDefault(e => e.Type == "currentTeamName")?.Value ?? "",
				IsLeader = Boolean.Parse(claims.FirstOrDefault(e => e.Type == "isLeader")?.Value ?? "false"),
			};
			return userClaims;
		}
		public async Task<IManagerClaims> GetManagerClaims()
		{
			var claims = _httpContextAccessor.HttpContext.User.Claims;
			IManagerClaims userClaims = new ManagerClaims
			{
                Email = claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email")?.Value ?? "",
                CurrentEventId = int.Parse(claims.FirstOrDefault(e => e.Type == "currentEventId")?.Value ?? ""),
				CurrentEventName = claims.FirstOrDefault(e => e.Type == "currentEventName")?.Value ?? "",
			};
			return userClaims;
		}
	}


}


