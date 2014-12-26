using Cegedim.Automation.Experimental;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cegedim.Test {

    [TestFixture]
    public class Experimental {

        private MITouch m_app;

       //[SetUp]
        public void Setup() {
            m_app = MITouch.Launch();
        }

        //[Test]
        public void Launch() {

        }

        //[Test]
        public void ValidCredentials() {
            var loginPage = m_app.GetLoginPage();
            m_app.Screenshot("I'm on the login page");

            loginPage.UserName = "jmayo";
            loginPage.Password = "cegedim";
            m_app.Screenshot("I've entered valid credentials");

            var dashboard = loginPage.SubmitCredentials();
            m_app.Screenshot("I'm on the dashboard page");
        }
    }
}
