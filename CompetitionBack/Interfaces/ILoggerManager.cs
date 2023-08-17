namespace MsSqlAccessor.Interfaces
{
	public interface ILoggerManager
	{
		void LogInfo(string message);
		void LogWarn(string message);
		void LogError(string message);
		void LogDebug(string message);

		void LogInfo(string message, Exception exception);
		void LogWarn(string message, Exception exception);
		void LogError(string message, Exception exception);
		void LogDebug(string message, Exception exception);
		
	}
}
