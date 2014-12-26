using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace Cegedim.Automation {

	public sealed class NewApplication {
		private static readonly string BinaryName = "MI-10.3.0016.resigned.ipa";
		private static readonly string BundleName = "com.cegedim.mi7";
		public static User primaryUser;

		public static iOSApp iosApp {
			get;
			private set;
		}

		public static IApp Launch(string ipAddress = null) {

			iosApp = ConfigureApp
				.iOS
				.ApiKey("3250166bbe78a884fbd6089582a8df7c")
				.InstalledApp(BundleName)
				.StartApp();
			return iosApp;
		}

//		public static MITouch Attach(string ipAddress = null) {
//			var server = CalabashServer.IOSAttach(ipAddress);
//			return new MITouch(server);
//		}
//
//		private MITouch(AppServer server)
//			: base(server) {
//		}

		public NewDashboardPage GetDashboardPageAfterReset() {
			iosApp.Invoke("resetToHome:", "");
			// Sometimes a pop over from previous tests will cause the subsequent tests to fail
			if (iosApp.Query(c => c.Marked("Delete")).Count() > 0)
				iosApp.Tap(c => c.Marked("Delete"));
			var dashboard = Activator.CreateInstance<NewDashboardPage>();
				dashboard.IsLoaded();
			return dashboard;
			//return AppConvention.ActivateAndWait<DashboardPage>(this);

		}

		//        public LoginPage GetLoginPage() {
		//            return AppConvention.ActivateAndWait<LoginPage>(this, timeout: TimeSpan.FromSeconds(15));
		//        }
	}
}

