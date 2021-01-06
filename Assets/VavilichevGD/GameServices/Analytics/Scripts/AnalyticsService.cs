using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.GameServices.AnalyticService {
	public sealed class AnalyticsService : GameServiceBase {

		#region EVENTS

		public override event Action OnInitializedEvent;

		#endregion

		private List<AnalyticsBehavior> behaviors;


		#region INITIALIZE


		protected override IEnumerator InitializeAsyncRoutine() {
			this.CreateBehaviors();
			
			foreach (var behavior in this.behaviors)
				yield return behavior.Initialize();
			
			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
		}

		private void CreateBehaviors() {
			this.behaviors = new List<AnalyticsBehavior>();
			
			// Add your services (Game Analytics, AppMetrica, FireBase etc...)
			this.behaviors.Add(new AnalyticsBehaviorUnity());
			
			this.LogCreatedBehaviors();
		}

		private void LogCreatedBehaviors() {
			if (!this.isLoggingEnabled)
				return;

			foreach (var behavior in this.behaviors)
				this.PrintLog($"ANALYTICS SERVICE: behavior created: {behavior.GetType().Name}");
		}

		private void InitFasade() {
			Analytics.Initialize(this);
			this.PrintLog($"ANALYTICS SERVICE: Fasade Initialized (Analytics.cs)");
		}

		#endregion
		

		
		public void Log(string eventName) {
			foreach (var behavior in this.behaviors) 
				behavior.Log(eventName);

			this.PrintLog(eventName, null);
		}

		public void Log(string eventName, Dictionary<string, object> parameters) {
			foreach (var behavior in this.behaviors) 
				behavior.Log(eventName, parameters);

			this.PrintLog(eventName, parameters);
		}

		
		
		private void PrintLog(string eventName, Dictionary<string, object> parameters = null) {
			if (!this.isLoggingEnabled)
				return;
			
			var parametersText = "";
			if (parameters != null) {
				parametersText = "\nParameters:\n";
				foreach (var pair in parameters) 
					parametersText += $"( {pair.Key} : {pair.Value} )\n";
			}

			var finalLog = $"ANALYTICS EVENT SENT: Name = {eventName}. {parametersText}";
			Debug.Log(finalLog);
		}

		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}
		
	}
}