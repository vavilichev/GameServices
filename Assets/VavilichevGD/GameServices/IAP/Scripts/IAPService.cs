using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.GameServices.Purchasing.Unity;

namespace VavilichevGD.GameServices.Purchasing {
	public sealed class IAPService : GameServiceBase {

		#region EVENTS

		public event Action<Product, bool> OnProductPurchasedEvent; 
		public override event Action OnInitializedEvent;

		#endregion
		
		private IAPBehavior behavior;
		private Dictionary<string, Product> iapProductsMap;


		public IAPService(Product[] iapProducts) {
			this.iapProductsMap = new Dictionary<string, Product>();
			
			foreach (var product in iapProducts) {
				if (product.info.paymentType != PaymentType.IAP)
					continue;

				this.iapProductsMap[product.info.id] = product;
			}
		}



		protected override IEnumerator InitializeAsyncRoutine() {
			this.CreateBehavior();

			yield return behavior.InitializeAsync();

			while (!behavior.isInitialized)
				yield return null;

			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
		}

		private void CreateBehavior() {
			// You can create your own IAP behavior here
			var products = this.iapProductsMap.Values.ToArray();
			this.behavior = new IAPBehaviorUnity(this, products);
		}

		private void InitFasade() {
			IAP.Initialize(this);
			this.PrintLog($"IAP SERVICE: Fasade Initialized (IAP.cs)");
		}

		public void Purchase(Product product, Action<Product, bool> callback) {

			void LocalCallback(Product _product, bool success) {
				callback?.Invoke(_product, success);
				this.OnProductPurchasedEvent?.Invoke(_product, success);
			}
			
			this.behavior.Purchase(product, LocalCallback);
		}

		public bool IsPurchased(Product product) {
			return this.behavior.IsPurchased(product);
		}

		public string GetLocalizedPrice(string productId) {
			return this.behavior.GetLocalizedPrice(productId);
		}

		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}
		
	}
}