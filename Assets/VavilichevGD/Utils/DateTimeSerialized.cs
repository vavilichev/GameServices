using System;

namespace VavilichevGD.Utils {
	[Serializable]
	public sealed class DateTimeSerialized {

		private string dateTimeStr;

		public DateTime value {
			get => this.GetDateTime();
			set => this.SetDateTime(value);
		}


		public DateTimeSerialized(DateTime dateTime) {
			this.SetDateTime(dateTime);
		}

		public DateTimeSerialized() {
			var dateTimeDefault = new DateTime();
			this.value = dateTimeDefault;
		}



		private DateTime GetDateTime() {
			if (!string.IsNullOrEmpty(dateTimeStr))
				return DateTime.Parse(dateTimeStr);
			return new DateTime();
		}

		private void SetDateTime(DateTime dateTime) {
			dateTimeStr = dateTime.ToString();
		}

		public override string ToString() {
			return dateTimeStr;
		}

	}
}