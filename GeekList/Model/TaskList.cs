using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GeekList
{
	public class TaskList : IDisposable
	{
		private List<Task> tasks;
		private XmlSerializer serializer;

		private static readonly string dbPath = Path.Combine (
			Environment.GetFolderPath (Environment.SpecialFolder.Personal),
			"geekList.xml");

		public List<Task> TaskCollection { 
			get { return tasks; } 
			set { tasks = value; }
		}
		public DateTime? LastSyncDate { get; set; }

		public TaskList () {
			tasks = new List<Task> ();
			serializer = new XmlSerializer (typeof(TaskList));
		}

		public void Persist() {
			using (var file = new StreamWriter(dbPath, false)) {
				serializer.Serialize (file, this);
			}
		}

		public void Load() {
			if (!File.Exists (dbPath))
				return;
			using (var file = new StreamReader(dbPath)) {
				var deserialized = (TaskList) serializer.Deserialize (file);
				this.tasks = deserialized.tasks;
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{/*
			db.Close ();*/
		}

		#endregion
	}
}

