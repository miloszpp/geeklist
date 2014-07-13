using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace GeekList
{
	public class CommandParser
	{
		private const string AddCommandPattern = @"([^#\^]+)(((\^(?<DUE>\S+))|(#(?<PRIO>\d+)))\s*)*";
		private const string TaskParameterisedCommand = @"(?<VERB>rm|do|pp) (?<SECTION>[a-z])(\.(?<ROW>[a-z])){0,1}";

		public CommandBase Parse(string commandText) {
			var parameterisedCommandMatch = Regex.Match (commandText, TaskParameterisedCommand);
			if (parameterisedCommandMatch.Success) {
				var verb = parameterisedCommandMatch.Groups ["VERB"];
				var section = char.Parse (parameterisedCommandMatch.Groups ["SECTION"].Value);
				var rowGroup = parameterisedCommandMatch.Groups ["ROW"];

				TaskParameterisedCommand command = null;
				if (verb.Value == "do")
					command = new CompleteCommand ();
				else if (verb.Value == "rm")
					command = new RemoveCommand ();
				else if (verb.Value == "pp")
					command = new PostponeCommand ();

				command.SectionId = ((int)(section)) - 97;
				if (rowGroup.Success) {
					command.RowId = ((int)(char.Parse(rowGroup.Value))) - 97;
				}

				return command;
			}

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
					return command;
				}
			}

			return null;
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

