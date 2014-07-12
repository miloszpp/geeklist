using System;
using SQLite;

namespace GeekList
{
	public class Task
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Due { get; set; }
		public int? Priority { get; set; }
		public bool Completed { get; set; }
	}
}

