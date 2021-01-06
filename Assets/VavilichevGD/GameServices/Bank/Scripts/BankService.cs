using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.GameServices.Purchasing {
	public class BankService : GameServiceBase {

		#region EVENTS

		public override event Action OnInitializedEvent;
		public event BankCurrencyHandler OnSoftCurrencyValueChangedEvent; 
		public event BankCurrencyHandler OnHardCurrencyValueChangedEvent; 

		#endregion

		public int softCurrency {
			get => this.state.softCurrency;
			private set => this.state.softCurrency = value;
		}

		public int hardCurrency {
			get => this.state.hardCurrency;
			private set => this.state.hardCurrency = value;
		}
		
			
		private BankState state { get; set; }


		
		public BankService(BankState state) {
			this.state = state;
			this.PrintLog($"BANK SERVICE: State loaded. Soft: {this.softCurrency}, Hard: {this.hardCurrency}");
		}

		public BankService() { }
		
		
		
		#region INITIALIZE


		protected override IEnumerator InitializeAsyncRoutine() {
			this.LoadState();
			this.InitFasade();
			this.OnInitializedEvent?.Invoke();
			yield break;
		}

		private void LoadState() {
			if (this.state != null)
				return;
			
			this.state = new BankState();
			this.PrintLog($"BANK SERVICE: State loaded. Soft: {this.softCurrency}, Hard: {this.hardCurrency}");
		}

		private void InitFasade() {
			Bank.Initialize(this);
			this.PrintLog($"BANK SERVICE: Fasade Initialized (Bank.cs)");
		}

		#endregion
		


		public void AddSoftCurrency(object sender, int value) {
			var oldValue = this.softCurrency;
			var newValue = oldValue + value;
			this.softCurrency = newValue;
			
			this.PrintLog($"BANK SERVICE: Soft currency changed. Old value: {oldValue}, new Value: {newValue}. Initiator: {sender.GetType().Name}");
			this.OnSoftCurrencyValueChangedEvent?.Invoke(sender, oldValue, newValue);
		}

		public void SpendSoftCurrency(object sender, int value) {
			var oldValue = this.softCurrency;
			var newValue = oldValue - value;
			this.softCurrency = newValue;

			this.PrintLog($"BANK SERVICE: Soft currency changed. Old value: {oldValue}, new Value: {newValue}. Initiator: {sender.GetType().Name}");
			this.OnSoftCurrencyValueChangedEvent?.Invoke(sender, oldValue, newValue);
		}

		public bool IsEnoughSoftCurrency(int value) {
			return this.softCurrency >= value;
		}
		
		public void AddHardCurrency(object sender, int value) {
			var oldValue = this.hardCurrency;
			var newValue = oldValue + value;
			this.hardCurrency = newValue;

			this.PrintLog($"BANK SERVICE: Hard currency changed. Old value: {oldValue}, new Value: {newValue}. Initiator: {sender.GetType().Name}");
			this.OnHardCurrencyValueChangedEvent?.Invoke(sender, oldValue, newValue);
		}

		public void SpendHardCurrency(object sender, int value) {
			var oldValue = this.hardCurrency;
			var newValue = oldValue - value;
			this.hardCurrency = newValue;
			
			this.PrintLog($"BANK SERVICE: Hard currency changed. Old value: {oldValue}, new Value: {newValue}. Initiator: {sender.GetType().Name}");
			this.OnHardCurrencyValueChangedEvent?.Invoke(sender, oldValue, newValue);
		}

		public bool IsEnoughHardCurrency(int value) {
			return this.hardCurrency >= value;
		}


		private void PrintLog(string text) {
			if (this.isLoggingEnabled)
				Debug.Log(text);
		}
		
	}
}