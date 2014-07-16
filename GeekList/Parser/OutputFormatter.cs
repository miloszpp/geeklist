using System;
using MonoTouch.Foundation;

namespace GeekList
{
	public static class OutputFormatter
	{
		public static DateTime Overdue = new DateTime(999, 9, 9);

		public static string FormatDate(this DateTime? date) {
			if (!date.HasValue) return "Not set";
			if (date == Overdue)
				return "Overdue";
			if (date.Value.Date.Equals(DateTime.Today.Date))
				return "Today";
			if (date.Value.Date.Equals(DateTime.Today.AddDays (1).Date))
				return "Tomorrow";
			return string.Format ("{0:d/M/yyyy}", date.Value);
		}

		public static string FormatDate(this NSDate date) {
			return new DateTime? (date.ToDateTime ()).FormatDate ();
		}

		public static string FormatId(this int id) {
			if (id + 97 > 122)
				return "?";
			return ((char)(id + 97)).ToString();
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

		public static DateTime ToDateTime(this NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime( 
				new DateTime(2001, 1, 1, 0, 0, 0) );
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}
	}
}

