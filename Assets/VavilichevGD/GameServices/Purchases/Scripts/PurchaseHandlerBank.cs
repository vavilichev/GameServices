using System;
using VavilichevGD.Utils;

namespace VavilichevGD.GameServices.Purchasing {
	public sealed class PurchaseHandlerBank : IPurchaseHandler {
		
		public void Purchase(object sender, Product product, Action<Product, bool> callback) {
			var paymentType = product.info.paymentType;
			
			if (paymentType != PaymentType.SoftCurrency && paymentType != PaymentType.HardCurrency)
				throw new NotSupportedException($"Purchase Handler Bank cannot handle not banking payment operation. Payment type: {paymentType}");
			
			if (product.info.paymentType == PaymentType.SoftCurrency) {
				this.TryToPurchaseForSoftCurrency(sender, product, callback);
				return;
			}
			
			if (product.info.paymentType == PaymentType.HardCurrency)
				this.TryToPurchaseForHardCurrency(sender, product, callback);
		}

		private void TryToPurchaseForSoftCurrency(object sender, Product product, Action<Product, bool> callback) {
			var price = (int) product.info.GetPrice();
			if (Bank.IsEnoughSoftCurrency(price)) {
				Bank.SpendSoftCurrency(sender, price);
				callback?.Invoke(product, Variables.SUCCESS);
			}
			else
				callback?.Invoke(product, Variables.FAIL);
		}
		
		private void TryToPurchaseForHardCurrency(object sender, Product product, Action<Product, bool> callback) {
			var price = (int) product.info.GetPrice();
			if (Bank.IsEnoughHardCurrency(price)) {
				Bank.SpendHardCurrency(sender, price);
				callback?.Invoke(product, Variables.SUCCESS);
			}
			else
				callback?.Invoke(product, Variables.FAIL);
		}
	}
}