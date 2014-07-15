using System;

namespace GeekList
{
	public class SyncManager
	{
		private ServerProxy proxy;
		private TaskList taskList;

		public SyncManager (ServerProxy proxy, TaskList taskList)
		{
			this.proxy = proxy;
			this.taskList = taskList;
		}

		public async System.Threading.Tasks.Task Synchronize()
		{
			var serverDate = await proxy.GetServerSyncDate ();
			if (serverDate == null || taskList.LastSyncDate == null || serverDate < taskList.LastSyncDate) {
				await proxy.UploadTasks (taskList);
				taskList.LastSyncDate = DateTime.Now;

			} else if (serverDate > taskList.LastSyncDate) {
				taskList.TaskCollection = proxy.GetTasks ();
			}
		}
	}
}

