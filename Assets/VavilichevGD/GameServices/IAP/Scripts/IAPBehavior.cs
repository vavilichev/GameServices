using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.GameServices.Purchasing {
	public abstract class IAPBehavior {

		#region CONSTANTS

		protected const bool SUCCESS = true;
		protected const bool FAIL = false;

		#endregion

		public abstract bool isInitialized { get; }
		
		public IAPBehavior(Product[] iapProducts) { }

		public Coroutine InitializeAsync() {
			return Coroutines.StartRoutine(this.InitializeAsyncRoutine());
		}

		protected abstract IEnumerator InitializeAsyncRoutine();


		public abstract void Purchase(Product iapProduct, Action<Product, bool> callback);
		public abstract bool IsPurchased(Product iapProduct);
		public abstract string GetLocalizedPrice(string productId);

	}
}