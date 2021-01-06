using System;

namespace VavilichevGD.GameServices.Purchasing {
	public sealed class PurchaseHandlerIAP : IPurchaseHandler {
		
		public void Purchase(object sender, Product product, Action<Product, bool> callback) {
			var paymentType = product.info.paymentType;
			
			if (paymentType != PaymentType.IAP)
				throw new NotSupportedException($"Purchase Handler IAP cannot handle not IAP payment operation. Payment type: {paymentType}");

			void LocalPurchaseCallback(Product product1, bool success) {
				callback?.Invoke(product, success);
			}
			
			IAP.Purchase(product, LocalPurchaseCallback);
		}

	}
}