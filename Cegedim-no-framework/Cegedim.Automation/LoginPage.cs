using System;
using Xamarin.Automation;

namespace Cegedim.Automation {

    public class LoginPage : CegedimPage { 

        private static class Query {
            internal static string LoginButton = "view:'MI.Login+LoginSource+LoginButton'";
            internal static string UsernameField = "view:'UITextField' index:0";
            internal static string PasswordField = "view:'UITextField' index:1";
        }

        internal LoginPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.LoginButton); }
        }

        public string Username {
            get { return GetField(Query.UsernameField); }
            set { SetField(Query.UsernameField, value); }
        }

        public string Password {
            get { return GetField(Query.PasswordField); }
            set { SetField(Query.PasswordField, value); }
        }

        public DashboardPage SubmitCredentials() {
            return AppConvention.TapActivateAndWait<DashboardPage>(
                Application, Query.LoginButton);
        }

        public LoginPage SubmitInvalidCredentials() {
            return AppConvention.TapActivateAndWait<LoginPage>(
                Application, Query.LoginButton);
        }

        public DashboardPage Login(string username, string password) {
            Username = username;
            Password = password;

            return SubmitCredentials();
        }
    }
}