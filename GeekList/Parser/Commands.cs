using System;

namespace GeekList {

	public abstract class CommandBase {
	}

	public class AddCommand : CommandBase {
		public string Description {
			get;
			set;
		}
		public int? Priority {
			get;
			set;
		}
		public DateTime? Due {
			get;
			set;
		}
	}

	public class TaskParameterisedCommand : CommandBase {
		public int SectionId { get; set; }
		public int? RowId {
			get;
			set;
		}
	}

	public class RemoveCommand : TaskParameterisedCommand {
	}

	public class CompleteCommand : TaskParameterisedCommand {
	}

	public class PostponeCommand : TaskParameterisedCommand {
	}
}