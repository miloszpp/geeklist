using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace GeekList
{
	public class CommandParser
	{
		private const string AddCommandPattern = @"add ([^#\^]+)(((\^(?<DUE>\S+))|(#(?<PRIO>\d+)))\s*)*";
		private const string RemoveCommandPattern = @"rm (\d+)\.(\d+)";
		private const string DoCommandPattern = @"do (\d+)\.(\d+)";

		public IList<CommandBase> Parse(string commandText) {
			var result = new List<CommandBase> ();

			var addCommandMatch = Regex.Match (commandText, AddCommandPattern);
			if (addCommandMatch.Success) {
				var description = addCommandMatch.Groups [1].Value;
				var priority = addCommandMatch.Groups ["PRIO"].Success ? new int?(int.Parse(addCommandMatch.Groups ["PRIO"].Value)) : null;
				var dueString = addCommandMatch.Groups ["DUE"].Success ? addCommandMatch.Groups ["DUE"].Value : null;
				var due = ParseDate (dueString);

				if ((dueString == null || due != null) &&
					(priority == null || (priority <= 3 && priority >= 1))) {
					var command = new AddCommand {
						Description = description,
						Priority = priority,
						Due = due
					};
					result.Add (command);
				}
			}
			var removeCommandMatch = Regex.Match (commandText, RemoveCommandPattern);
			if (removeCommandMatch.Success) {
				var section = int.Parse (removeCommandMatch.Groups [1].Value);
				var row = int.Parse (removeCommandMatch.Groups [2].Value);
				var command = new RemoveCommand {
					SectionId = section,
					RowId = row
				};
				result.Add (command);
			}
			var doCommandMatch = Regex.Match (commandText, DoCommandPattern);
			if (doCommandMatch.Success) {
				var section = int.Parse (doCommandMatch.Groups [1].Value);
				var row = int.Parse (doCommandMatch.Groups [2].Value);
				var command = new CompleteCommand {
					SectionId = section,
					RowId = row
				};
				result.Add (command);
			}

			return result;
		}

		private DateTime? ParseDate(string dateString) {
			if (dateString == "today")
				return DateTime.Today;
			if (dateString == "tomorrow")
				return DateTime.Today.AddDays (1);
			DateTime result;
			if (DateTime.TryParseExact (dateString, "MMMd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result)) {
				return result;
			}

			return null;
		}
	}
}

