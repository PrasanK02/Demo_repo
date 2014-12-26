using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    [Category("ExtendedTest")]
    public class ConvertAppointmentIntoCall {
        MITouch m_miTouch;

        [Test()]
        public void ConvertAnAppointmentIntoACall() {
            var plannerPage = Background();
            plannerPage.EnsureDayView();
            var callPage = plannerPage.ConvertAppointmentToCall();
            m_miTouch.Screenshot("I've begun to convert the appointment to a call");

            callPage.DetailFirstProduct();
            callPage.VerifyAppointmentDate(DateTime.Now);
            callPage.VerifyAppointment("Channel", "Phone");
            callPage.VerifyAppointment("Cycle", "None");
            callPage.VerifyAppointment("Make first contact");
            callPage.VerifyAppointmentAttendees();
            m_miTouch.Screenshot("I've detailed the first product and verified the appointment details");

            callPage.ManageInvitations();
            m_miTouch.Screenshot("I've managed the invitations");

            var plannerPageRevisited = callPage.FinishOnPlanner();
            plannerPageRevisited.VerifyCallAttendeeInDayView();
            m_miTouch.Screenshot("I've finished the call and verified attendee on the planner");
        }

        [Test()]
        public void ConvertAnAppointmentIntoAnExpressCall() {
            var plannerPage = Background();
            var dashboardPage = plannerPage.NavigateToDashboardPage();
            m_miTouch.Screenshot("I go to the dashboard page from the planner page");

//            dashboardPage.NavigateToExpressCallPage();
//            m_miTouch.Screenshot("I navigate to the express call page if available");
        }

        public PlannerPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            var plannerPage = dashboard.NavigateToPlannerPage();
            m_miTouch.Screenshot("I am on the planner page");

            var createTaskPopover = plannerPage.CreateNewTask();
            var newAppointmentPopover = createTaskPopover.CreateNewAppointment();
            m_miTouch.Screenshot("I create a new appointment task using the plus button");

            newAppointmentPopover.AddCustomerFromDatabase();
            m_miTouch.Screenshot("I select a customer for the appointment");

            newAppointmentPopover.Date = DateTime.Now;
            // newAppointmentPopover.Time = DateTime.Now.AddHours(5); // Add five hours from now - not yet implemented
            m_miTouch.Screenshot("I set the time for today");

            newAppointmentPopover.Duration = 20;
            m_miTouch.Screenshot("I set the duration for 20 minutes");

            newAppointmentPopover.Reminder = "30 Minutes";
            m_miTouch.Screenshot("I set the reminder to 30 Minutes");

            newAppointmentPopover.Cycle = "None";
            m_miTouch.Screenshot("I confirm that the cycle is set to None");

            newAppointmentPopover.Channel = "Phone";
            m_miTouch.Screenshot("I set the meeting channel to Phone");

            newAppointmentPopover.NextCallObjective = "Make first contact";
            m_miTouch.Screenshot("I set the next meeting objective");

            newAppointmentPopover.AddAttendeeFromDatabase();
            m_miTouch.Screenshot("I invite someone to the meeting");

            var plannerPageAfterAppointment = newAppointmentPopover.Done();
            m_miTouch.Screenshot("I'm done with creating the new appointment");

            plannerPageAfterAppointment.VerifyAppointment();
            m_miTouch.Screenshot("I should see the new appointment in the planner");

            return plannerPageAfterAppointment;
        }
    }
}

