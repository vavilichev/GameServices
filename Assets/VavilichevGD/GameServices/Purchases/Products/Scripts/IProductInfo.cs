namespace VavilichevGD.GameServices.Purchasing {
	public partial interface IProductInfo {
		string id { get; }
		PaymentType paymentType { get; }
		string titleCode { get; }
		string descriptionCode { get; }

		object GetPrice();
	}
}