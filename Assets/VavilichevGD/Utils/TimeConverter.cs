namespace VavilichevGD.Utils {
	public static class TimeConverter {
		
		public static string ToFormatMinSec(int seconds) {
			var min = seconds / 60;
			var sec = seconds % 60;
			var strMin = min < 10 ? $"0{min}" : min.ToString();
			var strSec = sec < 10 ? $"0{sec}" : sec.ToString();
			return $"{strMin}:{strSec}";
		}

		public static string ToFormatHrMinSec(int seconds) {
			var hours = seconds / 3600;
			var min = (seconds / 60) % 60;
			var sec = seconds % 60;
			var strHours = hours < 10 ? $"0{hours}" : hours.ToString();
			var strMin = min < 10 ? $"0{min}" : min.ToString();
			var strSec = sec < 10 ? $"0{sec}" : sec.ToString();
			return $"{strHours}:{strMin}:{strSec}";
		}
        
		public static string ToFormatHrMin(int seconds) {
			var hours = seconds / 3600;
			var min = (seconds / 60) % 60;
			var strHours = hours < 10 ? $"0{hours}" : hours.ToString();
			var strMin = min < 10 ? $"0{min}" : min.ToString();
			return $"{strHours}:{strMin}";
		}
		
	}
}