using System;

namespace VavilichevGD.GameServices.Purchasing {
	/// <summary>
	/// Fasade of Purchases Service.
	/// </summary>
	public static class Purchases {

		#region EVENTS

		public static event Action OnPurchasesServiceInitializedEvent; 

		#endregion


		public static bool isInitialized { get; private set; } = false;
		public static PurchasesService service { get; private set; }

		public static void Initialize(PurchasesService _service) {
			service = _service;

			isInitialized = true;
			OnPurchasesServiceInitializedEvent?.Invoke();
		}
		
		public static void Purchase(object sender, Product product, Action<Product, bool> callback) {
			service.Purchase(sender, product, callback);
		}
		
	}
}