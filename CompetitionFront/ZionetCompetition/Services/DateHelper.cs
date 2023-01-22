namespace ZionetCompetition.Services
{
	public static class DateHelper
	{
		public static bool IsBetweenDates(DateTime input, DateTime? StartTime, DateTime? EndTime)
		{
			if (StartTime is not null)
			{
				if (EndTime <= StartTime) EndTime = StartTime?.AddDays(1);
			}
			else
			{
				return false;
			}

			
			return (input > StartTime && input < EndTime);
		}
	}
}
