using System;
using System.Linq;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace Cegedim.Automation {

    public sealed class MITouch : App {
        private static readonly string BinaryName = "MI-10.3.0016.resigned.ipa";
        private static readonly string BundleName = "com.cegedim.mi7";
        public static User primaryUser;

        public static iOSApp iosApp {
            get;
            private set;
        }

        public static MITouch Launch(string ipAddress = null) {

            var server = CalabashServer.IOSLaunch(
                BinaryName,
                BundleName,
                ipAddress
            );


            iosApp = CalabashServer.iosApp;


            primaryUser = new User((CalabashServer)server);
            return new MITouch(server);
        }

        public static MITouch Attach(string ipAddress = null) {
            var server = CalabashServer.IOSAttach(ipAddress);
            return new MITouch(server);
        }

        private MITouch(AppServer server)
            : base(server) {
        }

        public DashboardPage GetDashboardPageAfterReset() {
            iosApp.Invoke("resetToHome:", "");
            // Sometimes a pop over from previous tests will cause the subsequent tests to fail
            if (MITouch.iosApp.Query(c => c.Marked("Delete")).Count() > 0)
                MITouch.iosApp.Tap(c => c.Marked("Delete"));
            return AppConvention.ActivateAndWait<DashboardPage>(this);
        }

//        public LoginPage GetLoginPage() {
//            return AppConvention.ActivateAndWait<LoginPage>(this, timeout: TimeSpan.FromSeconds(15));
//        }
    }
}

