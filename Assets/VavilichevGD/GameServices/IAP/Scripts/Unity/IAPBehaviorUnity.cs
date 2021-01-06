using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace VavilichevGD.GameServices.Purchasing.Unity {
	public class IAPBehaviorUnity : IAPBehavior, IStoreListener {

		private IAPService service;
		private IStoreController storeController;
		private IExtensionProvider storeExtensionProvider;
		private Dictionary<string, Product> iapProductsMap;
		private Product purchasingProduct;
		private event Action<Product, bool> callback; 
		private bool inProcess = false;
		
		public override bool isInitialized => this.storeController != null && this.storeExtensionProvider != null;

		#region CONSTRUCTOR AND INITIALIZING

		public IAPBehaviorUnity(IAPService service, Product[] iapProducts) : base(iapProducts) {
			this.service = service;
			this.iapProductsMap = new Dictionary<string, Product>();
			foreach (var product in iapProducts) {
				if (product.info.paymentType != PaymentType.IAP)
					continue;
				this.iapProductsMap[product.info.id] = product;
			}
		}

		
		protected override IEnumerator InitializeAsyncRoutine() {
			if (isInitialized)
				yield break;
			
			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			var products = this.iapProductsMap.Values.ToArray();
			foreach (var product in products) {
				if (product.info.isConsumable)
					builder.AddProduct(product.info.id, ProductType.Consumable);
				else if (!product.info.isConsumable) 
					builder.AddProduct(product.info.id, ProductType.NonConsumable);
			}
			
			UnityPurchasing.Initialize(this, builder);
		}
		
		
		private void ConfirmPendingPurchaseIfNeed() {
			foreach (var product in this.storeController.products.all)
				storeController.ConfirmPendingPurchase(product);
		}

		#endregion
		
		
		
		#region PURCHASING

		public override void Purchase(Product iapProduct, Action<Product, bool> callback) {
			var productId = iapProduct.info.id;
			
			if (iapProduct.info.paymentType != PaymentType.IAP) {
				this.PrintError($"IAPBehaviorUnity: Cannot buy not IAP product ({productId})");
				callback?.Invoke(iapProduct, FAIL);
				return;
			}
			
			if (!this.isInitialized) {
				this.PrintError($"IAPBehaviorUnity: Cannot start payment ({productId}) because IAP behavior Unity is not initialized!");
				this.TryToReinitialize();
				callback?.Invoke(iapProduct, FAIL);
				return;
			}
			
			if (this.inProcess) {
				this.PrintError($"IAPBehaviorUnity: Cannot start payment ({productId}) while another one wasnt ended");
				callback?.Invoke(iapProduct, FAIL);
				return;
			}

			
			UnityEngine.Purchasing.Product unityPurchasingProduct = this.storeController.products.WithID(productId);
			if (!this.CanPurchaseProduct(iapProduct, unityPurchasingProduct)) {
				this.PrintError($"IAPBehaviorUnity: Cannot pay for {productId}");
				callback?.Invoke(iapProduct, FAIL);
				return;
			}

			this.PrintLog($"IAPBehaviorUnity: Try to purchase '{productId}'");
			this.callback = callback;
			this.purchasingProduct = iapProduct;
			this.inProcess = true;
			this.storeController.InitiatePurchase(unityPurchasingProduct);
		}

		private void TryToReinitialize() {
			this.InitializeAsync();
		}
		
		private bool CanPurchaseProduct(Product product, UnityEngine.Purchasing.Product unityPurchasingProduct) {
			return unityPurchasingProduct != null && unityPurchasingProduct.availableToPurchase && !this.IsPurchased(product);
		}
		
		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
			var validPurchase = this.IsPurchaseValid(e);
			this.callback?.Invoke(this.purchasingProduct, validPurchase);
			
			this.PrintLog($"IAPBehaviorUnity: SUCCESS = {validPurchase}. Product: {e.purchasedProduct.definition.id}");
			this.ClearData();
			return PurchaseProcessingResult.Complete;
		}
		
		private bool IsPurchaseValid(PurchaseEventArgs args) {
#if DEBUG
			return true;
#endif
			
			// Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
			// Prepare the validator with the secrets we prepared in the Editor
			// obfuscation window.
			var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
				AppleTangle.Data(), Application.identifier);

			try {
				// On Google Play, result has a single product ID.
				// On Apple stores, receipts contain multiple products.
				var result = validator.Validate(args.purchasedProduct.receipt);
				// For informational purposes, we list the receipt(s)
				Debug.Log("IAPBehaviorUnity: Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
					Debug.Log("IAPBehaviorUnity: Product ID:" + productReceipt.productID);
					Debug.Log("IAPBehaviorUnity: Purchase Date:" + productReceipt.purchaseDate);
					Debug.Log("IAPBehaviorUnity: Transaction ID:" + productReceipt.transactionID);
				}

				return true;
			} catch (IAPSecurityException) {
				Debug.Log("IAPBehaviorUnity: Invalid receipt, not unlocking content");
				return false;
			}
#endif
			return true;
		}

		#endregion
		
		
		public override bool IsPurchased(Product iapProduct) {
			if (iapProduct.info.isConsumable)
				return false;
			
			var unityPurchasingProduct = this.storeController.products.WithID(iapProduct.info.id);
			if (unityPurchasingProduct != null)
				return unityPurchasingProduct.hasReceipt;
			return false;
		}
		
		public override string GetLocalizedPrice(string productId) {
			if (!this.isInitialized) {
				this.PrintError($"IAPBehaviorUnity: Not initialized");
				return "$0.01";
			}
			
			this.iapProductsMap.TryGetValue(productId, out var product);
			if (product == null) {
				this.PrintLog($"IAPBehaviorUnity: There is no IAP products with Id = {productId} was found to return localized price.");
				return "NaN";
			}
			
			UnityEngine.Purchasing.Product unityPurchasingProduct = this.storeController.products.WithID(productId);
			return unityPurchasingProduct.metadata.localizedPrice.ToString("0.00");
		}
		
		
		
		private void ClearData() {
			this.inProcess = false;
			this.callback = null;
			this.purchasingProduct = null;
		}
		
		private void PrintLog(string text) {
			if (this.service.isLoggingEnabled)
				Debug.Log(text);
		}

		private void PrintError(string text) {
			Debug.LogError(text);
		}

		
		
		
		#region EVENTS

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
			this.storeController = controller;
			this.storeExtensionProvider = extensions;

			this.ConfirmPendingPurchaseIfNeed();
			this.PrintLog($"IAPBehaviorUnity: Initialized. IAP Products count: {this.iapProductsMap.Count}");
		}
		
		public void OnInitializeFailed(InitializationFailureReason error) {
			this.PrintError("IAPBehaviorUnity: OnInitializeFailed (" + error + ")");
		}
		
		public void OnPurchaseFailed(UnityEngine.Purchasing.Product unityProduct, PurchaseFailureReason reason) {
			this.PrintError($"IAPBehaviorUnity: Payment failed. Product: {unityProduct.definition.id}, reason: {reason}");
			callback?.Invoke(this.purchasingProduct, FAIL);
			this.ClearData();
		}

		#endregion

	}
}