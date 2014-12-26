using System;
using System.Threading;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;

namespace Cegedim.Automation {

    internal sealed class ImageCheckBox : CegedimSubPage {

        private static class Query {
            internal static string CheckBox = "view:'MI.CustomControls.CdlIPhoneImageControl'";
            internal static string CheckBoxOn = CheckBox + " marked:'VAL:True'";
        }

        internal ImageCheckBox(MITouch application, CegedimEntity parent, AppElement element)
            : base(application, parent, element) {
        }

        public override bool IsLoaded {
            get { return true; }
        }

        public bool IsChecked {
            get { return TestIsVisible(Query.CheckBoxOn); }
            set {
                if (value == IsChecked)
                    return;

                TapAndWait(Query.CheckBox, () => IsChecked == value);
            }
        }
    }

    public class CustomerIdentityPage : CustomerPage {

        private static class Query {
            internal static string Loaded = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'Identity'";
            internal static string GeneralPopover = "view:'CdlView' id:'PAGE:pres/identity.cdl' index:1";
            internal static string Done = "* marked:'Done' index:0";
            internal static string TexasRoute = "* marked:'Texas route' parent view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell'";
            internal static string AddedRoute = "* marked:'Added Route' parent view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell'";
            internal static string PARoute = "* marked:'PA Route' parent view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell'";
            internal static string TestRoute = "* marked:'Test' parent view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell'";
            internal static string EditItem = "view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string DropDownList = "view:'Dendrite.IPhone.Forms.CdlDropDownList'";
            internal static string TextField = "view:'Dendrite.IPhone.Forms.CdlTextBox'";
            internal static string TableViewList = "view:'UITableView' marked:'Empty list'";
            internal static string TableRows = "view:'Dendrite.IPhone.Forms.CdlTableViewCell'";
            internal static string ClearText = "view:'UITextField' isFirstResponder:1 child view:'UIButton'";
            internal static string ClearButton = "view:'UINavigationButton' marked:'Clear'";
            internal static string WrapperView = "ViewControllerWrapperView";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string PopoverTable = Popover + " descendant view:'UITableView'";
            internal static string VisitContact = GeneralPopover + " descendant view:'UILabel' marked:'Visit' parent view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string CallContact = GeneralPopover + " descendant view:'UILabel' marked:'Call' parent view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string FaxContact = GeneralPopover + " descendant view:'UILabel' marked:'Fax' parent view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string MailContact = GeneralPopover + " descendant view:'UILabel' marked:'Mail' parent view:'Dendrite.IPhone.Forms.CdlEditItem'";
            internal static string EmailContact = GeneralPopover + " descendant view:'UILabel' marked:'Email' parent view:'Dendrite.IPhone.Forms.CdlEditItem'";
        }

        private Lazy<CheckBox> m_TexasRoute;
        private Lazy<CheckBox> m_AddedRoute;
        private Lazy<CheckBox> m_PARoute;
        private Lazy<CheckBox> m_TestRoute;
        private Lazy<ImageCheckBox> m_VisitContact;
        private Lazy<ImageCheckBox> m_CallContact;
        private Lazy<ImageCheckBox> m_FaxContact;
        private Lazy<ImageCheckBox> m_MailContact;
        private Lazy<ImageCheckBox> m_EmailContact;

        internal CustomerIdentityPage(MITouch application, AppContainer container)
            : base(application, container) {
            m_TexasRoute = new Lazy<CheckBox>(() => CheckBoxFactory(Query.TexasRoute));
            m_AddedRoute = new Lazy<CheckBox>(() => CheckBoxFactory(Query.AddedRoute));
            m_PARoute = new Lazy<CheckBox>(() => CheckBoxFactory(Query.PARoute));
            m_TestRoute = new Lazy<CheckBox>(() => CheckBoxFactory(Query.TestRoute));
            m_VisitContact = new Lazy<ImageCheckBox>(() => ImageCheckBoxFactory(Query.VisitContact));
            m_CallContact = new Lazy<ImageCheckBox>(() => ImageCheckBoxFactory(Query.CallContact));
            m_FaxContact = new Lazy<ImageCheckBox>(() => ImageCheckBoxFactory(Query.FaxContact));
            m_MailContact = new Lazy<ImageCheckBox>(() => ImageCheckBoxFactory(Query.MailContact));
            m_EmailContact = new Lazy<ImageCheckBox>(() => ImageCheckBoxFactory(Query.EmailContact));
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }  

        private CheckBox CheckBoxFactory(string query) {
            return new CheckBox(Application, this, Container.GetDescendant(query));
        }

        private ImageCheckBox ImageCheckBoxFactory(string query) {
            return new ImageCheckBox(Application, this, Container.GetDescendant(query));
        }

        // For selecting the Routes Checkboxes
        public bool TexasRoute {
            get { 
                return m_TexasRoute.Value.IsChecked;
            }
            set { 
                m_TexasRoute.Value.IsChecked = value;
            }
        }

        public bool AddedRoute {
            get { 
                return m_AddedRoute.Value.IsChecked;
            }
            set { 
                Thread.Sleep(TimeSpan.FromSeconds(0.6)); // step pause
                m_AddedRoute.Value.IsChecked = value;
            }
        }

        public bool PARoute {
            get { 
                return m_PARoute.Value.IsChecked;
            }
            set { 
                m_PARoute.Value.IsChecked = value;
            }
        }

        public bool TestRoute {
            get { 
                return m_TestRoute.Value.IsChecked;
            }
            set { 
                m_TestRoute.Value.IsChecked = value;
            }
        }

        public bool VisitContact {
            get {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                return m_VisitContact.Value.IsChecked;
            }
            set {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                m_VisitContact.Value.IsChecked = value;
            }
        }

        public bool CallContact {
            get {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                return m_CallContact.Value.IsChecked;
            }
            set {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                m_CallContact.Value.IsChecked = value;
            }
        }

        public bool FaxContact {
            get {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                return m_FaxContact.Value.IsChecked;
            }
            set {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                m_FaxContact.Value.IsChecked = value;
            }
        }

        public bool MailContact {
            get {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                return m_MailContact.Value.IsChecked;
            }
            set {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                m_MailContact.Value.IsChecked = value;
            }
        }

        public bool EmailContact {
            get {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                return m_EmailContact.Value.IsChecked;
            }
            set {
                if (!TestIsVisibleToElement(Query.GeneralPopover, Query.MailContact))
                    SwipeDownUntilVisibleToElement(Query.GeneralPopover, Query.GeneralPopover, Query.MailContact, ratio: 0.90);
                m_EmailContact.Value.IsChecked = value;
            }
        }

        public void TapDone() {
            TapAndWait(Query.Done, () => !TestIsVisible(Query.Done));
        }

        // Maybe find more robust way of doing this
        public void DismissSampling() {
            TapAndWait("* index:0", () => !TestIsVisible("* marked:'State License #'"), timeout: TimeSpan.FromSeconds(14));
        }

        // Should be in some general pop over with respect to root since pop overs aren't in the tree of cdlview
        private void AddTextField(string fieldName, string itemName) {
            // Try to find the relative textfield from the field name in the form
            var textFieldQuery = string.Format(Query.GeneralPopover + " descendant view marked:'{0}' parent {1} index:0 descendant {2} index:0", fieldName, Query.EditItem, Query.TextField);
            if (!TestIsVisible(textFieldQuery))
                SwipeDownUntil(Query.GeneralPopover, textFieldQuery, timeout: TimeSpan.FromSeconds(15), ratio: 0.90);
            if (TestIsVisible(textFieldQuery)) {
                // Clear any existing text first
                Thread.Sleep(TimeSpan.FromSeconds(2)); // Step pause
                TapAndWait(textFieldQuery, () => IsKeyboardVisible());
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Wait for keyboard to stop animating
                if (TestIsVisible(Query.ClearText)) {
                    TapAndWait(Query.ClearText, () => !TestIsVisible(Query.ClearText));
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // Post Timeout to Clear Field
                }
                SetField(textFieldQuery, itemName);
                Calabash.Tap(CalabashButton.Enter);
            }
        }
        private void AddDropDown(string fieldName, string headerTitle, string itemName) {
            // Try to find the relative dropdown list from the field name in the form
            // Adding general pop over since apparently for prof title the pop over doesn't block visibility of the other prof title
            var dropDownQuery = string.Format(Query.GeneralPopover + " descendant view marked:'{0}' index:0 parent {1} index:0 descendant {2} index:0", fieldName, Query.EditItem, Query.DropDownList);
            var itemQuery = string.Format("view:'UILabel' marked:'{0}' index:0", itemName);
            var headerQuery = string.Format("view:'UINavigationItemView' marked:'{0}' index:0", headerTitle);
            if (!TestIsVisible(dropDownQuery))
                SwipeDownUntil(Query.GeneralPopover, dropDownQuery, ratio: 0.90);
            if (TestIsVisible(dropDownQuery)) {
                TapAndWait(dropDownQuery, () => AppConvention.TestIsVisible(Container, headerQuery));
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Acts as post timeout
                SwipeDownUntil(Query.PopoverTable, itemQuery);
                // Clear selection before if possible
                if (TestIsVisible(Query.ClearButton)) {
                    TapAndWait(Query.ClearButton, () => TestIsVisible(dropDownQuery));
                    TapAndWait(dropDownQuery, () => TestIsVisible(headerQuery));
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // Acts as post timeout
                    SwipeDownUntil(Query.PopoverTable, itemQuery);
                }
                TapAndWait(itemQuery, () => TestIsVisible(Query.Loaded));
            }
        }

        public void FillInitialInformation() {
            AddTextField("Last Name", "Abbott");
            AddTextField("First Name", "Lisa");
            AddTextField("Second Last Name", "2ndlast");
            AddTextField("Salutation", "Yo");
            AddDropDown("Suffix", "Suffix", "Jr");
            AddDropDown("Prof. Suffix", "Prof. Suffix", "PA");
            AddDropDown("Prof. Title", "Prof. Title", "Osteopath");
            AddDropDown("Specialty", "Specialty", "Obstetrics And Gynecology");
            AddDropDown("Rep. Specialty", "Rep. Specialty", "Allergy");
            AddTextField("Email Address", "me@here.com");
            // Check Sales Data Restriction is No
            TestIsVisible(Query.GeneralPopover + " marked:'Sales Data Restriction' parent * index:0 descendant * marked:'No'");
            AddDropDown("Call Cycle Week Number", "Call Cycle Week Number", "1");
            AddDropDown("Call Cycle Week Day", "Call Cycle Week Day", "Tuesday");
            AddDropDown("Practice Size", "Practice Size", "0 - 10");
            AddTextField("Birth Year", "1962");
            AddDropDown("Birth Month", "Birth Month", "March");
            AddTextField("Birth Day", "12");
            // Check the checkboxes
            VisitContact = true;
            CallContact = true;
            FaxContact = true;
            MailContact = true;
            EmailContact = true;
        }
    }
}