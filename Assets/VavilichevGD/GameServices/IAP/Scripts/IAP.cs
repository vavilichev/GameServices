using System;

namespace VavilichevGD.GameServices.Purchasing {
	/// <summary>
	/// Fasade of IAPService
	/// </summary>
	public static class IAP {

		#region EVENTS

		public static event Action OnIAPServiceInitializedEvent;
		public static event Action<Product, bool> OnProductPurchasedEvent {
			add => service.OnProductPurchasedEvent += value;
			remove => service.OnProductPurchasedEvent -= value;
		}

		#endregion

		public static bool isInitialized { get; private set; }
		
		public static IAPService service { get; private set; }


		public static void Initialize(IAPService _service) {
			service = _service;
			isInitialized = true;
			OnIAPServiceInitializedEvent?.Invoke();
		}



		public static void Purchase(Product product, Action<Product, bool> callback) {
			CheckStatus();
			service.Purchase(product, callback);
		}
		
		public static bool IsPurchased(Product product) {
			CheckStatus();
			return service.IsPurchased(product);
		}

		public static string GetLocalizedPrice(string productId) {
			CheckStatus();
			return service.GetLocalizedPrice(productId);
		}
		
		
		private static void CheckStatus() {
			if (!isInitialized)
				throw new Exception("IAP is not initialized yet");
		}

	}
}