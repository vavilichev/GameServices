using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.AD.Example {
    public class ADSServiceExample : MonoBehaviour {

        [SerializeField] private Button buttonInterstitial;
        [SerializeField] private Button buttonRewardedVideo;
        [SerializeField] private Button buttonBanner;
        [SerializeField] private Text textLogging;

        private ADSService adService;

        private bool isBannerEnabled;
        
        private void OnEnable() {
            this.buttonInterstitial.onClick.AddListener(this.OnInterstitialButtonClicked);
            this.buttonRewardedVideo.onClick.AddListener(this.OnRewardedVideoButtonClicked);
            this.buttonBanner.onClick.AddListener(this.OnBannerButtonClicked);
        }

        private void OnDisable() {
            this.buttonInterstitial.onClick.RemoveListener(this.OnInterstitialButtonClicked);
            this.buttonRewardedVideo.onClick.RemoveListener(this.OnRewardedVideoButtonClicked);
            this.buttonBanner.onClick.RemoveListener(this.OnBannerButtonClicked);
        }

        private void Start() {
            this.adService = new ADSService();
            this.adService.isLoggingEnabled = true;
            this.adService.InitializeAsync();
        }


        private void Log(string text) {
            this.textLogging.text = text;
        }
        
        
        #region EVENTS

        private void OnInterstitialButtonClicked() {
            if (!ADS.isInitialized) {
                this.Log("ADS Service is not initialized yet");
                return;
            }

            void InterstitialCallback(bool success) {
                this.Log($"INTERSTITIAL - Shown successfully");
            }
            
            ADS.ShowInterstitial(InterstitialCallback);
        }

        private void OnRewardedVideoButtonClicked() {
            if (!ADS.isInitialized) {
                this.Log("ADS Service is not initialized yet");
                return;
            }

            void RewardedVideoCallback(bool success) {
                this.Log($"REWARDED - Fully watched: {success}");
            }
            
            ADS.ShowRewardedVideo(RewardedVideoCallback);
        }

        private void OnBannerButtonClicked() {
            if (!ADS.isInitialized) {
                this.Log("ADS Service is not initialized yet");
                return;
            }

            if (!this.isBannerEnabled) {
                ADS.ShowBanner();
                this.isBannerEnabled = true;
                this.Log("BANNER - Shown");
            }
            else {
                ADS.HideBanner();
                this.isBannerEnabled = false;
                this.Log("BANNER - Hidden");
            }
        }

        #endregion
    }
}