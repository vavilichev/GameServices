using System;
using UnityEngine;

namespace VavilichevGD.GameServices.AD.Unity {
	public class ADSSettingsUnity : ScriptableObject {
		
		[Header("Android:")] 
		[SerializeField] private string m_androidAppId;

		[Header("iOS:")] 
		[SerializeField] private string m_iOSAppId;

		[Space] 
		[SerializeField] private float m_breakTime = 5f;
		[SerializeField] private bool m_testMode;

		public string appId => GetAppId();
		public bool testMode => m_testMode;
		public float breakTime => m_breakTime;


		private string GetAppId() {
#if UNITY_ANDROID
            return m_androidAppId;
#elif UNITY_IOS
            return m_iOSAppId;
#endif
			throw new PlatformNotSupportedException();
		}
	}
}