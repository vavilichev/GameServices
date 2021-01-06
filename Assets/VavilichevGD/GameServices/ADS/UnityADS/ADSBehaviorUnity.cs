using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using VavilichevGD.Tools;

namespace VavilichevGD.GameServices.AD.Unity {
	public sealed class ADSBehaviorUnity : ADSBehavior {


		#region CONSTANTS

		private const string PATH_SETTINGS = "ADSSettingsUnity";
		private const string REWARDED_VIDEO = "rewardedVideo";
		private const string INTERSTITIAL = "video";
		private const string BANNER = "banner";

		#endregion

		#region EVENTS

		public override event Action OnVideoADStartedEvent;
		public override event Action OnRewardedVideoStartedEvent;
		public override event Action OnInterstitialStartedEvent;
		public override event Action OnVideoADFinishedEvent;
		public override event Action OnRewardedVideoFinishedEvent;
		public override event Action OnInterstitialFinishedEvent;

		#endregion

		private bool rewardedVideoIsWorking;
		private bool interstitialVideoIsWorking;

		private ADSSettingsUnity settings;


		

		public override void Initialize() {
			this.settings = Resources.Load<ADSSettingsUnity>(PATH_SETTINGS);

			Advertisement.debugMode = settings.testMode;
			Advertisement.Initialize(settings.appId, settings.testMode);
		}



		
		#region REWARDED VIDEO

		public override void ShowRewardedVideo(Action<bool> callback) {
			Coroutines.StartRoutine(this.ShowRewardedVideoRoutine(callback));
		}

		private IEnumerator ShowRewardedVideoRoutine(Action<bool> callback) {
			if (this.rewardedVideoIsWorking) {
				Debug.LogWarning("AD process is working now (rewarded video)");
				callback?.Invoke(FAIL);
				yield break;
			}

			this.rewardedVideoIsWorking = true;
#if !UNITY_EDITOR
			var timer = 0f;
			while (!Advertisement.IsReady(REWARDED_VIDEO)) {
				yield return null;
				timer += Time.unscaledDeltaTime;
				if (timer >= settings.breakTime) {
					this.rewardedVideoIsWorking = false;
					Debug.LogWarning($"Not loaded during a BREAK_TIME ({settings.breakTime} sec (rewardedVideo)");
					callback?.Invoke(FAIL);
					yield break;
				}
			}
#endif

			void RewardedVideoCallback(ShowResult result) {
				this.rewardedVideoIsWorking = false;
				switch (result) {
					case ShowResult.Finished:
						callback?.Invoke(SUCCESS);
						break;
					case ShowResult.Failed:
						callback?.Invoke(FAIL);
						break;
				}
				
				this.OnRewardedVideoFinishedEvent?.Invoke();
				this.OnVideoADFinishedEvent?.Invoke();
			}

			var options = new ShowOptions {resultCallback = RewardedVideoCallback};
			Advertisement.Show(REWARDED_VIDEO, options);
			
			this.OnRewardedVideoStartedEvent?.Invoke();
			this.OnVideoADStartedEvent?.Invoke();
		}

		#endregion



		#region INTERSTITIAL

		public override void ShowInterstitial(Action<bool> callback = null) {
			Coroutines.StartRoutine(this.ShowInterstitialRoutine(callback));
		}

		private IEnumerator ShowInterstitialRoutine(Action<bool> callback) {
			if (this.interstitialVideoIsWorking) {
				Debug.LogWarning("AD process is working now (interstitial)");
				callback?.Invoke(FAIL);
				yield break;
			}

			this.interstitialVideoIsWorking = true;
			if (!Advertisement.IsReady(INTERSTITIAL)) {
				this.interstitialVideoIsWorking = false;
				Debug.LogWarning("AD is not loaded yet now (interstitial)");
				callback?.Invoke(FAIL);
				yield break;
			}

			void InterstitialCallback(ShowResult result) {
				this.rewardedVideoIsWorking = false;
				switch (result) {
					case ShowResult.Failed:
						callback?.Invoke(FAIL);
						break;
					default:
						callback?.Invoke(SUCCESS);
						break;
				}

				this.interstitialVideoIsWorking = false;
				this.OnInterstitialFinishedEvent?.Invoke();
				this.OnVideoADFinishedEvent?.Invoke();
			}

			var options = new ShowOptions {resultCallback = InterstitialCallback};
			Advertisement.Show(options);
			
			this.OnInterstitialStartedEvent?.Invoke();
			this.OnVideoADStartedEvent?.Invoke();
		}

		#endregion



		#region BANNER

		public override void ShowBanner(BannerPosition position) {
			if (Advertisement.Banner.isLoaded || Advertisement.IsReady(BANNER)) {
				Advertisement.Banner.SetPosition(this.ConvertBannerPosition(position));
				Advertisement.Banner.Show(BANNER);
			}
		}

		private UnityEngine.Advertisements.BannerPosition ConvertBannerPosition(VavilichevGD.GameServices.AD.BannerPosition position) {
			switch (position) {
				case BannerPosition.TOP_LEFT:
					return UnityEngine.Advertisements.BannerPosition.TOP_LEFT;
				case BannerPosition.TOP_CENTER:
					return UnityEngine.Advertisements.BannerPosition.TOP_CENTER;
				case BannerPosition.TOP_RIGHT:
					return UnityEngine.Advertisements.BannerPosition.TOP_RIGHT;
				case BannerPosition.BOTTOM_LEFT:
					return UnityEngine.Advertisements.BannerPosition.BOTTOM_LEFT;
				case BannerPosition.BOTTOM_CENTER:
					return UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER;
				case BannerPosition.BOTTOM_RIGHT:
					return UnityEngine.Advertisements.BannerPosition.BOTTOM_RIGHT;
				default:
					return UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER;
			}
		}

		public override void HideBanner() {
			Advertisement.Banner.Hide();
		}

		#endregion



		public override bool IsRewardedVideoAvailable() {
			return !this.rewardedVideoIsWorking && Advertisement.IsReady(REWARDED_VIDEO);
		}

		public override bool IsInterstitialAvailable() {
			return !this.interstitialVideoIsWorking && Advertisement.IsReady(INTERSTITIAL);
		}

		public override bool IsBannerAvailable() {
			return Advertisement.Banner.isLoaded || Advertisement.IsReady(BANNER);
		}
		
	}
}