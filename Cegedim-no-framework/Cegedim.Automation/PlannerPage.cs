using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.UITest;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using NUnit.Framework;

namespace Cegedim.Automation 
{
    public class PlannerPage : CegedimPage
    {
        private DateTime m_date;
        private Lazy<Calendar> m_calendar;
		private static class Query {
			internal static string Calendar = "view:'MI.CustomControls.MonthGridView'";
            internal static string NewTask = "view:'UILabel' marked:'+'";
            internal static string DayView = "view id:'PAGE:planner/plannerday.cdl'";
            internal static string WeekView = "view id:'PAGE:planner/plannerweek.cdl'";
            internal static string AttendeeAppointment = "tableView index:1 descendant tableViewCell descendant view";
            internal static string Day = "UILabel marked:'Day'";
            internal static string Week = "UILabel marked:'Week'";
            internal static string AppointmentCell = "view:'MI.CustomControls_NewControl.ApptCell'";
            internal static string CallIcon = "tableViewCell descendant imageView {accessibilityIdentifier ENDSWITH 'call.png'} index:0";
            internal static string HomeIcon = "UITabBarButton marked:'Home'";
            internal static string MonthButton = "MI.CustomControls.MonthsButton";
            internal static string DeleteIcon = "view {accessibilityIdentifier contains 'delete'}";
            internal static string DeleteAppointment = "view marked:'Delete Appointment' index:0";
            internal static string PlannerAppointment = "view {accessibilityIdentifier contains 'plannerappointmentday'}";
        }

        internal PlannerPage(MITouch application, AppContainer container)
            : base(application, container) {
            m_calendar = new Lazy<Calendar>(() => CalendarFactory("* index:0"));
        }

		public override bool IsLoaded {
			get { return AppConvention.TestIsVisible(Container, Query.Calendar); }
		}

        public DateTime Date {
            get {
                return m_date;
            }
            set {
                m_calendar.Value.Year = value.Year;
                m_calendar.Value.Month = value.ToString("MMMMM");
                m_calendar.Value.Day = value.Day;
                m_date = value;
            }
        }

        private Calendar CalendarFactory(string query) {
            return new Calendar(Application, this, Container.GetDescendant(query));
        }

        public CreateTaskPopover CreateNewTask() {
            return AppConvention.TapActivateAndWait<CreateTaskPopover>(
                Application, Query.NewTask
            );
        }

        public SelectMonthPopover CreateMonthPopover() {
            return AppConvention.TapActivateAndWait<SelectMonthPopover>(
                Application, Query.MonthButton
            );
        }

        public DashboardPage NavigateToDashboardPage() {
            return AppConvention.TapActivateAndWait<DashboardPage>(
                Application, Query.HomeIcon
            );
        }

        public void SwipeBackMonth() {
            var calendarRect = Calabash.Query(Query.Calendar).First().Rectangle;
            var centerY = calendarRect.Height / 2.0 + calendarRect.Y;
            var leftX = calendarRect.Width / 10.0 + calendarRect.X;
            var rightX = calendarRect.Width * 5.0 / 6.0 + calendarRect.X;
            var leftPoint = new Point((int)leftX, (int)centerY);
            var rightPoint = new Point((int)rightX, (int)centerY);
            Calabash.Pan(leftPoint, rightPoint);
        }

        public void SwipeForwardMonth() {
            var calendarRect = Calabash.Query(Query.Calendar).First().Rectangle;
            var centerY = calendarRect.Height / 2.0 + calendarRect.Y;
            var leftX = calendarRect.Width / 10.0 + calendarRect.X;
            var rightX = calendarRect.Width * 5.0 / 6.0 + calendarRect.X;
            var leftPoint = new Point((int)leftX, (int)centerY);
            var rightPoint = new Point((int)rightX, (int)centerY);
            Calabash.Pan(rightPoint, leftPoint);
        }

        public void VerifyMonth(string monthName) {
            string monthQuery = string.Format("{0} {{accessibilityIdentifier contains '{1}'}}", Query.Calendar, monthName);
            TestIsVisible(monthQuery);
        }
            
        public void VerifyCalls() {
            bool valid = false;
            var callCustomers = CallPage.CallCustomers;
           // var customerCount = callCustomers.Count();
            foreach (var callCustomer in callCustomers) {
                string customerId = callCustomer["customerid"].ToString();
                //string customerName = callCustomer["Name"].ToString();
                string queryString = String.Format("view marked:'KEY:{0}' index:0", customerId);
                if (!TestIsVisible(queryString))
                    throw new Exception("Customer id query not visible");
                var queryResults = Calabash.Query(queryString);
                int index = 0;
                foreach (var queryResult in queryResults) {
                    var callIconQuery = String.Format("view marked:'KEY:{0}' index:{1} imageView index:1", customerId, index);
                    var callIconResult = Calabash.Query(callIconQuery);
                    if (callIconResult.Count() == 0) {
                        callIconQuery = String.Format("view marked:'KEY:{0}' index:{1} imageView index:0", customerId, index);
                        callIconResult = Calabash.Query(callIconQuery);
                    }
                    foreach (var iconResult in callIconResult) {
                        if (iconResult.Id.Contains("plannercallday.png"))
                            valid = true;
                    }
                    index += 1;
                }
                if (!valid)
                    throw new Exception("call for customer not found");
            }
        }

        // For the incomplete call scenario I'm only adding one customer so use this for that scenario
        public void VerifyFirstCall() {
            bool valid = false;
            var callCustomer = CallPage.CallCustomers.First();
            string customerId = callCustomer["customerid"].ToString();
            //string customerName = callCustomer["Name"].ToString();
            string queryString = String.Format("view marked:'KEY:{0}' index:0", customerId);
            if (!TestIsVisible(queryString))
                throw new Exception("Customer id query not visible");
            var queryResults = Calabash.Query(queryString);
            int index = 0;
            foreach (var queryResult in queryResults) {
                var callIconQuery = String.Format("view marked:'KEY:{0}' index:{1} imageView index:1", customerId, index);
                var callIconResult = Calabash.Query(callIconQuery);
                if (callIconResult.Count() == 0) {
                    callIconQuery = String.Format("view marked:'KEY:{0}' index:{1} imageView index:0", customerId, index);
                    callIconResult = Calabash.Query(callIconQuery);
                }
                foreach (var iconResult in callIconResult) {
                    if (iconResult.Id.Contains("plannercallday.png"))
                        valid = true;
                }
                index += 1;
            }
            if (!valid)
                throw new Exception("call for customer not found");
        }

        public void EnsureDayView() {
            TapAndWait(Query.Day, () => TestIsVisible(Query.DayView));
            if (!TestIsVisible(Query.DayView))
                Assert.Fail("Not on the day view");
        }

        public void EnsureWeekView() {
            TapAndWait(Query.Week, () => TestIsVisible(Query.WeekView));
            if (!TestIsVisible(Query.WeekView))
                Assert.Fail("Not on the week view");
        }

        public void ClearOldAppointments(string query) {
            Func<bool> clearAppointments = () => {
                string rawIcon = string.Format("{0} parent tableViewCell descendant {1}", query, Query.PlannerAppointment);
                string attendee = PickAttendee(query);
                TapAndWait(attendee, () => TestIsVisible(Query.DeleteIcon), postTimeout: TimeSpan.FromSeconds(0.6));
                TapAndWait(Query.DeleteIcon, () => TestIsVisible(Query.DeleteAppointment), postTimeout: TimeSpan.FromSeconds(0.3));
                TapAndWait(Query.DeleteAppointment, () => !TestIsVisible(Query.DeleteAppointment), postTimeout: TimeSpan.FromSeconds(0.6));
                if (PossiblePlannerAttendees(query) > 1)
                    return false;
                else 
                    return true;
            };
            Wait(clearAppointments);
        }

        public int PossiblePlannerAttendees(string attendeeAppointment) {
            int plannerCount = Calabash.Query(attendeeAppointment).Count();
            int possibleAttendees = 0;
            for (int i = 0; i < plannerCount; i++) {
                string rawQuery = string.Format("{0} index:{1}", attendeeAppointment, i);
                string rawIcon = string.Format("{0} parent tableViewCell descendant {1}", rawQuery, Query.PlannerAppointment);
                if (TestIsVisible(rawIcon))
                    possibleAttendees += 1;
            }
            return possibleAttendees;
        }

        public string PickAttendee(string attendeeAppointment) {
            int counter = 0;
            Func<bool> pickAttendee = () => {
                string iconQuery = string.Format("{0} index:{1} parent tableViewCell descendant {2}", attendeeAppointment, counter.ToString(), Query.PlannerAppointment);
                if (TestIsVisible(iconQuery))
                    return true;
                else {
                    counter += 1;
                    return false;
                }
            };
            Wait(pickAttendee, timeout: TimeSpan.FromSeconds(25));
            string attendee = string.Format("{0} index:{1}", attendeeAppointment, counter.ToString());
            return attendee;
        }

        public void VerifyAttendeeInDayView() {
            string attendeeAppointmentQuery = string.Format("{0} marked:'{1}'", Query.AttendeeAppointment, NewAppointmentPopover.CustomerName);
            string attendeeAppointmentIcon = string.Format("{0} parent tableViewCell descendant {1}", attendeeAppointmentQuery, Query.PlannerAppointment);
            if (PossiblePlannerAttendees(attendeeAppointmentQuery) > 1)
                ClearOldAppointments(attendeeAppointmentQuery);
            string attendeeAppointment = PickAttendee(attendeeAppointmentQuery);
            if (!TestIsVisible(attendeeAppointment))
                Assert.Fail("Attendee appointment not seen");
        }

        public void VerifyCallAttendeeInDayView() {
            string attendeeAppointmentQuery = string.Format("{0} marked:'{1}' index:0", Query.AttendeeAppointment, NewAppointmentPopover.CustomerName);
            string attendeeAppointmentIcon = string.Format("{0} parent tableViewCell descendant {1}", attendeeAppointmentQuery, Query.PlannerAppointment);
            if (!TestIsVisible(attendeeAppointmentQuery))
                Assert.Fail("Attendee appointment not seen");
        }

        public void VerifyAttendeeInWeekView() {
            string attendeeAppointmentQuery = string.Format("{0} descendant view marked:'{1}' index:0", Query.AppointmentCell, NewAppointmentPopover.CustomerName);
            if (!TestIsVisible(attendeeAppointmentQuery))
                Assert.Fail("Attendee appointment not seen");
        }

        public void VerifyAppointment() {
            EnsureDayView();
            VerifyAttendeeInDayView();
            EnsureWeekView();
            VerifyAttendeeInWeekView();
        }

        public CallPage ConvertAppointmentToCall() {
            string attendeeAppointmentQuery = string.Format("{0} marked:'{1}'", Query.AttendeeAppointment, NewAppointmentPopover.CustomerName);
            string attendee = PickAttendee(attendeeAppointmentQuery);
            string callIcon = string.Format("{0} parent {1}", attendee, Query.CallIcon);
            return AppConvention.TapActivateAndWait<CallPage>(
                Application, callIcon
            );
        }
    }



    public class CreateTaskPopover : PlannerPage
    {
        private static class Query {
            internal static string Appointment = "tableViewCell descendant label marked:'Appointment'";
            internal static string Todo = "tableViewCell descendant view marked:'To Do'";
            internal static string OutOfTerritory = "tableViewCell descendant view marked:'Out of Territory'";
        }

        internal CreateTaskPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return AppConvention.TestIsVisible(Container, Query.Appointment); }
        }

        public TodoPopover CreateToDo() {
            return AppConvention.TapActivateAndWait<TodoPopover>(
                Application, Query.Todo
            );
        }

        public NewAppointmentPopover CreateNewAppointment() {
            return AppConvention.TapActivateAndWait<NewAppointmentPopover>(
                Application, Query.Appointment
            );
        }
    }

    public class NewAppointmentPopover : DropShadowPopover {
        private Lazy<Calendar> m_calendar;
        private DateTime m_date;
        private DateTime m_time;
        private string m_customer;
        private string m_customerDatabase; // Name from Database
        private int m_duration;
        private string m_reminder;
        private string m_cycle;
        private string m_channel;
        private string m_nextCallObjective;
        private string m_purpose;
        public static string CustomerName = "Not set";
        public static string AttendeeName = "Not set";

        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'New Appointment'";
            internal static string DropShadowView = "UIDropShadowView";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string NewCustomer = "view id:'newcust' descendant view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string SearchField = "UIFieldEditor";
            internal static string TableViewCell = "UITableViewCell";
            internal static string Date = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'_date_'";
            internal static string Time = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'__appointment_time'";
            internal static string AddAttendee = "view marked:'attendees' child view id:'__prodfileAttendeeAddColumn'";
            internal static string SearchAllAttendee = "UILabel marked:'Search All'";
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
            internal static string Done = "UINavigationButton marked:'Done'";
            internal static string ScrollableContent = "UIDropShadowView descendant view:'Dendrite.IPhone.Forms.UIScrollableContent'";
        }

        internal NewAppointmentPopover(MITouch application, AppContainer container)
            : base(application, container) {
            m_calendar = new Lazy<Calendar>(() => CalendarFactory(Query.Popover));
            Random random = new Random();
            int sampleValue = random.Next(0, 9); // In the range of the sample size
            m_customerDatabase = JArray.Parse(Calabash.SelectCustomerNames(10))[sampleValue]["display_name"].ToString();
        }

        public override bool IsLoaded {
            get {
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                return TestIsVisible(Query.Loaded);
            }
        }

        public string Customer {
            get {
                return m_customer;
            }
            set {
                TapAndWait(Query.NewCustomer, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1.5));
                char[] stringSeparator = new char[2];
                stringSeparator[0] = ',';
                stringSeparator[1] = ' ';
                // Select last name. For example: Don't search Ali, Nadir but instead search Ali then select Ali, Nadir
                string parsedName = value.Split(stringSeparator)[0];
                SetField(Query.SearchField, parsedName);
                Calabash.Tap(CalabashButton.Enter);
                string customerQuery = string.Format("{0} descendant view marked:'{1}' index:0", Query.TableViewCell, value);
                Wait(() => TestIsVisible(customerQuery), postTimeout: TimeSpan.FromSeconds(1));
                TapAndWait(customerQuery, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
                m_customer = value;
                CustomerName = m_customer;
            }
        }

        public void AddCustomerFromDatabase() {
            Customer = m_customerDatabase;
        }

        private Calendar CalendarFactory(string query) {
            return new Calendar(Application, this, Container.GetDescendant(query));
        }

        public DateTime Date {
            get {
                return m_date;
            }
            set {
                TapAndWait(Query.Date, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1.5));
                m_calendar.Value.Year = value.Year;
                m_calendar.Value.Month = value.ToString("MMMM");
                m_calendar.Value.Day = value.Day;
                Wait(() => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
                m_date = value;
            }
        }

        // TODO: Finish implementation of the time setter (no current time picker helpers in C# to my knowlodge 09-08-2014)
        public DateTime Time {
            get {
                return m_time;
            }
            set {
                //TapAndWait(Query.Time, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(2));
            }
        }

        public int Duration {
            get {
                return m_duration;
            }
            set {
                AddText("Duration (Min)", value.ToString(), clearText: true, tapActionKey: true);
                Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                m_duration = value;
            }
        }

        public string Reminder {
            get {
                return m_reminder;
            }
            set {
                SelectDropDown("Reminder", value, clearExisting: true, postTimeout: TimeSpan.FromSeconds(1));
                m_reminder = value;
            }
        }

        public string Cycle {
            get {
                return m_cycle;
            }
            set {
                SelectDropDown("Cycle", value);
                m_cycle = value;
            }
        }

        public string Channel {
            get {
                return m_channel;
            }
            set {
                SelectDropDown("Channel", value, postTimeout: TimeSpan.FromSeconds(0.5));
                m_channel = value;
            }
        }

        public string NextCallObjective {
            get {
                return m_nextCallObjective;
            }
            set {
                AddText("Next Call Objective", value);
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1.5)); // step pause
            }
        }

        public string Purpose {
            get {
                return m_purpose;
            }
            set {
                SelectDropDown("Purpose", value, clearExisting: true, postTimeout: TimeSpan.FromSeconds(0.5));
                m_purpose = value;
            }
        }

        public void AddAttendeeFromDatabase() {
            if (Calabash.Query(Query.AddAttendee).Count() == 0)
                SwipeDownUntil(Query.ScrollableContent, () => Calabash.Query(Query.AddAttendee).Count() > 0, postTimeout: TimeSpan.FromSeconds(2.5), ratio: 0.8);
            TapAndWait(Query.AddAttendee, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
            TapAndWait(Query.SearchAllAttendee, () => TestIsVisible(Query.SearchField), postTimeout: TimeSpan.FromSeconds(1));
            string attendeeName = Calabash.SelectAttendeeNames(m_customerDatabase, 10);
            AttendeeName = attendeeName;
            char[] stringSeparator = new char[2];
            stringSeparator[0] = ',';
            stringSeparator[1] = ' ';
            // Select last name. For example: Don't search Ali, Nadir but instead search Ali then select Ali, Nadir
            string parsedAttendeeName = attendeeName.Split(stringSeparator)[0];
            SetField(Query.SearchField, parsedAttendeeName);
            Calabash.Tap(CalabashButton.Enter);
            string checkBoxQuery = string.Format("view marked:'{0}' parent {1} index:0 descendant {2}", attendeeName, Query.TableViewCell, Query.CheckBox);
            Wait(() => TestIsVisible(checkBoxQuery), postTimeout: TimeSpan.FromSeconds(0.7));
            string checkedBox = string.Format("{0} marked:'VAL:True'", checkBoxQuery);
            TapAndWait(checkBoxQuery, () => TestIsVisible(checkedBox));
            TapAndWait(Query.Done, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(2));
        }

        public PlannerPage Done() {
            return AppConvention.TapActivateAndWait<PlannerPage>(
                Application, Query.Done
            );
        }

//        public string RequestType {
//            get {
//                return m_requestType;
//            }
//            set {
//                SelectDropDown("Request Type", value);
//                m_requestType = value;
//            }
//        }
    }

    public class SelectMonthPopover : Popover {
        private string m_month;
        private static class Query {
            internal static string Loaded = "view:'_UIPopoverView'";
        }
        internal SelectMonthPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public string Month {
            get {
                return m_month;
            }
            set {
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                string monthQuery = string.Format("{0} descendant view marked:'{1}'", Query.Loaded, value);
                TapAndWait(monthQuery, () => !TestIsVisible(Query.Loaded), postTimeout: TimeSpan.FromSeconds(1));
                TestIsVisible(string.Format("view {{accessibilityIdentifier contains '{0}'}}", value));
            }
        }
    }
}

