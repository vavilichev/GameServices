using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.GameServices.AnalyticService {
	public abstract class AnalyticsBehavior {
		public abstract Coroutine Initialize();

		public abstract void Log(string eventName);
		public abstract void Log(string eventName, Dictionary<string, object> parameters);
	}
}