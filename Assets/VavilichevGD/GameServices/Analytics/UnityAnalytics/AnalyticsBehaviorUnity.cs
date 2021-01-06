using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.GameServices.AnalyticService {
	public class AnalyticsBehaviorUnity : AnalyticsBehavior {
		public override Coroutine Initialize() {
			return null;
		}

		public override void Log(string eventName) {
			UnityEngine.Analytics.Analytics.CustomEvent(eventName);
		}

		public override void Log(string eventName, Dictionary<string, object> parameters) {
			UnityEngine.Analytics.Analytics.CustomEvent(eventName, parameters);
		}
	}
}