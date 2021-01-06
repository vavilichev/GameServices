using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.Purchasing.Example {
	public class WidgetProductExample : MonoBehaviour {

		#region EVENTS

		public event Action<WidgetProductExample> OnClickedEvent;

		#endregion

		[SerializeField] private Text textTitle;
		[SerializeField] private Text textDescription;
		[SerializeField] private Text textPrice;
		[SerializeField] private Button buttonPurchase;


		public Product product { get; private set; }
		
		

		public void Setup(Product product) {
			this.product = product;
			
			this.SetupTitle(product);
			this.SetupDescription(product);
			this.SetupPrice(product);
		}

		private void SetupTitle(Product product) {
			this.textTitle.text = product.info.titleCode;
		}

		private void SetupDescription(Product product) {
			this.textDescription.text = product.info.descriptionCode;
		}

		private void SetupPrice(Product product) {
			var paymentType = product.info.paymentType;
			
			string price;
			switch (paymentType) {
				case PaymentType.SoftCurrency:
					price = product.info.GetPriceString();
					break;
				case PaymentType.HardCurrency:
					price = product.info.GetPriceString();
					break;
				case PaymentType.ADS:
					price = "Watch AD";
					break;
				case PaymentType.IAP:
					price = IAP.GetLocalizedPrice(product.info.id);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			this.textPrice.text = price;
		}


		#region LIFECYCLE

		private void OnEnable() {
			this.buttonPurchase.onClick.AddListener(this.OnClick);
		}

		private void OnDisable() {
			this.buttonPurchase.onClick.RemoveListener(this.OnClick);
		}

		#endregion



		#region EVENTS

		private void OnClick() {
			this.OnClickedEvent?.Invoke(this);
		}

		#endregion
		
	}
}