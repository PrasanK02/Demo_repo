using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    public class PlannerMiniCalendar {
        MITouch m_miTouch;

        [Test()]
        public void ManipulateTheMiniCalendar() {
            PlannerPage plannerPage = Background();
            m_miTouch.Screenshot("I see the planner page");

            plannerPage.Date = DateTime.Now.AddMonths(-1);
            m_miTouch.Screenshot("I use the mini calendar to go back one month");

            plannerPage.Date = DateTime.Now.AddMonths(1);
            m_miTouch.Screenshot("I use the mini calendar to go forward one month");

            plannerPage.Date = DateTime.Now.AddYears(-1);
            m_miTouch.Screenshot("I use the mini calendar to go back one year");

            plannerPage.Date = DateTime.Now.AddYears(1);
            m_miTouch.Screenshot("I use the mini calendar to go forward one year");

            var fifthOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5);
            plannerPage.Date = fifthOfTheMonth;
            m_miTouch.Screenshot("I use the mini calendar to go to the 5th of the month");

            var firstNextMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            var daysUntilTuesday = (((int)DayOfWeek.Tuesday - (int)firstNextMonthDate.DayOfWeek) + 7) % 7;
            var firstTuesdayNextMonth = firstNextMonthDate.AddDays(daysUntilTuesday);
            plannerPage.Date = firstTuesdayNextMonth;
            m_miTouch.Screenshot("I use the mini calendar to go to the first Tuesday of the next month");

            var specifiedDate = new DateTime(2013, 1, 10);
            plannerPage.Date = specifiedDate;
            m_miTouch.Screenshot("I use the mini calendar to go to the 10th of January 2013");

            var monthPopover = plannerPage.CreateMonthPopover();
            m_miTouch.Screenshot("I open the month popover");

            monthPopover.Month = "August";
            m_miTouch.Screenshot("I use the months popover to change the month to August");

            var secondMonthPopover = plannerPage.CreateMonthPopover();
            m_miTouch.Screenshot("I open the month popover");

            secondMonthPopover.Month = DateTime.Now.ToString("MMMM");
            m_miTouch.Screenshot("I use the months popover to go back to the original month");

            // reset to today
            plannerPage.Date = DateTime.Now;
            var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");
            plannerPage.SwipeBackMonth();
            plannerPage.VerifyMonth(previousMonth);
            m_miTouch.Screenshot("I swipe the mini calendar to the previous month");

            plannerPage.Date = DateTime.Now;
            var nextMonth = DateTime.Now.AddMonths(1).ToString("MMMM");
            plannerPage.SwipeForwardMonth();
            plannerPage.VerifyMonth(nextMonth);
            m_miTouch.Screenshot("I swipe the mini calendar to the next month");
        }

        public PlannerPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            PlannerPage plannerPage = dashboard.NavigateToPlannerPage();
            return plannerPage;
        }
    }
}

