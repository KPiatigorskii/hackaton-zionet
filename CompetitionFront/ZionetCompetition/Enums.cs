namespace ZionetCompetition.Enums
{
    public enum StatusEnm
    {
        Active = 1,
        NotActive = 2
    }
	public static class Entities
	{
		public static Dictionary<string, string> Roles = new Dictionary<string, string>()
		{
			{"Admin", "Admin"},
			{"Manager", "Manager" },
			{"Participant", "Participant"}
		};


	}
	public enum EnentStatusEnm
    {
        Preparation = 1,
        Disable = 2,
        Running = 3,
        Pause = 4,
        Finished = 6
    }

    public enum TaskStatusEnm
    {
        InProgress = 1,
        Pass = 2,
        OnChecking = 3,
        Approved = 4
    }
}
