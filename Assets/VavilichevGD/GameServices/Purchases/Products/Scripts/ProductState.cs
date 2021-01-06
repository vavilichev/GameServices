using System;

namespace VavilichevGD.GameServices.Purchasing {
	[Serializable]
	public class ProductState {
		public string productId;
		public bool isViewed;

		public ProductState(string productId) {
			this.productId = productId;
			this.isViewed = false;
		}
	}
}