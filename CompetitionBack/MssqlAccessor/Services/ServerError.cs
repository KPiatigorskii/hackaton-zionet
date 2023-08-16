using Microsoft.AspNet.SignalR.Hubs;
using MsSqlAccessor.Enums;
using System.Diagnostics;

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

    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            Debug.WriteLine("=> Exception " + exceptionContext.Error.Message);
            if (exceptionContext.Error.InnerException != null)
            {
                Debug.WriteLine("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
            }
            base.OnIncomingError(exceptionContext, invokerContext);

        }
    }
}
