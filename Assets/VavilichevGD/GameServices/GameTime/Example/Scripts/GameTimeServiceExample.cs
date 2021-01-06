using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.Time {
	public class GameTimeServiceExample : MonoBehaviour {

		[SerializeField] private Text textFPT;
		[SerializeField] private Text textLastSessionTimeStartTime;
		[SerializeField] private Text textCurrentSessionTimeStartTime;
		[SerializeField] private Text textTimeNowPT;
		[SerializeField] private Text textTimeNowDevice;
		
		private GameTimeService gameTimeService;

		#region LIFECYCLE

		private void Start() {
			this.CreateGameTimeService();
			
			if (!GameTime.isInitialized)
				GameTime.OnGameTimeServiceInitializedEvent += this.OnGameTimeServiceInitialized;
			else
				this.Setup();
		}
		
		private void CreateGameTimeService() {
			this.gameTimeService = new GameTimeService();
			this.gameTimeService.isLoggingEnabled = true;
			this.gameTimeService.InitializeAsync();
		}

		private void OnDestroy() {
			GameTime.OnSecondTickEvent -= this.OnSecondTick;
		}

		#endregion
		
		
		private void Setup() {
			this.textFPT.text = GameTime.firstPlayTimeDevice.ToString() + "\n(device)";

			this.UpdateStartTime(this.textLastSessionTimeStartTime, GameTime.lastSessionTimeData);
			this.UpdateStartTime(this.textCurrentSessionTimeStartTime, GameTime.currentSessionTimeData);
			this.UpdateTimeNow();
			
			GameTime.OnSecondTickEvent += this.OnSecondTick;
		}

		private void UpdateStartTime(Text textField, GameSessionTimeData sessionTimeData) {
			if (sessionTimeData.isValid)
				textField.text = sessionTimeData.timeReceivedFromServer
					? sessionTimeData.sessionStartServer + "\n(server PT)"
					: sessionTimeData.sessionStartDevice + "\n(device)";
			else
				textField.text = "NOT VALID";
		}

		private void UpdateTimeNow() {
			this.textTimeNowPT.text = GameTime.nowPT.ToString();
			this.textTimeNowDevice.text = GameTime.nowDevice.ToString();
		}

		

		#region EVENTS

		private void OnGameTimeServiceInitialized() {
			GameTime.OnGameTimeServiceInitializedEvent -= this.OnGameTimeServiceInitialized;
			this.Setup();
		}
		
		private void OnSecondTick() {
			this.UpdateTimeNow();
		}

		#endregion
	}
}