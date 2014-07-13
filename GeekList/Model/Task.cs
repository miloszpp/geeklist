using System;

namespace GeekList
{
	public class Task
	{
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Due { get; set; }
		public int? Priority { get; set; }
		public bool Completed { get; set; }
	}
}

