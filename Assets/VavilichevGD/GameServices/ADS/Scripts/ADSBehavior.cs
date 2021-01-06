using System;

namespace VavilichevGD.GameServices.AD {

	public enum BannerPosition {
		TOP_LEFT,
		TOP_CENTER,
		TOP_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_CENTER,
		BOTTOM_RIGHT
	}
	
	public abstract class ADSBehavior {

		#region CONSTANTS

		protected const bool SUCCESS = true;
		protected const bool FAIL = false;

		#endregion
		
		#region EVENTS

		public abstract event Action OnVideoADStartedEvent;
		public abstract event Action OnRewardedVideoStartedEvent;
		public abstract event Action OnInterstitialStartedEvent;
		public abstract event Action OnVideoADFinishedEvent;
		public abstract event Action OnRewardedVideoFinishedEvent;
		public abstract event Action OnInterstitialFinishedEvent; 

		#endregion

		public abstract void Initialize();
		public abstract void ShowRewardedVideo(Action<bool> callback);
		public abstract void ShowInterstitial(Action<bool> callback = null);
		public abstract void ShowBanner(BannerPosition position = BannerPosition.BOTTOM_CENTER);
		public abstract void HideBanner();
		public abstract bool IsRewardedVideoAvailable();
		public abstract bool IsInterstitialAvailable();
		public abstract bool IsBannerAvailable();
	}
}