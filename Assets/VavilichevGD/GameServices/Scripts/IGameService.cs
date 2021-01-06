using System;
using UnityEngine;

namespace VavilichevGD.GameServices {
	public interface IGameService {

		#region EVENTS

		event Action OnInitializedEvent; 

		#endregion
		
		bool isLoggingEnabled { get; }
		
		Coroutine InitializeAsync();
	}
}