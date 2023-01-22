namespace ZionetCompetition.Models
{
	public interface IUserClaims
	{
		string Email { get; set; }
		int CurrentEventId { get; set; }
		string CurrentEventName { get; set; }
		bool IsActive { get; set; }
		bool IsApplied { get; set; }
		int CurrentTeamtId { get; set; }
		string CurrentTeamName { get; set; }
		bool IsLeader { get; set; }
	}

	public interface IManagerClaims
	{
        string Email { get; set; }
        int CurrentEventId { get; set; }
		string CurrentEventName { get; set; }
	}
	public class UserClaims : IUserClaims
	{
        public string Email { get; set; }
        public int CurrentEventId { get; set; }
		public string CurrentEventName { get; set; }
		public bool IsActive { get; set; }
		public bool IsApplied { get; set; }
		public int CurrentTeamtId { get; set; }
		public string CurrentTeamName { get; set; }
		public bool IsLeader { get; set; }
	}
	public class ManagerClaims : IManagerClaims
	{
        public string Email { get; set; }
        public int CurrentEventId { get; set; }
		public string CurrentEventName { get; set; }
	}
}
