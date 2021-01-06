using UnityEngine;

namespace VavilichevGD.GameServices.Purchasing.Example {
	[CreateAssetMenu(fileName = "ProductInfoExample", menuName = "Monetization/Products/New ProductInfoExample")]
	public class ProductInfoExample : ProductInfo {

		[SerializeField] private int price;
		
		public override object GetPrice() {
			return this.price;
		}

		public override string GetPriceString() {
			return this.price.ToString();
		}
	}
}