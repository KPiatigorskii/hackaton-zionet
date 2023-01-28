namespace MsSqlAccessor.Enums
{
    public enum StatusEnm
    {
        Active = 1,
        NotActive = 2
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

    public enum EventStatusEnm
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
        Discard,
        OnCheck,
        OnChecking,
        Approved,
        Rejected,
        NeedHelp,
    }

}

