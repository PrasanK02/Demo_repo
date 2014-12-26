using System;
using System.Linq;
using System.Threading;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using Cegedim.Automation;
using NUnit.Framework;


public static class Globals {
    static MITouch _app { get; set; }
    static bool _isLoggedIn { get; set; }
//	public static IApp App {
//		get {
//			// IMPORTANT This will set the OS version for local testing since elements differ
//			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
//				Environment.SetEnvironmentVariable("XTC_DEVICE_OS", "7.1");
//			}
//			if (_app == null) {
//				_app = MITouch.Launch();
//			}
//			else {
//				try {
//					var result = MITouch.iosApp.Query(c => c.Raw("*"));
//					if (result.Count() == 0) {
//						_app = MITouch.Launch();
//						_isLoggedIn = false;
//					}
//					var popover = MITouch.iosApp.Query(c => c.Raw("* {text contains 'There is no account defined'}"));
//					if (popover.Count() != 0) {
//						_app = MITouch.Launch();
//						_isLoggedIn = false;
//					}
//					var loginButton = MITouch.iosApp.Query(c => c.Raw("view:'MI.Login+LoginSource+LoginButton'"));
//					if (loginButton.Count() != 0)
//						_isLoggedIn = false;
//				} catch {
//					_app = MITouch.Launch();
//					_isLoggedIn = false;
//				}
//			}
//			return _app;
//		}


    public static MITouch App {
        get {
            // IMPORTANT This will set the OS version for local testing since elements differ
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                Environment.SetEnvironmentVariable("XTC_DEVICE_OS", "7.1");
            }
            if (_app == null) {
                _app = MITouch.Launch();
            }
            else {
                try {
                    var result = MITouch.iosApp.Query(c => c.Raw("*"));
                    if (result.Count() == 0) {
                        _app = MITouch.Launch();
                        _isLoggedIn = false;
                    }
                    var popover = MITouch.iosApp.Query(c => c.Raw("* {text contains 'There is no account defined'}"));
                    if (popover.Count() != 0) {
                        _app = MITouch.Launch();
                        _isLoggedIn = false;
                    }
                    var loginButton = MITouch.iosApp.Query(c => c.Raw("view:'MI.Login+LoginSource+LoginButton'"));
                    if (loginButton.Count() != 0)
                        _isLoggedIn = false;
                } catch {
                    _app = MITouch.Launch();
                    _isLoggedIn = false;
                }
            }
            return _app;
        }
    }

    public static bool IsLoggedIn() {
			return _isLoggedIn;
    }

    public static DashboardPage QuickSetUp() {
        DashboardPage dashboard;
        if (IsLoggedIn()) 
            dashboard = App.GetDashboardPageAfterReset();
        else {
//            var loginPage = App.GetLoginPage();
//            dashboard = loginPage.Login("jmayo", "cegedim");
            dashboard = App.GetDashboardPageAfterReset();
            _isLoggedIn = true;
        }
        return dashboard;
    }

    public static void ForceLaunch() {
        _app = MITouch.Launch();
    }
}
