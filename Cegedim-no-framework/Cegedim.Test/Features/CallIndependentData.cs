using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture]
    //[Category("BasicTest")]
    [Category("CallIndependentData")]
    public class CallIndependentData : BaseTestFixture {
        //MITouch m_miTouch;

        [Test]
        public void CreateAndFinishDataIndependentCall() {
            var callPage = Background();
            m_miTouch.Screenshot("I am on the call page");

            callPage.DetailFirstProduct();
            callPage.AddProfiledAttendee(2);
            callPage.SelectSampleRequest();
            callPage.AddSampleRequest();
            m_miTouch.Screenshot("I've added the sample request");

            callPage.AddMedicalInquiry();
            m_miTouch.Screenshot("I've added a medical inquiry");

            callPage.AddCallObjective();
            m_miTouch.Screenshot("I've added call objective");

            callPage.AddItemDisbursed(5);
            m_miTouch.Screenshot("I've added the item disbursed");

            callPage.AddCallNotes("This doctor is very important to us");
            m_miTouch.Screenshot("I've add a call note");

            var callToDoPopover = callPage.CreateToDo();
            callToDoPopover.Subject = "Present Test Cloud";
            callToDoPopover.Description = "Make sure we cover all gotchas";
            callToDoPopover.DueDate = DateTime.Now;
            callToDoPopover.Reminder = "30 Minutes";
            callToDoPopover.Channel = "Face to Face";
            callToDoPopover.ShowInPlanner = true;
            callToDoPopover.Done();
            m_miTouch.Screenshot("I've added a To Do task");

            var marketingRequestPopover = callPage.CreateMarketingRequest();
            marketingRequestPopover.RequestType = "Loyalty Card";
            // Select product by index since product names are long
            marketingRequestPopover.Product = 0;
            marketingRequestPopover.ExpectedDeliveryDate = NextTuesday();
            marketingRequestPopover.Quantity = 100;
            // Select location by index since the location names are really long
            marketingRequestPopover.Location = 0;
            marketingRequestPopover.Notes = "Marketing purposes";
            marketingRequestPopover.Done();
            m_miTouch.Screenshot("I've added a marketing request");

            var nonProfiledAttendeePopover = callPage.CreateNonProfiledAttendees();
            nonProfiledAttendeePopover.Title = "Physician's Assistant";
            nonProfiledAttendeePopover.Name = "PA";
            nonProfiledAttendeePopover.Role = "Owner";
            nonProfiledAttendeePopover.Notes = "PA Notes";
            nonProfiledAttendeePopover.Done();
            m_miTouch.Screenshot("I've added a non profiled attendee");

            var expensesPopover = callPage.CreateExpenses();
            expensesPopover.Product = "Azor";
            expensesPopover.Next();
            expensesPopover.Type = "Entertainment Expenses";
            expensesPopover.Allocation = "Abbott, Lisa";
            expensesPopover.Next();
            expensesPopover.Amount = 509;
            expensesPopover.OverallTradeSecret = true;
            expensesPopover.Reason = "This is a trade expense secret, I am not going to tell anyone";
            expensesPopover.Done();
            m_miTouch.Screenshot("I've added an expense");

            var sampleDisbursedPopover = callPage.CreateSampleDisbursed();
            sampleDisbursedPopover.AddSampleFromDatabase();
            sampleDisbursedPopover.Done();
            m_miTouch.Screenshot("I've disbursed sample product");

            var random = new Random();
            var randNum = random.Next(1000);
            callPage.DocumentId = randNum;
            m_miTouch.Screenshot("I've added a document Id and see the Save for Later button");

            var callDetailsPopover = callPage.CreateCallDetails();
            callDetailsPopover.Duration = 25;
            callDetailsPopover.Done();
            callPage.AccompaniedBy = "District Manager";
            callPage.SignCallDetails();
            m_miTouch.Screenshot("I've added the call details");

            var searchPage = callPage.Finish();
            var dashboardPage = searchPage.NavigateToDashboardPage();
            var plannerPage = dashboardPage.NavigateToPlannerPage();
            m_miTouch.Screenshot("I should be able to complete a data independent call");

            plannerPage.VerifyCalls();
            m_miTouch.Screenshot("I see the call was recorded correctly");
        }

        [Test()]
        public void SwitchCustomerForTheCall() {
            var callPage = Background();
            m_miTouch.Screenshot("I am on the call page");

            callPage.SwitchCustomerFromDatabase();
            m_miTouch.Screenshot("I've switched the primary customer on the call");

            callPage.DetailFirstProduct();
            m_miTouch.Screenshot("I've detailed the first product");

            var searchPage = callPage.Finish();
            var dashboardPage = searchPage.NavigateToDashboardPage();
            var plannerPage = dashboardPage.NavigateToPlannerPage();
            m_miTouch.Screenshot("I was able to finish the call for the new customer");

            plannerPage.VerifyCalls();
            m_miTouch.Screenshot("I see the call was recorded correctly after switching customer");
        }

        [Test()]
        public void IncompleteCall() {
            var callPage = Background();
            m_miTouch.Screenshot("I am on the call page");

            callPage.DetailFirstProduct();
            m_miTouch.Screenshot("I've detailed the first product");

            var callReadPage = callPage.SaveForLater();
            m_miTouch.Screenshot("I should be able to save the call");

            var searchPage = callReadPage.NavigateToSearchPage();
            var dashboardPage = searchPage.NavigateToDashboardPage();
            var incompleteCallsPage = dashboardPage.NavigateToIncompleteCallsPage();
            m_miTouch.Screenshot("I should be able to see the call from the incomplete call page");

            incompleteCallsPage.FinishIncompleteCall();
            var dashboardPageRevisited = incompleteCallsPage.NavigateToDashboardPage();
            var plannerPage = dashboardPageRevisited.NavigateToPlannerPage();
            m_miTouch.Screenshot("I should be able to finish the call");

            plannerPage.VerifyFirstCall(); // For this scenario only one customer is added
            m_miTouch.Screenshot("I should see the call was recorded correctly for incomplete call scenario");
        }

        public CallPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            var searchPage = dashboard.NavigateToSearchPage();
            // Does some database queries to search for a customer
            var callPage = searchPage.NavigateToCallPage();
            return callPage;
        }

        // Helper to get date time of next tuesday
        public DateTime NextTuesday() {
            var currentDateTime = DateTime.Now;
            var daysUntilNextTuesday = ((int)DayOfWeek.Tuesday - (int)currentDateTime.DayOfWeek + 7) % 7;
            if (daysUntilNextTuesday == 0)
                daysUntilNextTuesday = 7;
            // Set Next Tuesday at 11:30 AM
            var nextTuesday = currentDateTime.AddDays((double)daysUntilNextTuesday).Date + new TimeSpan(11, 30, 0);
            return nextTuesday;
        }
    }
}