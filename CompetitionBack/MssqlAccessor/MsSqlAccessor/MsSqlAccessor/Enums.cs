namespace MsSqlAccessor.Enums
{
    public enum StatusEnm
    {
        Active = 1,
        NotActive = 2
    }
	public enum RoleEnm
	{
		Admin = 1,
		Manager = 2,
		Participant = 3
	}

	public enum AppError
    {
        General = 1,
        ConnectionError,
        BadRequest,
        ItemNotFound,
        ConflictData,
        NonNumericInput,
        InputParameterNotSupplied,
        DeletionConflict
    }

    public static class Entities
    {
        public static Dictionary<string, string> Errors = new Dictionary<string, string>() 
        { 
            {"General", "General"},
            {"ConnectionError", "Connection Error" },
            {"BadRequest", "Bad Request"},
            {"ItemNotFound", "Item Not Found"},
            {"ConflictData", "Conflict Data collision" }
        };


    }


}

