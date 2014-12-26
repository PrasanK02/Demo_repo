using System;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace Cegedim.Automation {
    public class NewLoginPage { 
        private iOSApp app;
        private string username = "";
        private string password = "";

        private static class Query {
            internal static string LoginButton = "view:'MI.Login+LoginSource+LoginButton'";
            internal static string UsernameField = "view:'UITextField' index:0";
            internal static string PasswordField = "view:'UITextField' index:1";
        }

        public NewLoginPage(iOSApp application) {
            app = application;
        }

        public void IsLoaded() {
            app.WaitForElement(c => c.Raw(Query.LoginButton)); 
        }

        public string Username {
            get { 
                return username; 
            }
            set { 
                app.EnterText(c => c.Raw(Query.UsernameField), value);
                username = value;
            }
        }

        public string Password {
            get { 
                return password;
            }
            set {  
                app.EnterText(c => c.Raw(Query.PasswordField), value);
                password = value;
            }
        }

        public void SubmitCredentials() {
            app.Tap(c => c.Raw(Query.LoginButton));
        }
    }
}

