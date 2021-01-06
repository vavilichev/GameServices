using System;
using VavilichevGD.GameServices.AD;

namespace VavilichevGD.GameServices.Purchasing {
	public sealed class PurchaseHandlerADS : IPurchaseHandler {
		public void Purchase(object sender, Product product, Action<Product, bool> callback) {
			var paymentType = product.info.paymentType;
			
			if (paymentType != PaymentType.ADS)
				throw new NotSupportedException($"Purchase Handler ADS cannot handle not ADS payment operation. Payment type: {paymentType}");
			
			void RewardedVideoCallback(bool success) {
				callback?.Invoke(product, success);
			}
			
			ADS.ShowRewardedVideo(RewardedVideoCallback);
		}
		
	}
}