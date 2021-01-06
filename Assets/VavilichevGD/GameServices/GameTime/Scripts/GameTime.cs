using System;

namespace VavilichevGD.GameServices.Time {
	/// <summary>
	/// Fasade of GameTimeService
	/// </summary>
	public static class GameTime {

		#region EVENTS

		public static event Action OnGameTimeServiceInitializedEvent;
		public static event Action OnSecondTickEvent;
		public static event Action OnPausedEvent;
		public static event Action OnUnpausedEvent; 

		#endregion

		public static bool isInitialized { get; private set; }
		public static GameTimeService service { get; private set; }

		public static double timeBetweenSessionsSeconds {
			get {
				CheckStatus();
				return service.timeBetweenSessionsSeconds;
			}
		} 
		
		public static float timeSinceGameStarted {
			get {
				CheckStatus();
				return service.timeSinceGameStarted;
			}
		} 
		
		public static GameSessionTimeData currentSessionTimeData {
			get {
				CheckStatus();
				return service.currentSessionTimeData;
			}
		} 
		
		public static GameSessionTimeData lastSessionTimeData {
			get {
				CheckStatus();
				return service.lastSessionTimeData;
			}
		} 
		
		public static DateTime firstPlayTimeDevice {
			get {
				CheckStatus();
				return service.firstPlayTimeDevice;
			}
		}
		
		public static DateTime nowDevice {
			get {
				CheckStatus();
				return service.nowDevice;
			}
		}
		
		public static DateTime nowPT {
			get {
				CheckStatus();
				return service.nowPT;
			}
		}
		
		public static bool isPaused {
			get {
				CheckStatus();
				return service.isPaused;
			}
		}
		
		public static float deltaTime {
			get {
				CheckStatus();
				return service.deltaTime;
			}
		}
		
		public static float unscaledDeltaTime {
			get {
				CheckStatus();
				return service.unscaledDeltaTime;
			}
		}

		
		
		public static void Initialize(GameTimeService _service) {
			service = _service;
			isInitialized = true;
			SubscribeOnEvents();
			OnGameTimeServiceInitializedEvent?.Invoke();
		}

		private static void SubscribeOnEvents() {
			service.OnSecondTickEvent += () => OnSecondTickEvent?.Invoke();
			service.OnPausedEvent += () => OnPausedEvent?.Invoke();
			service.OnUnpausedEvent += () => OnUnpausedEvent?.Invoke();
		}

		public static void Pause() {
			CheckStatus();
			service.Pause();
		}

		public static void Unpause() {
			CheckStatus();
			service.Unpause();
		}
		
		private static void CheckStatus() {
			if (!isInitialized)
				throw new Exception("GameTime is not initialized yet");
		}
			
	}
}