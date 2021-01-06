using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;
using Object = UnityEngine.Object;

namespace VavilichevGD.GameServices.Time {
	public class GameTimeService : GameServiceBase {

		#region CONSTANTS

		private const string PATH_TIME_HELPER = "[GAME TIME HELPER]";

		#endregion
		
		#region EVENTS

		public override event Action OnInitializedEvent;
		public event Action OnSecondTickEvent;
		public event Action OnPausedEvent;
		public event Action OnUnpausedEvent; 

		#endregion
		
		
		public double timeBetweenSessionsSeconds { get; private set; }
		public float timeSinceGameStarted { get; private set; } = 1f;

		public GameSessionTimeData currentSessionTimeData { get; private set; }
		public GameSessionTimeData lastSessionTimeData { get; private set; }
		
		public DateTime firstPlayTimeDevice { get; private set; }
		public DateTime nowDevice => this.GetNowDateTimeDevice();
		public DateTime nowPT => this.GetNowDateTimePT();

		public bool isPaused { get; private set; }
		
		public float deltaTime => this.timeMonoHelper.deltaTime;
		public float unscaledDeltaTime => this.timeMonoHelper.unscaledDeltaTime;
		
		
		private GameTimeMonoHelper timeMonoHelper;
		private float timeScaleBeforePaused;



		public GameTimeService(GameSessionTimeData lastSessionTimeData) {
			this.lastSessionTimeData = lastSessionTimeData;
			this.PrintLog($"GAME TIME SERVICE: Last session loaded (isValid: {this.lastSessionTimeData.isValid}): {this.lastSessionTimeData}");
		}

		public GameTimeService() { }


		#region INITIALIZATION


		protected override IEnumerator InitializeAsyncRoutine() {
			this.currentSessionTimeData = new GameSessionTimeData();
			this.LoadLastSessionTimeData();
			this.LoadFirstPlayTimeData();
			this.CreateHelper();
			yield return Coroutines.StartRoutine(this.LoadTimeNow());
			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
		}

		private void LoadLastSessionTimeData() {
			if (this.lastSessionTimeData != null)
				return;
			
			this.lastSessionTimeData = new GameSessionTimeData();
			this.lastSessionTimeData.isValid = false;
			this.PrintLog($"GAME TIME SERVICE: Last session loaded (isValid: {this.lastSessionTimeData.isValid}): {this.lastSessionTimeData}");
		}

		private void LoadFirstPlayTimeData() {
			const string KEY = "FIRST_PLAY_TIME";
			if (!PlayerPrefs.HasKey(KEY))
				PlayerPrefs.SetString(KEY, DateTime.Now.ToString());
			
			var loadedFPTString = PlayerPrefs.GetString(KEY);
			var loadedFPT = DateTime.Parse(loadedFPTString);
			this.firstPlayTimeDevice = loadedFPT;
			
			this.PrintLog($"GAME TIME SERVICE: First play date time loaded: {this.firstPlayTimeDevice}");
		}


		#region LOAD CURRENT SESSION DATA

		private IEnumerator LoadTimeNow() {
			var timeLoader = new TimeLoader();
			timeLoader.OnTimeDownloadedEvent += TimeLoader_OnTimeDownloaded;
			yield return timeLoader.LoadTime();
		}

		private void TimeLoader_OnTimeDownloaded(TimeLoader timeLoader, DownloadedTimeArgs e) {
			timeLoader.OnTimeDownloadedEvent -= this.TimeLoader_OnTimeDownloaded;
			this.PrintLog($"GAME TIME SERVICE: Time loaded from server => {e.loadedFromServer} Value: {e.downloadedTime}");
			
			this.InitGameTimeSessionCurrent(e.downloadedTime);
			this.CalculateTimePassedSinceLastSession(this.lastSessionTimeData, this.currentSessionTimeData);
		}

		private void InitGameTimeSessionCurrent(DateTime downloadedTime) {
			this.currentSessionTimeData.sessionStartSerializedFromServer.value = downloadedTime;
			var deviceTime = DateTime.Now.ToUniversalTime();
			this.currentSessionTimeData.sessionStartSerializedFromDevice.value = deviceTime;
			this.currentSessionTimeData.timeValueActiveDeviceAtStart = this.GetDeviceWorkTimeInSeconds();
			this.currentSessionTimeData.isValid = true;
		}

		private long GetDeviceWorkTimeInSeconds() {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass systemClock = new AndroidJavaClass("android.os.SystemClock");
			return Mathf.FloorToInt(systemClock.CallStatic<long>("elapsedRealtime") / 1000f);
#elif UNITY_IOS && !UNITY_EDITOR
			return IOSTime.GetSystemUpTime() / 1000L;
#else
			var deviceRunTimeTicks = Environment.TickCount & Int32.MaxValue;
			var totalSeconds = Mathf.FloorToInt(deviceRunTimeTicks / 1000f);
			return totalSeconds;
#endif
		}

		private void CalculateTimePassedSinceLastSession(GameSessionTimeData timeDataLastSession,
			GameSessionTimeData timeDataCurrentSession) {
			if (timeDataLastSession == null) {
				this.timeBetweenSessionsSeconds = 0;
				return;
			}

			this.timeBetweenSessionsSeconds =
				timeDataCurrentSession.timeValueActiveDeviceAtStart - timeDataLastSession.timeValueActiveDeviceAtEnd;
			if (this.timeBetweenSessionsSeconds < 0f) {
				this.timeBetweenSessionsSeconds = Mathf.FloorToInt(
					(long) (timeDataCurrentSession.sessionStart - timeDataLastSession.sessionOver).TotalSeconds);
				this.timeBetweenSessionsSeconds =
					Mathf.Max((long) this.timeBetweenSessionsSeconds, 0L);
			}
			
			this.PrintLog($"GAME TIME SERVICE: Time between sessions calculated => {this.timeBetweenSessionsSeconds}");
		}

		#endregion

		private void CreateHelper() {
			var prefab = Resources.Load<GameTimeMonoHelper>(PATH_TIME_HELPER);
			this.timeMonoHelper = Object.Instantiate(prefab);
			this.timeMonoHelper.name = prefab.name;
			this.timeMonoHelper.OnSecondTickEvent += this.TimeMonoHelperOnSecondTick;
			this.PrintLog($"GAME TIME SERVICE: Helper created => {this.timeMonoHelper.name}");
		}

		private void InitFasade() {
			GameTime.Initialize(this);
			this.PrintLog($"GAME TIME SERVICE: Fasade Initialized (GameTime.cs)");
		}

		#endregion



		#region LOCAL METHODS

		private DateTime GetNowDateTimeDevice()
		{
			return DateTime.Now;
		}

		private DateTime GetNowDateTimePT() {
			if (this.currentSessionTimeData.timeReceivedFromServer) {
				var gameStartTime = this.currentSessionTimeData.sessionStartServer;
				var currentTime = gameStartTime.AddSeconds(this.timeSinceGameStarted);
				return currentTime;
			}

			var now = this.nowDevice;
			return now.ToUniversalTime();
		}

		#endregion


		
		public void Pause() {
			this.timeScaleBeforePaused = UnityEngine.Time.timeScale;
			UnityEngine.Time.timeScale = 0f;
			this.isPaused = true;
			this.OnPausedEvent?.Invoke();
		}

		public void Unpause() {
			UnityEngine.Time.timeScale = this.timeScaleBeforePaused;
			this.isPaused = false;
			this.OnUnpausedEvent?.Invoke();
		}


		#region EVENTS

		private void TimeMonoHelperOnSecondTick() {
			this.timeSinceGameStarted++;
			this.OnSecondTickEvent?.Invoke();
		}

		#endregion


		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}
	}
}