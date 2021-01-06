using System;

namespace VavilichevGD.GameServices.Purchasing {
	/// <summary>
	/// Fasade of BankService.
	/// </summary>
	public static class Bank {

		#region EVENTS

		public static event Action OnBankServiceInitializedEvent; 
		public static event BankCurrencyHandler OnSoftCurrencyValueChangedEvent {
			add => service.OnSoftCurrencyValueChangedEvent += value;
			remove => service.OnSoftCurrencyValueChangedEvent -= value;
		}
		public static event BankCurrencyHandler OnHardCurrencyValueChangedEvent {
			add => service.OnHardCurrencyValueChangedEvent += value;
			remove => service.OnHardCurrencyValueChangedEvent -= value;
		} 

		#endregion


		public static bool isInitialized { get; private set; } = false;
		public static BankService service { get; private set; }

		public static int softCurrency => service.softCurrency;
		public static int hardCurrency => service.hardCurrency;
		
		public static void Initialize(BankService _service) {
			service = _service;
			isInitialized = true;
			
			OnBankServiceInitializedEvent?.Invoke();
		}
		
		
		public static void AddSoftCurrency(object sender, int value) {
			CheckStatus();
			service.AddSoftCurrency(sender, value);
		}

		public static void SpendSoftCurrency(object sender, int value) {
			CheckStatus();
			service.SpendSoftCurrency(sender, value);
		}

		public static bool IsEnoughSoftCurrency(int value) {
			CheckStatus();
			return service.IsEnoughSoftCurrency(value);
		}
		
		public static void AddHardCurrency(object sender, int value) {
			CheckStatus();
			service.AddHardCurrency(sender, value);
		}

		public static void SpendHardCurrency(object sender, int value) {
			CheckStatus();
			service.SpendHardCurrency(sender, value);
		}

		public static bool IsEnoughHardCurrency(int value) {
			CheckStatus();
			return service.IsEnoughHardCurrency(value);
		}
		
		private static void CheckStatus() {
			if (!isInitialized)
				throw new Exception("Bank is not initialized yet");
		}
		
	}
}