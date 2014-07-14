using System;
using System.Net;
using System.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace GeekList
{
	public class SyncManager
	{
		private const string login = "miloszpp@gmail.com";
		private const string pass = "753dfx";
		private const string serverUrl = "http://localhost:3000/api/";

		private string userId;
		private string loginToken;

		public SyncManager ()
		{
		}

		public DateTime GetServerSyncDate() {
			LoginIfRequired ();

			var result = CallService ("get_sync_date", new Dictionary<string, string> {
				{ "loginToken", loginToken },
				{ "userId", userId }
			});
			return DateTime.Parse (result ["sync_date"]);
		}

		private bool LoginIfRequired() {
			if (userId != null) return false;
			var loginData = CallService ("login", new Dictionary<string, string> {
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

		private static JsonObject CallService(string urlPart, Dictionary<string, string> postParams) {
			using (var wb = new WebClient())
			{
				var data = new NameValueCollection();
				foreach (var key in postParams.Keys) {
					data [key] = postParams [key];
				}
				var response = wb.UploadValues(serverUrl + urlPart, data);
				return (JsonObject) JsonObject.Load(new MemoryStream(response));
			}
		}

	}
}

