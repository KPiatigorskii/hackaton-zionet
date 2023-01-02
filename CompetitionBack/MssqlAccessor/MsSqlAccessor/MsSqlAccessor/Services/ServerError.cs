using MsSqlAccessor.Enums;

namespace MsSqlAccessor.Services
{
    public class ServerError : Exception
    {
        public AppError Error { get; }

        public ServerError(AppError error)
        {
            Error = error;
        }
    }
}
