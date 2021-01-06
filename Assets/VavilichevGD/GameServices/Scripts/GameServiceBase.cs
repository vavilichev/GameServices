using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.GameServices {
	public abstract class GameServiceBase : IGameService{

		#region EVENTS

		public abstract event Action OnInitializedEvent;

		#endregion
		public bool isLoggingEnabled { get; set; }

		public Coroutine InitializeAsync() {
			return Coroutines.StartRoutine(this.InitializeAsyncRoutine());
		}

		protected abstract IEnumerator InitializeAsyncRoutine();
	}
}