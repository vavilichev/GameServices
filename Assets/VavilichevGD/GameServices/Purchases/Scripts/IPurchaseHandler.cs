using System;

namespace VavilichevGD.GameServices.Purchasing {
	public interface IPurchaseHandler {

		void Purchase(object sender, Product product, Action<Product, bool> callback);
		
	}
}