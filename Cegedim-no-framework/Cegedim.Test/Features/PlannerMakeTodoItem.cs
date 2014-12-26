using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    [Category("BasicTest")]
    public class PlannerMakeTodoItem {
        MITouch m_miTouch;

        [Test()]
        public void MakeATodoItem() {
            var plannerPage = Background();
            m_miTouch.Screenshot("I'm on the planner page");

            var createTaskPopover = plannerPage.CreateNewTask();
            m_miTouch.Screenshot("I've opened the task options");

            var toDoPopover = createTaskPopover.CreateToDo();
            m_miTouch.Screenshot("I create a new To Do item");

            toDoPopover.Subject = "Lunch with Xamarin";
            m_miTouch.Screenshot("I set the item subject to 'Lunch with Xamarin'");

            toDoPopover.Description = "Make sure we buy them lunch";
            m_miTouch.Screenshot("I set the item description to 'Make sure we buy them lunch'");

            toDoPopover.DueDate = NextTuesday();
            m_miTouch.Screenshot("I set the date for next Tuesday");

            // TODO: Set the time for 11:30AM
            // todoPopover.DueTime = 11:30AM
            // m_miTouch.Screenshot("I set the time");

            toDoPopover.Reminder = "30 Minutes";
            m_miTouch.Screenshot("I set the reminder to 30 minutes before the meeting");

            toDoPopover.Channel = "Face to Face";
            m_miTouch.Screenshot("I set the channel to be a Face to Face meeting");

            toDoPopover.Type = "Lead";
            m_miTouch.Screenshot("I set the type to be a Lead");

            toDoPopover.AddAssigneeFromDatabase();
            m_miTouch.Screenshot("I assign the task to a teammate");

            var plannerPageRevisited = toDoPopover.DoneReturnToPlanner();
            m_miTouch.Screenshot("I am done creating the todo item and see the planner page");
        }

        public PlannerPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            var plannerPage = dashboard.NavigateToPlannerPage();
            return plannerPage;
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

