using UnityEditor;
using VavilichevGD.GameServices.AD.Unity;
using VavilichevGD.Utils.Editor;

namespace VavilichevGD.GameServices.AD.Editor {
    public static class ADSSettingsEditor {

        // [MenuItem("VavilichevGD/GameServices/ADS/SetupAdMob")]
        // public static void SetupSettingsADSAdMob() {
        //     var path = "Assets/VavilichevGD/GameServices/ADS/Resources/ADSSettingsAdMob.asset";
        //     LoadOrCreateAsset<ADSSettingsAdMob>(path);
        // }

        
        [MenuItem("VavilichevGD/GameServices/ADS/SetupUnity")]
        public static void SetupSettingsADSUnity() {
            var path = "Assets/VavilichevGD/GameServices/ADS/Resources/ADSSettingsUnity.asset";
            EditorExtensions.LoadOrCreateScriptableObject<ADSSettingsUnity>(path);
        }
        
        // [MenuItem("VavilichevGD/GameServices/ADS/SetupAppLovin")]
        // public static void SetupSettingsADSAppLovin() {
        //     var path = "Assets/VavilichevGD/GameServices/ADS/Resources/ADSSettingsAppLovin.asset";
        //     LoadOrCreateAsset<ADSSettingsAppLovin>(path);
        // }
        
   
    }
}