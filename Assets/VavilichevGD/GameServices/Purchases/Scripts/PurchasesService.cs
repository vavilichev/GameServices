using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.GameServices.Purchasing {
	public sealed class PurchasesService : GameServiceBase {

		#region EVENTS

		public override event Action OnInitializedEvent;

		#endregion

		private IPurchaseHandler purchaseHandlerBank;
		private IPurchaseHandler purchaseHandlerADS;
		private IPurchaseHandler purchaseHandlerIAP;


		#region INITIALIZATION


		protected override IEnumerator InitializeAsyncRoutine() {
			this.CreateHandlers();
			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
			yield break;
		}

		private void CreateHandlers() {
			this.purchaseHandlerBank = new PurchaseHandlerBank();
			this.purchaseHandlerADS = new PurchaseHandlerADS();
			this.purchaseHandlerIAP = new PurchaseHandlerIAP();
		}

		private void InitFasade() {
			Purchases.Initialize(this);
			this.PrintLog($"PURCHASES SERVICE: Fasade Initialized (Purchases.cs)");
		}

		#endregion


		public void Purchase(object sender, Product product, Action<Product, bool> callback) {
			var paymentType = product.info.paymentType;
			var paymentHandler = this.GetHandler(paymentType);
			
			this.PrintLog(
				$"Product ({product.info.id}) payment type defined as " +
				$"{product.info.paymentType.ToString()}. Try to use handler " +
				$"{paymentHandler.GetType().Name}. Initiator is: {sender.GetType().Name}");
			
			paymentHandler.Purchase(sender, product, callback);
		}

		private IPurchaseHandler GetHandler(PaymentType paymentType) {
			switch (paymentType) {
				case PaymentType.SoftCurrency:
					return this.purchaseHandlerBank;
				
				case PaymentType.ADS:
					return this.purchaseHandlerADS;

				case PaymentType.HardCurrency:
					return this.purchaseHandlerBank;
				
				case PaymentType.IAP:
					return this.purchaseHandlerIAP;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}

	}
}