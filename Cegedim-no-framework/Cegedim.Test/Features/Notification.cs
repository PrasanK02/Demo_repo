using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    [Category("ExtendedTest")]
    public class Notification {
        MITouch m_miTouch;

        [Test()]
        public void BadgeCountNotificationShouldMatch() {
            var notificationsPage = Background();
            var notificationCount = notificationsPage.NotificationCount();
            m_miTouch.Screenshot("I see how many notifications I have");

            var dashboardPage = notificationsPage.NavigateToDashboard();
            m_miTouch.Screenshot("I go back to the dashboard");

            var dashboardNotificationCount = dashboardPage.CountNotifications();
            if (notificationCount != dashboardNotificationCount)
                Assert.Fail("Notifications badge count doesn't match the number of notifications");
            m_miTouch.Screenshot("The number of notifications should match the badge count on the dashboard");
        }

        [Test]
        public void ViewDetailsOfNotification() {
            var notificationsPage = Background();
            notificationsPage.SelectFirstNotification();
            m_miTouch.Screenshot("I tap the first notification");

            notificationsPage.ConfirmNotificationDetails();
            m_miTouch.Screenshot("I see the details for the notification");
        }

        [Test]
        public void SortNotification() {
            var notificationsPage = Background();
            string topNotification = notificationsPage.TopNotification();
            string bottomNotification = notificationsPage.BottomNotification();
            notificationsPage.SortNotifications();
            m_miTouch.Screenshot("I tap sort on the notifications");

            // TODO: Automate the testing of the sorted list
            m_miTouch.Screenshot("I see the list sorted");
        }

        [Test]
        public void SearchNotifications() {
            var notificationsPage = Background();
            string notificationDetail = notificationsPage.NotificationDetail();
            notificationsPage.SearchFor(notificationDetail);
            m_miTouch.Screenshot("I type the notification subject in search");

            notificationsPage.SelectFirstNotification();
            notificationsPage.ConfirmNotificationDetails();
            m_miTouch.Screenshot("I verify the notification item");
        }

        public NotificationsPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            var notificationsPage = dashboard.NavigateToNotificationsPage();
            m_miTouch.Screenshot("I navigate to the notifications page");

            return notificationsPage;
        }
    }
}

