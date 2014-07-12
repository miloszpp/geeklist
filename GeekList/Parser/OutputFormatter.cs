using System;

namespace GeekList
{
	public static class OutputFormatter
	{
		public static string FormatDate(this DateTime? date) {
			if (!date.HasValue) return "Not set";
			if (date == DateTime.Today)
				return "Today";
			if (date == DateTime.Today.AddDays (1))
				return "Tomorrow";
			return string.Format ("{0:d/M/yyyy}", date.Value);
		}

		public static string FormatPriority(this int? priority) {
			if (priority == 1) 
				return "Low";
			if (priority == 2)
				return "Medium";
			if (priority == 3)
				return "High";
			return "Not set";
		}
	}
}

