using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using System;
using System.Threading;
using System.Linq;
using NUnit.Framework;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cegedim.Automation {

    public class NotificationsPage : CegedimPage { 

        private static class Query {
            internal static string Loaded = "view id:'PAGE:home/homepagealerts.cdl'";
            internal static string Notifications = "tableView marked:'notifications' child tableViewCell";
            internal static string NotificationDetail = "view id:'__notificationDetail'";
            internal static string GeneralNotificationTable = "tableView marked:'notifications'";
            internal static string Back = "view marked:'Back'";
            internal static string Search = "view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string Delete = "view marked:'Delete'";
            internal static string HeaderView = "view:'Dendrite.IPhone.Forms.GridControl.CdlGridHeaderView'";
            internal static string SortButton = "view marked:'navSearch' descendant view:'Dendrite.IPhone.Forms.ShinyButton'";
            internal static string NotificationHeaderLabels = "view marked:'notifications' child view:'Dendrite.IPhone.Forms.GridControl.CdlGridHeaderView' child label";
        }

        internal NotificationsPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public int NotificationCount() {
            return Calabash.Query(Query.Notifications).Count();
        }

        public DashboardPage NavigateToDashboard() {
            return AppConvention.TapActivateAndWait<DashboardPage>(Application, Query.Back);
        }

        public string NotificationQuery(int index) {
            return string.Format("{0} index:{1}", Query.Notifications, index);
        }

        public string NotificationSubject(int index) {
            return Calabash.Query(string.Format("{0} descendant view:'Dendrite.IPhone.Forms.CdlDropDownList'", NotificationQuery(index))).First().Text;
        }

        // TODO: The ordering of the indexing to determine details and dates this way is reversed so I'm using the Ruby way of fetching from the db
//        public string NotificationDetail(int index) {
//            return Calabash.Query(string.Format("{0} descendant view:'Dendrite.IPhone.Forms.CdlMultiRowTextBoxRO'", NotificationQuery(index))).First().Text;
//        }

//        public string NotificationDate(int index) {
//            string dateQuery = string.Format("{0} sibling {1} descendant UILabel index:0", NotificationQuery(index), Query.HeaderView);
//            return Calabash.Query(dateQuery).First().Text;
//        }

        public string TopNotification() {
            return Calabash.Query(Query.NotificationHeaderLabels).First().Label;
        }

        public string BottomNotification() {
            return Calabash.Query(Query.NotificationHeaderLabels).Last().Label;
        }

        public void SelectNotification(int index) {
            TapAndWait(NotificationQuery(index), () => TestIsVisible(Query.NotificationDetail), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        public void SelectFirstNotification() {
            string notificationDetail = NotificationDetail();
            string detailQuery = string.Format("view:'Dendrite.IPhone.Forms.CdlMultiRowTextBoxRO' marked:'{0}' index:0", notificationDetail);
            TapAndWait(detailQuery, () => TestIsVisible(Query.NotificationDetail), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        public string NotificationDate() {
            var firstDetail = FetchNotificationDetails();
            string notificationDate = firstDetail["vc_date"].ToString();
            return notificationDate;
        }

        public string NotificationDetail() {
            var firstDetail = FetchNotificationDetails();
            string notificationDetail = firstDetail["vc_description"].ToString();
            return notificationDetail;
        }

        // TODO: Because of this Fetch for details and dates only confirmation will work if you tap the first notification
        public JToken FetchNotificationDetails() {
            JArray notificationDetails = JArray.Parse(Calabash.SelectNotificationDetails());
            JToken firstDetail = null;
            foreach(var detail in notificationDetails) {
                if (detail["__sort_order"].ToString() == "1") {
                    firstDetail = detail;
                    break;
                }
            }
            return firstDetail;
        }

        public void ConfirmNotificationDetails() {
            // TODO: The indexing of the header dates and the tableviewcells are opposite
            SelectFirstNotification();
            Wait(() => TestIsVisible(string.Format("webDocumentView text:'{0}'", NotificationDetail())));
            string notificationDate = NotificationDate();
            char[] parseChar = { ',', ' ' };
            string[] rawDate = notificationDate.Split(parseChar);
            // because I found a test case where Nov 04 would search for 04 in the details instead of just 4
            string day = rawDate[3];
            string depictedDay;
            if (day[0] == '0')
                depictedDay = day.Remove(0, 1);
            else
                depictedDay = day;
            string date = rawDate[0] + " " + depictedDay;
            string monthYear = rawDate[2] + " " + rawDate[5];
            string dateQuery = string.Format("textFieldLabel marked:'{0}'", date);
            string monthYearQuery = string.Format("textFieldLabel marked:'{0}'", monthYear);
            if (!TestIsVisible(dateQuery) || !TestIsVisible(monthYearQuery))
                Assert.Fail("Date or Month not shown on page");
        }

        public void SortNotifications() {
            // Causes only an animation but view may not change
            TapAndWait(Query.SortButton, () => TestIsVisible("* index:0"), postTimeout: TimeSpan.FromSeconds(0.4));
        }

        public void SearchFor(string subject) {
            SetField(Query.Search, subject);
            Calabash.Tap(CalabashButton.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
            HideKeyboard();
            Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.4));
        }
    }
}
