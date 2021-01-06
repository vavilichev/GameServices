using UnityEngine;

namespace VavilichevGD.GameServices.Purchasing {
	public abstract class ProductInfo : ScriptableObject, IProductInfo {

		[SerializeField] private string m_id;
		[SerializeField] private PaymentType m_paymentType;
		[SerializeField] private bool m_isConsumable = true;
		[SerializeField] private string m_titleCode;
		[SerializeField] private string m_descriptionCode;

		public string id => this.m_id;
		public PaymentType paymentType => this.m_paymentType;
		public bool isConsumable => this.m_isConsumable;
		public string titleCode => this.m_titleCode;
		public string descriptionCode => this.m_descriptionCode;

		public abstract object GetPrice();
		public abstract string GetPriceString();
	}
}