using System;
using System.Collections.Generic;
using SQLite;
using System.IO;

namespace GeekList
{
	public class TaskList : IDisposable
	{
		private IList<Task> tasks;

		private readonly SQLiteConnection db;
		private readonly object connectionLock;

		private static readonly string dbPath = Path.Combine (
			Environment.GetFolderPath (Environment.SpecialFolder.Personal),
			"geekList.db3");

		public IList<Task> TaskCollection { get { return tasks; } }

		public TaskList () {
			tasks = new List<Task> ();
			connectionLock = new object ();
			lock (connectionLock) {
				db = new SQLiteConnection (dbPath);
				db.CreateTable<Task> ();
			}
		}

		public void Persist() {
			lock (connectionLock) {
				db.DeleteAll<Task> ();
				foreach (Task task in tasks) {
					db.Insert (task);
				}
			}
		}

		public void Load() {
			lock (connectionLock) {
				tasks = new List<Task> (db.Table<Task> ());
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			db.Close ();
		}

		#endregion
	}
}

