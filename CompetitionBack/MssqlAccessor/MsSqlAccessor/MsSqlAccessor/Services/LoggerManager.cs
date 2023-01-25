using MsSqlAccessor.Interfaces;
using NLog;
using ILogger = NLog.ILogger;

namespace MsSqlAccessor.Services
{
	public class LoggerManager : ILoggerManager
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();
		public void LogDebug(string message) => logger.Debug(message);
		public void LogError(string message) => logger.Error(message);
		public void LogInfo(string message) => logger.Info(message);
		public void LogWarn(string message) => logger.Warn(message);

		public void LogDebug(string message, Exception exception) => logger.Debug(message);
		public void LogError(string message, Exception exception) => logger.Error(message);
		public void LogInfo(string message, Exception exception) => logger.Info(message);
		public void LogWarn(string message, Exception exception) => logger.Warn(message);

	}
}
