namespace VavilichevGD.GameServices.Purchasing {
	public interface IProductInfoDynamic : IProductInfo {
		IProductInfo GenerateInfo();
	}
}