using System;
using System.Collections.Generic;

namespace VavilichevGD.GameServices.AnalyticService {
	/// <summary>
	/// Fasade of AnalyticsService
	/// </summary>
	public static class Analytics {

		#region EVENTS

		public static event Action OnAnalyticsServiceInitializedEvent;

		#endregion

		public static bool isInitialized { get; private set; }
		
		private static AnalyticsService service;

		public static void Initialize(AnalyticsService _service) {
			service = _service;
			isInitialized = true;
			OnAnalyticsServiceInitializedEvent?.Invoke();
		}

		public static void Log(string eventName) {
			CheckStatus();
			service.Log(eventName);
		}

		public static void Log(string eventName, Dictionary<string, object> parameters) {
			CheckStatus();
			service.Log(eventName, parameters);
		}

		private static void CheckStatus() {
			if (!isInitialized)
				throw new Exception("Analytics is not initialized yet");
		}

	}
}