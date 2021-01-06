using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VavilichevGD.GameServices.Time {
	public class GameTimeMonoHelper : MonoBehaviour {

		#region EVENTS

		public event Action OnSecondTickEvent;

		#endregion

		
		public float deltaTime { get; private set; }
		public float unscaledDeltaTime { get; private set; }
		
		private float timer;


		private void Awake() {
			DontDestroyOnLoad(this.gameObject);
		}

		private void Update() {
			this.deltaTime = UnityEngine.Time.deltaTime;
			this.unscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
			
			this.timer += this.unscaledDeltaTime;
			
			if (this.timer > 1f) {
				this.timer = this.timer - Mathf.Floor(this.timer);
				this.OnSecondTickEvent?.Invoke();
			}
		}

		
		#if UNITY_EDITOR
		
		[InitializeOnLoadMethod]
		static void OnProjectLoadedInEditor() {

			const int executionOrder = -500;

			var tempObject = new GameObject();
			var tempHelper = tempObject.AddComponent<GameTimeMonoHelper>();
			var monoScript = MonoScript.FromMonoBehaviour(tempHelper);
			var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
			
			if (executionOrder != currentOrder) {
				MonoImporter.SetExecutionOrder(monoScript, executionOrder);
				Debug.Log("GAME TIME HELPER changed execution order to " + executionOrder);
			}

			DestroyImmediate(tempObject);
		}
		
		#endif
		
	}
}