using NUnit.Framework;
using System;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Cegedim.Automation;


namespace Cegedim
{
    [TestFixture]
    [Category("NoFramework")]
    public class Login 
    {
        private iOSApp app;

        [SetUp]
        public void Setup() {
            app = ConfigureApp
                .iOS
                .ApiKey("3250166bbe78a884fbd6089582a8df7c")
                .InstalledApp("com.cegedim.mi7")
                .StartApp();
        }

        [Test]
        public void ValidCredentials() {
            var loginPage = new NewLoginPage(app);
            loginPage.IsLoaded();
            app.Screenshot("I'm on the login page");

            loginPage.Username = "jmayo";
            loginPage.Password = "cegedim";
            app.Screenshot("I've entered valid credentials");

            loginPage.SubmitCredentials();
            var dashboardPage = new NewDashboardPage(app);
            dashboardPage.IsLoaded();
            app.Screenshot("I'm on the dashboard page");
        }

        [Test]
        public void InvalidCredentials() {
            var loginPage = new NewLoginPage(app);
            loginPage.IsLoaded();
            app.Screenshot("I'm on the login page");

            loginPage.Username = "jmay";
            loginPage.Password = "cegy";
            app.Screenshot("I've entered invalid credentials");

            loginPage.SubmitCredentials();
            Thread.Sleep(1);
            loginPage.IsLoaded();
            app.Screenshot("I'm still on the login page");
        }       
    }
}
