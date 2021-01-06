using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.GameServices.AD.Unity;

namespace VavilichevGD.GameServices.AD {
	public sealed class ADSService : GameServiceBase {

		#region CONSTANTS

		private const bool SUCCESS = true;
		private const bool FAIL = false;

		#endregion

		#region EVENTS

		public override event Action OnInitializedEvent;
		public event Action OnVideoADStartedEvent {
			add => behavior.OnVideoADStartedEvent += value;
			remove => behavior.OnVideoADStartedEvent -= value;
		}
		public event Action OnRewardedVideoStartedEvent{
			add => behavior.OnRewardedVideoStartedEvent += value;
			remove => behavior.OnRewardedVideoStartedEvent -= value;
		}
		public event Action OnInterstitialStartedEvent{
			add => behavior.OnInterstitialStartedEvent += value;
			remove => behavior.OnInterstitialStartedEvent -= value;
		}
		public event Action OnVideoADFinishedEvent{
			add => behavior.OnVideoADFinishedEvent += value;
			remove => behavior.OnVideoADFinishedEvent -= value;
		}
		public event Action OnRewardedVideoFinishedEvent{
			add => behavior.OnRewardedVideoFinishedEvent += value;
			remove => behavior.OnRewardedVideoFinishedEvent -= value;
		}
		public event Action OnInterstitialFinishedEvent{
			add => behavior.OnInterstitialFinishedEvent += value;
			remove => behavior.OnInterstitialFinishedEvent -= value;
		} 

		#endregion
		
		public bool isInterstitialAndBannerActive { get; private set; } = true;
		
		private ADSBehavior behavior;


		protected override IEnumerator InitializeAsyncRoutine() {
			this.CreateBehavior();
			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
			yield break;
		}
		

		private void CreateBehavior() {
			this.behavior = new ADSBehaviorUnity();
			this.PrintLog($"ADS SERVICE: Behavior created ({this.behavior.GetType().Name})");
			
			this.behavior.Initialize();
		}

		private void InitFasade() {
			ADS.Initialize(this);
			this.PrintLog($"ADS SERVICE: Fasade Initialized (ADS.cs)");
		}
		

		public void ShowRewardedVideo(Action<bool> callback) {
			this.PrintLog($"ADS SERVICE: Try to show Rewarded Video)");
			this.behavior.ShowRewardedVideo(callback);
		}

		public void ShowInterstitial(Action<bool> callback = null) {
			this.PrintLog($"ADS SERVICE: Try to show Interstitial (isEnabled = {this.isInterstitialAndBannerActive}))");

			if (this.isInterstitialAndBannerActive) {
				this.behavior.ShowInterstitial(callback);
				return;
			}
			
			callback?.Invoke(FAIL);
		}

		public void ShowBanner(BannerPosition position) {
			this.PrintLog($"ADS SERVICE: Show Banner");
			if (this.isInterstitialAndBannerActive)
				this.behavior.ShowBanner(position);
		}

		public void HideBanner() {
			this.PrintLog($"ADS SERVICE: Hide Banner");
			this.behavior.HideBanner();
		}

		public bool IsRewardedVideoAvailable() {
			return this.behavior.IsRewardedVideoAvailable();
		}

		public bool IsInterstitialAvailable() {
			if (!this.isInterstitialAndBannerActive)
				return false;
			return this.behavior.IsInterstitialAvailable();
		}

		public bool IsBannerAvailable() {
			if (!this.isInterstitialAndBannerActive)
				return false;
			return this.behavior.IsBannerAvailable();
		}

		public void DeactivateInterstitialAndBanner() {
			this.isInterstitialAndBannerActive = false;
			this.HideBanner();
			this.PrintLog($"ADS SERVICE: Interstitial and banner deactivated");
		}

		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}

	}
}