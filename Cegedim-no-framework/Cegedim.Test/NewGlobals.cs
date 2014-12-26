using System;
using System.Linq;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Cegedim.Automation;
using NUnit.Framework;


public static class NewGlobals {
    static iOSApp _app { get; set; }
    static bool _isLoggedIn { get; set; }

    public static iOSApp App {
        get {
            // IMPORTANT This will set the OS version for local testing since elements differ
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                Environment.SetEnvironmentVariable("XTC_DEVICE_OS", "7.1");
            }
            if (_app == null) {
                _app = Launch();
            }
            else {
                try {
                    var result = _app.Query(c => c.Raw("*"));
                    if (result.Count() == 0) {
                        _app = Launch();
                        _isLoggedIn = false;
                    }
                    var popover = _app.Query(c => c.Raw("* {text contains 'There is no account defined'}"));
                    if (popover.Count() != 0) {
                        _app = Launch();
                        _isLoggedIn = false;
                    }
                    var loginButton = _app.Query(c => c.Raw("view:'MI.Login+LoginSource+LoginButton'"));
                    if (loginButton.Count() != 0)
                        _isLoggedIn = false;
                } catch {
                    _app = Launch();
                    _isLoggedIn = false;
                }
            }
            return _app;
        }
    }

    public static bool IsLoggedIn() {
        return _isLoggedIn;
    }

	public static NewDashboardPage QuickSetUp() {
		if (IsLoggedIn ()) {
			GetDashboardPageAfterReset ();
		}
        else {
            //GetDashboardPageAfterReset();
			var loginPage = new NewLoginPage (_app);
			loginPage.Username = "jmayo";
			loginPage.Password = "cegedim";
			loginPage.SubmitCredentials ();
			var dashboardPage = new NewDashboardPage (_app);
			dashboardPage.IsLoaded ();
            _isLoggedIn = true;
        }
		return new NewDashboardPage (_app);
    }

    public static iOSApp Launch() {
        _app = ConfigureApp
            .iOS
            .ApiKey("3250166bbe78a884fbd6089582a8df7c")
            .InstalledApp("com.cegedim.mi7")
            .StartApp();
		return _app;
    }

    public static void GetDashboardPageAfterReset() {
        _app.Invoke("resetToHome:", "");
        // Sometimes a pop over from previous tests will cause the subsequent tests to fail
        if (_app.Query(c => c.Marked("Delete")).Count() > 0)
            _app.Tap(c => c.Marked("Delete"));
    }
}