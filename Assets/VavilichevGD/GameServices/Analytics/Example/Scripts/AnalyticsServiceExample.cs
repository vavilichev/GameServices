using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.AnalyticService.Example {
	public class AnalyticsServiceExample : MonoBehaviour {

		[SerializeField] private Button buttonEventNoParameters;
		[SerializeField] private Button buttonEventWithParameters;
		
		private AnalyticsService analyticsService;

		
		
		#region LIFECYCLE

		private void OnEnable() {
			this.buttonEventNoParameters.onClick.AddListener(this.OnNoParametersButtonClick);
			this.buttonEventWithParameters.onClick.AddListener(this.OnWithParametersButtonClick);
		}

		private void OnDisable() {
			this.buttonEventNoParameters.onClick.RemoveListener(this.OnNoParametersButtonClick);
			this.buttonEventWithParameters.onClick.RemoveListener(this.OnWithParametersButtonClick);
		}


		private void Start() {
			this.analyticsService = new AnalyticsService();
			this.analyticsService.isLoggingEnabled = true;
			this.analyticsService.InitializeAsync();
		}

		#endregion

		
		#region EVENTS

		private void OnNoParametersButtonClick() {
			const string eventName = "event_no_parameters";
			
			Analytics.Log(eventName);
		}

		private void OnWithParametersButtonClick() {
			const string eventName = "event_with_parameters";
			
			var parameters = new Dictionary<string, object>
			{
				{ "parameter_1", "parameter_value_1" },
				{ "parameter_2", "parameter_value_2" },
				{ "parameter_3", "parameter_value_3" },
				{ "parameter_4", "parameter_value_4" }
			};

			Analytics.Log(eventName, parameters);
		}

		#endregion

	}
}