using System;
using System.Net;
using System.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace GeekList
{
	public class ServerProxy
	{
		private const string login = "miloszpp@gmail.com";
		private const string pass = "753dfx";
		private const string serverUrl = "http://localhost:3000/api/";

		private string userId;
		private string loginToken;

		public ServerProxy ()
		{
		}

		public async Task<DateTime?> GetServerSyncDate() {
			if (!await LoginIfRequired ())
				return null;

			var result = await CallService ("get_sync_date", new Dictionary<string, string> {
				{ "loginToken", loginToken },
				{ "userId", userId }
			});
			if (string.IsNullOrEmpty (result ["sync_date"]))
				return null;
			return DateTime.Parse (result ["sync_date"]);
		}

		public async Task<bool> UploadTasks(TaskList tasks) {
			if (!await LoginIfRequired())
				return false;

			var tasksArray = tasks.TaskCollection.Select (t => {
				var list = new List<KeyValuePair<string, JsonValue>>();
				list.Add(new KeyValuePair<string, JsonValue>("description", new JsonPrimitive(t.Description)));
				list.Add(new KeyValuePair<string, JsonValue>("due", t.Due.HasValue ? new JsonPrimitive(t.Due.Value.ToString()) : null));
				list.Add(new KeyValuePair<string, JsonValue>("completed", new JsonPrimitive(t.Completed)));
				list.Add(new KeyValuePair<string, JsonValue>("priority", t.Priority.HasValue ? new JsonPrimitive(t.Priority.Value) : null));
				return new JsonObject(list);
			}).ToArray();
			var tasksJsonArray = new JsonArray (tasksArray);
			var jsonString = tasksJsonArray.ToString ();
			await CallService ("receive_tasks", new Dictionary<string, string> () {
				{ "tasks", jsonString },
				{ "loginToken", loginToken },
				{ "userId", userId }
			});
			return true;
		}

		public List<Task> GetTasks() {
			return null;
//			CallService ("get_tasks", new Dictionary<string, string> () {
//				{ "loginToken", loginToken },
//				{ "userId", userId }
//			});
		}

		private async Task<bool> LoginIfRequired() {
			if (userId != null) return true;
			var loginData = await CallService ("login", new Dictionary<string, string> {
				{ "user", login },
				{ "password", pass }
			});
			if (loginData ["success"]) {
				loginToken = loginData ["loginToken"];
				userId = loginData ["userId"];
				return true;
			}
			return false;
		}

		private async static Task<JsonObject> CallService(string urlPart, Dictionary<string, string> postParams) {
			using (var wb = new WebClient())
			{
				var data = new NameValueCollection();
				foreach (var key in postParams.Keys) {
					data [key] = postParams [key];
				}
				wb.Headers.Add("Content-Type", "application/x-www-form-urlencoded"); // bug in Xamarin impl
				var response = await wb.UploadValuesTaskAsync(new Uri(serverUrl + urlPart), "POST", data);
				return (JsonObject) JsonObject.Load(new MemoryStream(response));
			}
		}

	}
}

