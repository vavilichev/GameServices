using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.GameServices.Purchasing.Example {
	public class BankServiceExample : MonoBehaviour {

		[SerializeField] private BankServiceExampleCurrencyController controllerSoftCurrency;
		[SerializeField] private BankServiceExampleCurrencyController controllerHardCurrency;
		[SerializeField] private Text textLog;

		private BankService bankService;


		private void OnEnable() {
			this.controllerSoftCurrency.Subscribe();
			this.controllerHardCurrency.Subscribe();

			this.controllerSoftCurrency.OnAddButtonClickEvent += OnAddedSoftCurrency;
			this.controllerSoftCurrency.OnSpendButtonClickEvent += OnSpentSoftCurrency;
			this.controllerHardCurrency.OnAddButtonClickEvent += OnAddedHardCurrency;
			this.controllerHardCurrency.OnSpendButtonClickEvent += OnSpentHardCurrency;
		}

		private void OnDisable() {
			this.controllerSoftCurrency.Unsubscribe();
			this.controllerHardCurrency.Unsubscribe();

			this.controllerSoftCurrency.OnAddButtonClickEvent -= OnAddedSoftCurrency;
			this.controllerSoftCurrency.OnSpendButtonClickEvent -= OnSpentSoftCurrency;
			this.controllerHardCurrency.OnAddButtonClickEvent -= OnAddedHardCurrency;
			this.controllerHardCurrency.OnSpendButtonClickEvent -= OnSpentHardCurrency;
		}


		private void Start() {
			this.bankService = new BankService();
			this.bankService.isLoggingEnabled = true;
			this.bankService.InitializeAsync();

			if (Bank.isInitialized)
				this.UpdateValues();
			else
				Bank.OnBankServiceInitializedEvent += this.OnBankServiceInitialized;
			
			this.controllerSoftCurrency.FirstSetup(20);
			this.controllerHardCurrency.FirstSetup(10);
		}

		private void UpdateValues() {
			this.EraseLog();
			this.controllerSoftCurrency.SetCurrentValue(Bank.softCurrency);
			this.controllerHardCurrency.SetCurrentValue(Bank.hardCurrency);
		}

		private void Log(string text) {
			this.textLog.text = text;
		}

		private void EraseLog() {
			this.textLog.text = "";
		}




	#region EVENTS

		private void OnAddedSoftCurrency(int value) {
			this.EraseLog();
			Bank.AddSoftCurrency(this, value);
			this.controllerSoftCurrency.SetCurrentValue(Bank.softCurrency);
		}

		private void OnSpentSoftCurrency(int value) {
			if (!Bank.IsEnoughSoftCurrency(value)) {
				this.Log("Not enough SOFT currency.");
				return;
			}
			
			this.EraseLog();
			Bank.SpendSoftCurrency(this, value);
			this.controllerSoftCurrency.SetCurrentValue(Bank.softCurrency);
		}

		private void OnAddedHardCurrency(int value) {
			this.EraseLog();
			Bank.AddHardCurrency(this, value);
			this.controllerHardCurrency.SetCurrentValue(Bank.hardCurrency);
		}

		private void OnSpentHardCurrency(int value) {
			if (!Bank.IsEnoughHardCurrency(value)) {
				this.Log("Not enough HARD currency.");
				return;
			}
			
			this.EraseLog();
			Bank.SpendHardCurrency(this, value);
			this.controllerHardCurrency.SetCurrentValue(Bank.hardCurrency);
		}
		
		private void OnBankServiceInitialized() {
			Bank.OnBankServiceInitializedEvent -= this.OnBankServiceInitialized;
			this.UpdateValues();
		}

		#endregion
		
	}
}