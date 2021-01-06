using System;

namespace VavilichevGD.GameServices.Purchasing {
	[Serializable]
	public class BankState {
		public int softCurrency;
		public int hardCurrency;

		public BankState() {
			this.softCurrency = 0;
			this.hardCurrency = 0;
		}

		public BankState(int softCurrency, int hardCurrency) {
			this.softCurrency = softCurrency;
			this.hardCurrency = hardCurrency;
		}
	}
}