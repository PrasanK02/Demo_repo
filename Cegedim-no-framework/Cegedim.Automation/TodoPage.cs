using System;
using System.Linq;
using System.Threading;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using NUnit.Framework;

namespace Cegedim.Automation {

    public class TodoPage : CegedimPage { 
        private string m_todoType;
        private static class Query {
            internal static string Loaded = "view id:'PAGE:collaborativetools/todolist.cdl'";
            internal static string Received = "view marked:'Received'";
            internal static string Personal = "view marked:'Personal'";
            internal static string Sent = "view marked:'Sent'";
            internal static string SegmentedBar = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar'";
            internal static string TodoItem = "view marked:'navItems' descendant tableViewCell descendant textFieldLabel";
            internal static string AddButton = "view marked:'addButton'";
            internal static string Delete = "view marked:'Delete'";
            internal static string SecondDelete = "view:'_UIModalItemTableViewCell' marked:'Delete'";
            internal static string Edit = "view marked:'Edit'";
            internal static string MoreDetails = "view marked:'More Details'";
            internal static string FewerDetails = "view marked:'Fewer Details'";
            internal static string TapOverHere = "view marked:'Tap over here to view a To Do'";
        }

        internal TodoPage(MITouch application, AppContainer container)
            : base(application, container) {
        }


        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public string TodoType {
            get { return m_todoType; }
            set {
                if (value == "Received") 
                    TapAndWait(Query.Received, () => TestIsVisible(Query.SegmentedBar + "id:'VAL:1'"));
                else if (value == "Personal")
                    TapAndWait(Query.Personal, () => TestIsVisible(Query.SegmentedBar + "id:'VAL:0'"));
                else if (value == "Sent")
                    TapAndWait(Query.Sent, () => TestIsVisible(Query.SegmentedBar + "id:'VAL:2'"));
                else 
                    Assert.Fail("Todo type not available.");
                m_todoType = value;
                Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
            }
        }

        public string TodoItem(string subject) {
            return string.Format("{0} {{text LIKE '{1}'}}", Query.TodoItem, subject);
        }

        public void ConfirmTodoItem(string subject) {
            string todoItemQuery = TodoItem(subject);
            if (!TestIsVisible(todoItemQuery))
                Assert.Fail("Todo item specified is not visible");
        }

        public void ConfirmNoTodoItem(string subject) {
            string todoItemQuery = TodoItem(subject);
            if (TestIsVisible(todoItemQuery))
                Assert.Fail("The deleted todo item specified is still visible");
        }

        public void DeleteAllTodosWithSubject(string todoSubject) {
            var todos = JArray.Parse(Calabash.SelectTodo(todoSubject));
            foreach(var todo in todos) {
                // TODO: Investigate if this should be personal or received
                TodoType = "Personal";
                DeleteTodoItem(todoSubject);
            }
        }

        public void DeleteTodoItem(string todoSubject) {
            TapAndWait(TodoItem(todoSubject), () => TestIsVisible(Query.Delete), postTimeout: TimeSpan.FromSeconds(0.3));
            TapAndWait(Query.Delete, () => TestIsVisible(Query.SecondDelete), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(Query.SecondDelete, () => !TestIsVisible(Query.SecondDelete), timeout: TimeSpan.FromSeconds(15));
        }

        public void MoreDetails() {
            TapAndWait(Query.MoreDetails, () => TestIsVisible(Query.FewerDetails));
        }

        public void FewerDetails() {
            TapAndWait(Query.FewerDetails, () => TestIsVisible(Query.MoreDetails));
        }

        public void ConfirmDetail(string detail) {
            string detailQuery = string.Format("view marked:'{0}'", detail);
            AssertElementExists(detailQuery);
        }

        public TodoPopover CreateTodoPopover() {
            return AppConvention.TapActivateAndWait<TodoPopover>(Application, Query.AddButton);
        }

        public TodoPopover EditTodoPopover(string subject) {
            TapAndWait(TodoItem(subject), () => TestIsVisible(Query.Edit), postTimeout: TimeSpan.FromSeconds(0.5));
            return AppConvention.TapActivateAndWait<TodoPopover>(Application, Query.Edit);
        }
    }

    public class TodoPopover : DropShadowPopover {
        private string m_subject;
        private string m_description;
        private string m_channel;
        private string m_type;
        private string m_reminder;
        private string m_customerDatabase;
        private string m_customerTeamId;
        private DateTime m_date;
        private Lazy<Calendar> m_calendar;
        public static string AssigneeName = "Not set";
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'To Do'";
            internal static string DropShadowView = "UIDropShadowView";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string DueDate = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'__dueDateEdit'";
            internal static string AddAssignee = "view id:'-ToDoAssignment-' descendant view {accessibilityIdentifier contains 'add'}";
            internal static string ScrollableContent = "UIDropShadowView descendant view:'Dendrite.IPhone.Forms.UIScrollableContent'";
            internal static string SearchAllAttendee = "UILabel marked:'Search All'";
            internal static string SearchField = "UITextFieldLabel marked:'Search' index:0";
            internal static string Done = "UINavigationButton marked:'Done'";
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
            internal static string TableViewCell = "UITableViewCell";
        }

        internal TodoPopover(MITouch application, AppContainer container)
            : base(application, container) {
            m_calendar = new Lazy<Calendar>(() => CalendarFactory(Query.Popover));
            Random random = new Random();
            int sampleValue = random.Next(0, 9); // In the range of the sample size
            m_customerDatabase = JArray.Parse(Calabash.SelectCustomerNames(10))[sampleValue]["display_name"].ToString();
            m_customerTeamId = JArray.Parse(Calabash.SelectUserAccountDetails("Jmayo")).First["team_id"].ToString();
        }

        public override bool IsLoaded {
            get {
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                return TestIsVisible(Query.Loaded);
            }
        }

        private Calendar CalendarFactory(string query) {
            return new Calendar(Application, this, Container.GetDescendant(query));
        }

        public string Subject {
            get {
                return m_subject;
            }
            set {
                AddText("Subject", value, clearText: true, tapActionKey: true);
                m_subject = value;
            }
        }

        public string Description {
            get {
                return m_description;
            }
            set {
                AddText("Description", value);
                m_description = value;
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
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

        public string Type {
            get {
                return m_type;
            }
            set {
                SelectDropDown("Type", value, postTimeout: TimeSpan.FromSeconds(0.5));
                m_type = value;
            }
        }

        public DateTime DueDate {
            get {
                return m_date;
            }
            set {
                TapAndWait(Query.DueDate, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1.5));
                m_calendar.Value.Year = value.Year;
                m_calendar.Value.Month = value.ToString("MMMM");
                m_calendar.Value.Day = value.Day;
                m_date = value;
            }
        }

        public string Reminder {
            get {
                return m_reminder;
            }
            set {
                SelectDropDown("Reminder", value, postTimeout: TimeSpan.FromSeconds(1));
                m_reminder = value;
            }
        }

        public void AddAssigneeFromDatabase() {
            if (Calabash.Query(Query.AddAssignee).Count() == 0)
                SwipeDownUntil(Query.ScrollableContent, () => Calabash.Query(Query.AddAssignee).Count() > 0, postTimeout: TimeSpan.FromSeconds(2.5), ratio: 0.8);
            TapAndWait(Query.AddAssignee, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
            string assigneeResult = Calabash.SelectToDoAssignees(m_customerTeamId, 10);
            string assigneeName = (JArray.Parse(assigneeResult).First["display_name"]).ToString();
            AssigneeName = assigneeName;
            char[] stringSeparator = new char[2];
            stringSeparator[0] = ',';
            stringSeparator[1] = ' ';
            // Select last name. For example: Don't search Ali, Nadir but instead search Ali then select Ali, Nadir
            string parsedAssigneeName = assigneeName.Split(stringSeparator)[0];
            TapAndWait(Query.SearchField, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(1.5));
            SetField(Query.SearchField, parsedAssigneeName);
            Calabash.Tap(CalabashButton.Enter);
            string checkBoxQuery = string.Format("view marked:'{0}' parent {1} index:0 descendant {2}", assigneeName, Query.TableViewCell, Query.CheckBox);
            Wait(() => TestIsVisible(checkBoxQuery), postTimeout: TimeSpan.FromSeconds(0.7));
            string checkedBox = string.Format("{0} marked:'VAL:True'", checkBoxQuery);
            TapAndWait(checkBoxQuery, () => TestIsVisible(checkedBox));
            TapAndWait(Query.Done, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(2));
        }

        public PlannerPage DoneReturnToPlanner() {
            return AppConvention.TapActivateAndWait<PlannerPage>(
                Application, Query.Done
            );
        }

        public TodoPage DoneReturnToTodoPage() {
            return AppConvention.TapActivateAndWait<TodoPage>(
                Application, Query.Done
            );
        }
    }
}