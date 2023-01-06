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
        ConnectionError = 2,
        BadRequest = 3,
        NoData = 4,
        NonNumericInput = 5,
        InputParameterNotSupplied = 6,
        DeletionConflict = 7
    }
}

