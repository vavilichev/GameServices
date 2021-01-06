namespace VavilichevGD.GameServices.Purchasing {
	public sealed class Product {
		
		public ProductInfo info { get; }
		public ProductState state { get; }

		public Product(ProductInfo info, ProductState state) {
			this.info = info;
			this.state = state;
		}
		
	}
}