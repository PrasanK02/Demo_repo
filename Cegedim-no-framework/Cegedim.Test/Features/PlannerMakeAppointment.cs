using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    public class PlannerMakeAppointment {
        MITouch m_miTouch;

        [Test()]
        public void MakeAnIntroductoryAppointment() {
            var plannerPage = Background();
            m_miTouch.Screenshot("I see the planner");

            var createTaskPopover = plannerPage.CreateNewTask();
            m_miTouch.Screenshot("I create a new task");

            var newAppointmentPopover = createTaskPopover.CreateNewAppointment();
            m_miTouch.Screenshot("I create a new appointment using the plus button");

            newAppointmentPopover.AddCustomerFromDatabase();
            m_miTouch.Screenshot("I select a customer for the appointment");

            newAppointmentPopover.Date = NextTuesday();
            m_miTouch.Screenshot("I set the date for the next Tuesday from now");

            // TODO: Set the time 
            // newAppointmentPopover.Time = 15:00
            // m_miTouch.Screenshot("I set the time to 15:00");

            newAppointmentPopover.Duration = 20;
            m_miTouch.Screenshot("I set the duration of the meeting to 20 minutes");

            newAppointmentPopover.Reminder = "30 Minutes";
            m_miTouch.Screenshot("I set a reminder for 30 minutes");

            newAppointmentPopover.Cycle = "None";
            m_miTouch.Screenshot("I confirm that the Cycle is None");

            newAppointmentPopover.Channel = "Phone";
            m_miTouch.Screenshot("I set the meeting Channel to Phone");

            newAppointmentPopover.Purpose = "Introduction";
            m_miTouch.Screenshot("I set the purpose of the meeting to Introduction");

            newAppointmentPopover.NextCallObjective = "Make first contact";
            m_miTouch.Screenshot("I set the next meeting objective to 'Make first contact'");

            newAppointmentPopover.AddAttendeeFromDatabase();
            m_miTouch.Screenshot("I invite someone to the meeting");

            var plannerPageRevisited = newAppointmentPopover.Done();
            m_miTouch.Screenshot("I finish creating the appointment and see the planner page again");

            plannerPageRevisited.Date = NextTuesday(); // This is the date of the next appointment
            plannerPageRevisited.VerifyAppointment();
            m_miTouch.Screenshot("I see the new appointment in the planner");
        }

        public PlannerPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            var plannerPage = dashboard.NavigateToPlannerPage();
            return plannerPage;
        }

        // Helper to get date time of next tuesday
        // TODO: I use this multiple times so I should refactor later 
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

