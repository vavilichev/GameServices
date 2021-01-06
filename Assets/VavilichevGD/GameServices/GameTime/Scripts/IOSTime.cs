using System.Runtime.InteropServices;

namespace VavilichevGD.GameServices.Time {
	public static class IOSTime {
		[DllImport("__Internal")]
		public static extern long GetSystemUpTime();
	}
}