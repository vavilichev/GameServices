using System;
using VavilichevGD.Utils;

namespace VavilichevGD.GameServices.Time {
	[Serializable]
	public sealed class GameSessionTimeData {

		public DateTimeSerialized sessionStartSerializedFromServer;
		public DateTimeSerialized sessionStartSerializedFromDevice;
		public long timeValueActiveDeviceAtStart;
		public long timeValueActiveDeviceAtEnd;
		public double sessionDuration;
		public bool isValid;

		
		public bool timeReceivedFromServer => this.sessionStartSerializedFromServer.value != new DateTime();
		public DateTime sessionStart => this.GetSessionStartTime();
		public DateTime sessionStartDevice => this.sessionStartSerializedFromDevice.value;
		public DateTime sessionStartServer => this.sessionStartSerializedFromServer.value;
		public DateTime sessionOver => this.GetSessionOverTime();

		
		
		public GameSessionTimeData() {
			this.sessionStartSerializedFromServer = new DateTimeSerialized();
			this.sessionStartSerializedFromDevice = new DateTimeSerialized();
			this.isValid = false;
		}

		
		
		private DateTime GetSessionStartTime() {
			if (this.timeReceivedFromServer)
				return this.sessionStartSerializedFromServer.value;
			return this.sessionStartSerializedFromDevice.value;
		}

		private DateTime GetSessionOverTime() {
			var start = this.sessionStart;
			return start.AddSeconds(this.sessionDuration);
		}

		
		
		public override string ToString() {
			return $"Time start from server: {this.sessionStartSerializedFromServer}\n" +
			       $"Time start from device: {this.sessionStartSerializedFromDevice}\n" +
			       $"Active device time at start: {this.timeValueActiveDeviceAtStart}\n" +
			       $"Active device time at end: {this.timeValueActiveDeviceAtEnd}\n" +
			       $"Session duration: {this.sessionDuration}\n" +
			       $"Time received from server: {this.sessionStart}\n" +
			       $"Session start: {this.sessionStart}\n" +
			       $"Session over: {this.sessionOver}";
		}

	}
}