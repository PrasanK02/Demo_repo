using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    [Category("BasicTest")]
    public class CallDependentData {
		private iOSApp app;

        [Test()]
        public void CreateAndFinishDataDependentCall() {
            var callPage = Background();
            app.Screenshot("I am on the call page");

//            callPage.DetailFirstProduct();
//            callPage.AddProfiledAttendee(2);
//            callPage.AddSpeaker();
//            m_miTouch.Screenshot("I've added a speaker");
//
//            callPage.CallPurpose = "Ordering"; // Set the Call Purpose
//            m_miTouch.Screenshot("I've added a call purpose");
//
//            callPage.NavigateToPostCallTab();
//            callPage.AddCallDialogues();
//            m_miTouch.Screenshot("I've added a call dialog");
//
//            var searchPage = callPage.Finish();
//            var dashboardPage = searchPage.NavigateToDashboardPage();
//            var plannerPage = dashboardPage.NavigateToPlannerPage();
//            m_miTouch.Screenshot("I should be able to complete a data dependent call");
//
//            plannerPage.VerifyCalls();
//            m_miTouch.Screenshot("I see the call was recorded correctly");
        }

        public NewCallPage Background(){
			app = NewGlobals.App;
			var dashboardPage = NewGlobals.QuickSetUp ();
            var searchPage = dashboardPage.NavigateToSearchPage();

            // Does some database queries to search for a customer
            var callPage = searchPage.NavigateToCallPage();
            return callPage;
        }
    }
}