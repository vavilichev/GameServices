using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Tools;

namespace VavilichevGD.GameServices.Time {
	public sealed class TimeLoader {

		#region CONSTANTS

		private const bool LOADED_FROM_LOCAL = false;
		private const bool LOADED_FROM_INTERNET = true;
		private const int BREAK_TIME_DEFAULT = 2;
		private const string SERVER_URL = "https://www.microsoft.com";

		#endregion

		#region EVENTS

		public event Action<TimeLoader, DownloadedTimeArgs> OnTimeDownloadedEvent;

		#endregion
		
		public bool isLoading { get; private set; }

		

		public Coroutine LoadTime(int breakTime = BREAK_TIME_DEFAULT) {
			if (!this.isLoading)
				return Coroutines.StartRoutine(this.LoadTimeRoutine(breakTime));
			
			throw new Exception("TimeLoader is busy.");
		}

		private IEnumerator LoadTimeRoutine(int breakTime) {
			this.isLoading = true;

			var request = new UnityWebRequest(SERVER_URL);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.timeout = breakTime;

			yield return request.SendWebRequest();
			if (!this.ValidResponse(request)) {
				this.isLoading = false;
				yield break;
			}

			var todaysDates = request.GetResponseHeaders()["date"];
			var downloadedTime = DateTime.ParseExact(todaysDates,
									   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
									   CultureInfo.InvariantCulture.DateTimeFormat,
									   DateTimeStyles.AdjustToUniversal);

			this.NotifyAboutDownloadedTime(downloadedTime, false, null, LOADED_FROM_INTERNET);
			this.isLoading = false;
		}

		private bool ValidResponse(UnityWebRequest request) {
			var errorText = "";

			if (request.isNetworkError)
				errorText = $"Downloading time stopped: {request.error}";
			else if (request.downloadHandler == null)
				errorText = $"Downloading time stopped: DownloadHandler is NULL";
			else if (string.IsNullOrEmpty(request.downloadHandler.text))
				errorText = $"Downloading time stopped: Downloaded string is empty or NULL";

			
			if (string.IsNullOrEmpty(errorText))
				return true;

			this.NotifyAboutDownloadedTime(new DateTime(), true, errorText, LOADED_FROM_LOCAL);
			return false;
		}

		private void NotifyAboutDownloadedTime(DateTime downloadedTime, bool error, string errorText, bool downloadedFromServer) {
			var downloadedTimeArgs = new DownloadedTimeArgs(downloadedTime, error, errorText, downloadedFromServer);
			this.OnTimeDownloadedEvent?.Invoke(this, downloadedTimeArgs);
		}
	}
}