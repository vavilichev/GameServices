using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.Purchasing.Example {
	[Serializable]
	public class BankServiceExampleCurrencyController {

		#region EVENTS

		public event Action<int> OnAddButtonClickEvent;
		public event Action<int> OnSpendButtonClickEvent; 

		#endregion
			
		[SerializeField] private InputField inputValue;
		[SerializeField] private Button buttonAdd;
		[SerializeField] private Button buttonSpend;
		[SerializeField] private Text textCurrentValue;

		public void FirstSetup(int value) {
			this.inputValue.text = value.ToString();
		}

		public void Subscribe() {
			this.buttonAdd.onClick.AddListener(this.OnAddButtonClick);
			this.buttonSpend.onClick.AddListener(this.OnSpendButtonClick);
		}

		public void Unsubscribe() {
			this.buttonAdd.onClick.RemoveListener(this.OnAddButtonClick);
			this.buttonSpend.onClick.RemoveListener(this.OnSpendButtonClick);
		}

		public void SetCurrentValue(int value) {
			this.textCurrentValue.text = value.ToString();
		}

		#region EVENTS

		private void OnAddButtonClick() {
			if (string.IsNullOrEmpty(this.inputValue.text))
				return;
			
			var value = int.Parse(this.inputValue.text);
			this.OnAddButtonClickEvent?.Invoke(value);				
		}

		private void OnSpendButtonClick() {
			if (string.IsNullOrEmpty(this.inputValue.text))
				return;
			
			var value = int.Parse(this.inputValue.text);
			this.OnSpendButtonClickEvent?.Invoke(value);
		}

		#endregion
	}
}