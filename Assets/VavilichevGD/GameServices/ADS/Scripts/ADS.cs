using System;

namespace VavilichevGD.GameServices.AD {
	/// <summary>
	/// Fasade of ADSService
	/// </summary>
	public static class ADS {

		#region EVENTS

		public static event Action OnADSServiceInitializedEvent;
		public static event Action OnVideoADStartedEvent {
			add => service.OnVideoADStartedEvent += value;
			remove => service.OnVideoADStartedEvent -= value;
		}
		public static event Action OnRewardedVideoStartedEvent {
			add => service.OnRewardedVideoStartedEvent += value;
			remove => service.OnRewardedVideoStartedEvent -= value;
		}
		public static event Action OnInterstitialStartedEvent {
			add => service.OnInterstitialStartedEvent += value;
			remove => service.OnInterstitialStartedEvent -= value;
		}
		public static event Action OnVideoADFinishedEvent {
			add => service.OnVideoADFinishedEvent += value;
			remove => service.OnVideoADFinishedEvent -= value;
		}
		public static event Action OnRewardedVideoFinishedEvent {
			add => service.OnRewardedVideoFinishedEvent += value;
			remove => service.OnRewardedVideoFinishedEvent -= value;
		}
		public static event Action OnInterstitialFinishedEvent {
			add => service.OnInterstitialFinishedEvent += value;
			remove => service.OnInterstitialFinishedEvent -= value;
		}

		#endregion

		public static bool isInitialized { get; private set; }

		public static bool isInterstitialAndBannerActive => service != null && service.isInterstitialAndBannerActive;
		
		private static ADSService service;

		public static void Initialize(ADSService _service) {
			service = _service;
			isInitialized = true;
			OnADSServiceInitializedEvent?.Invoke();
		}

		public static void ShowRewardedVideo(Action<bool> callback) {
			CheckStatus();
			service.ShowRewardedVideo(callback);
		}

		public static void ShowInterstitial(Action<bool> callback = null) {
			CheckStatus();
			service.ShowInterstitial(callback);
		}

		public static void ShowBanner(BannerPosition position = BannerPosition.BOTTOM_CENTER) {
			CheckStatus();
			service.ShowBanner(position);
		}

		public static void HideBanner() {
			CheckStatus();
			service.HideBanner();
		}

		public static bool IsRewardedVideoAvailable() {
			CheckStatus();
			return service.IsRewardedVideoAvailable();
		}

		public static bool IsInterstitialAvailable() {
			CheckStatus();
			return service.IsInterstitialAvailable();
		}

		public static bool IsBannerAvailable() {
			CheckStatus();
			return service.IsBannerAvailable();
		}

		public static void DeactivateInterstitialAndBanner() {
			service.DeactivateInterstitialAndBanner();
		}
		
		
		private static void CheckStatus() {
			if (!isInitialized)
				throw new Exception("ADS is not initialized yet");
		}
	}
}