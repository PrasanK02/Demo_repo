using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;
using Xamarin.Automation;
using Newtonsoft.Json.Linq;
using Xamarin.Automation.Calabash;
using NUnit.Framework;

namespace Cegedim.Automation {

    internal sealed class CheckBox : CegedimSubPage {

        private static class Query {
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
            internal static string CheckBoxOn = CheckBox + " marked:'VAL:True'";
        }

        internal CheckBox(MITouch application, CegedimEntity parent, AppElement element) 
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

    internal sealed class DefaultCheckBox : CegedimSubPage {

        private static class Query {
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+DefaultCheckBox'";
            internal static string CheckBoxOn = CheckBox + " marked:'VAL:True'";
        }

        internal DefaultCheckBox(MITouch application, CegedimEntity parent, AppElement element)
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

    internal sealed class SelectGroup : CegedimSubPage {

        private static class Query {
            internal static string SelectAllButton = "view:'MI.CustomControls.SelectAllButton'";
            internal static string UnselectAll = SelectAllButton + " marked:'Unselect All'";
        }

        internal SelectGroup(MITouch application, CegedimEntity parent, AppElement element) 
            : base(application, parent, element) {
        }

        public override bool IsLoaded {
            get { return true; }
        }

        public bool IsSelected {
            get { return TestIsVisible(Query.UnselectAll); }
            set {
                if (value == IsSelected)
                    return;

                TapAndWait(Query.SelectAllButton, () => IsSelected == value);
            }
        }
    }

    public class SearchPage : CegedimPage {


        private static class Query {
            internal static string Loaded = "* marked:'PAGE:targeting/customersearch.cdl'";
            internal static string CustomerSearchFieldBackup = "UITextFieldLabel marked:'Search customer'";
            internal static string CustomerSearchField = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_lastname'";
            //internal static string CustomerSearchFieldBackUp = "UIFieldEditor"; // Appears to be different search field queries
            internal static string ClearSearchButton = CustomerSearchField + " descendant button";
            internal static string Keyboard = "view:'UIKeyboardAutomatic'";
            internal static string Departments = "* id:'TYPE:DEPT'";
            internal static string Pharmacies = "* id:'TYPE:PHAR'";
            internal static string Professionals = "* id:'TYPE:PRES'";
            internal static string Individuals = "* id:'TYPE:INDV'";
            internal static string Organizations = "* id:'TYPE:ORG'";
            internal static string FilterButton = "* {accessibilityIdentifier ENDSWITH 'filter.png'} index:0";
            internal static string FilterPopover = "view:'CdlView' id:'PAGE:targeting/filterpopover.cdl'";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string FilterReset = "view:'UIButton' marked:'Reset'";
            internal static string FilterDone = "view:'UIButton' marked:'Done'";
            internal static string FilterCountry = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_country'";
            internal static string FilterState = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_state'";
            internal static string FilterCity = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_city'";
            internal static string FilterList = "view:'_UIPopoverView' descendant view:'UITableView' marked:'Empty list' index:0";
            internal static string FilterPostalCode = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_postcode'";
            internal static string FilterAffiliation = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_affiliation_type'";
            internal static string FilterMainInstitution = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_main_customer_name'";
            internal static string FilterBrick = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_brick'";
            internal static string FilterSpecialty = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_specialty'";
            internal static string FilterStatus = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_status'";
            internal static string ClearButton = "view:'UINavigationButton' marked:'Clear'";
            internal static string CustomerRow = "view:'UITableViewCell'";
            internal static string CustomerCountLabel = "* marked:'DATA:vt_preset_list/vc_total' descendant textFieldLabel index:0";
            internal static string FirstContact = "view:'UITableViewCell' descendant textFieldLabel index:0";
            internal static string AutoCorrect = "view:'UIAutocorrectShadowView'";
            internal static string DashboardButton = "view:'UITabBarButton' label {text beginswith 'Home'}";
            internal static string TableViewCells = "UITableViewCell"; // in case this gets changed
            internal static string CustomerCallIcon = "tableViewCell descendant imageView {accessibilityIdentifier ENDSWITH 'call.png'}";
            internal static string MoreActions = "view:'Dendrite.IPhone.Forms.GridControl.GridImage' {accessibilityIdentifier contains 'moreactions'}";
        }

        private Lazy<CheckBox> m_departmentsCheckBox;
        private Lazy<CheckBox> m_pharmaciesCheckBox;
        private Lazy<CheckBox> m_professionalsCheckBox;
        private Lazy<SelectGroup> m_individualSelectGroup;
        private Lazy<SelectGroup> m_organizationSelectGroup;

        internal SearchPage(MITouch application, AppContainer container)
            : base(application, container) {
            m_departmentsCheckBox = new Lazy<CheckBox>(() => CheckBoxFactory(Query.Departments));
            m_pharmaciesCheckBox = new Lazy<CheckBox>(() => CheckBoxFactory(Query.Pharmacies));
            m_professionalsCheckBox = new Lazy<CheckBox>(() => CheckBoxFactory(Query.Professionals));
            m_individualSelectGroup = new Lazy<SelectGroup>(() => SelectGroupFactory(Query.Individuals));
            m_organizationSelectGroup = new Lazy<SelectGroup>(() => SelectGroupFactory(Query.Organizations));
        }

        private CheckBox CheckBoxFactory(string query) {
            return new CheckBox(Application, this, Container.GetDescendant(query));
        }

        private SelectGroup SelectGroupFactory(string query) {
            return new SelectGroup(Application, this, Container.GetDescendant(query));
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }
            
        public string CustomerSearchField {
            get { return GetField(Query.CustomerSearchField); }
            set { 
                Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                SetField(Query.CustomerSearchField, value); 
            }
        }

        public void ClearSearchField() {
            // Open keyboard if its not there
            // TODO: Discover a compact and faster way of doing this. The new order tests seem to require a different search
            // field query.
            if (!IsKeyboardVisible()) {
                try {
                    Wait(() => TestIsVisible(Query.CustomerSearchField));
                    TapAndWait(Query.CustomerSearchField, () => IsKeyboardVisible(), timeout: TimeSpan.FromSeconds(12),
                        postTimeout: TimeSpan.FromSeconds(0.5));
                } catch {
                    Wait(() => TestIsVisible(Query.CustomerSearchFieldBackup));
                    TapAndWait(Query.CustomerSearchFieldBackup, () => IsKeyboardVisible(), timeout: TimeSpan.FromSeconds(12),
                        postTimeout: TimeSpan.FromSeconds(0.5));
                }
            }

            if(TestIsVisible(Query.ClearSearchButton)) {
                TapAndWait(Query.ClearSearchButton, () => !TestIsVisible(Query.ClearSearchButton));
            }
        }
            
        public void SearchFor(string searchText) {
            ClearSearchField();
            CustomerSearchField = searchText;
            Thread.Sleep(TimeSpan.FromSeconds(0.2)); // step pause
            Calabash.Tap(CalabashButton.Enter);
        }
            
        public void ConfirmResults() {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Wait(() => TestIsVisible(Query.FirstContact), postTimeout: TimeSpan.FromSeconds(0.3));
            var firstContact = Container.Descendants(Query.FirstContact).First().Label;
            var currentCustomerCount = Container.Descendants(Query.CustomerCountLabel).First().Label;
            var results = (string)Calabash.Invoke("getBusinessObjectData:", "vt_query");
            JArray customerArray = JArray.Parse(results);
            var customerCount = customerArray.Count().ToString();
            var customers = customerArray.Select(
                x => new Customer { 
                    CustomerId = x["customer_id"].ToString(), CustomerType = x["customer_type"].ToString(), DisplayName = x["vc_display_name"].ToString()
                }
            );
            // Check if the length matches the desired length
            if (currentCustomerCount != customerCount) {
                Assert.Fail("Count doesn't match the desired count");
            }
            // Check if the first contact matches the json results
            bool userFound = false;
            foreach(var customer in customers) {
                if (firstContact == customer.DisplayName) {
                    userFound = true;
                    break;
                }
            }
            if(!userFound) {
                var errorString = String.Format("Expected customer: {0} Observed customer: {1}", firstContact, customers.First().DisplayName);
                Assert.Fail(errorString);
            }
        }

        public bool FilterByDepartments {
            get { 
                HideKeyboard();
                return m_departmentsCheckBox.Value.IsChecked; 
            }
            set { 
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                m_departmentsCheckBox.Value.IsChecked = value; 
            }
        }

        public bool FilterByPharmacies {
            get { 
                HideKeyboard();
                return m_pharmaciesCheckBox.Value.IsChecked; 
            }
            set { 
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                m_pharmaciesCheckBox.Value.IsChecked = value; 
            }
        }   

        public bool FilterByProfessionals {
            get { 
                HideKeyboard();
                return m_professionalsCheckBox.Value.IsChecked; 
            }
            set { 
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                m_professionalsCheckBox.Value.IsChecked = value; 
            }
        }   
            
        public bool FilterByIndividuals {
            get { 
                HideKeyboard();
                return m_individualSelectGroup.Value.IsSelected; 
            }
            set { 
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                m_individualSelectGroup.Value.IsSelected = value; 
            }
        }   

        public bool FilterByOrganizations {
            get { 
                HideKeyboard();
                return m_organizationSelectGroup.Value.IsSelected; 
            }
            set { 
                HideKeyboard();
                Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
                m_organizationSelectGroup.Value.IsSelected = value; 
            }
        }   

        public void OpenFilter(bool reset) {
            if(!TestIsVisible(Query.FilterPopover)) {
                TapAndWait(Query.FilterButton, () => TestIsVisible(Query.FilterPopover));
                if (reset)
                    TapAndWait(Query.FilterReset, () => TestIsVisible(Query.FilterPopover));
            }
            Thread.Sleep(TimeSpan.FromSeconds(1.5)); // Step Pause
        }

        public void FilterByCountryState(bool reset = false) {
            var results = Calabash.SelectCountriesAndStates();
            JArray countryStateArray = JArray.Parse(results);
            string country = countryStateArray.Take(1).Select(c => c["country"].ToString()).First();
            string state = countryStateArray.Take(1).Select(c => c["state"].ToString()).First();
            OpenFilter(reset);
            TapAndWait(Query.FilterCountry, () => TestIsVisible(Query.ClearButton));
            // Clear any existing selection
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            TapAndWait(Query.ClearButton, () => TestIsVisible(Query.FilterCountry));
            TapAndWait(Query.FilterCountry, () => TestIsVisible(Query.ClearButton));

            // Not sure why I need this sleep here but it breaks locally otherwise
            Thread.Sleep(TimeSpan.FromSeconds(2));
            var countryQuery = string.Format("* marked:'{0}' parent view:'UITableViewCell' index:0", country);
            var tableBottomBar = Query.Popover + " descendant view:'UIToolBar'";
            // TODO: Speed up this swipe by decreasing the polling time or increasing the 
            // swipe speed
             SwipeDownUntil(Query.FilterList, countryQuery, timeout: TimeSpan.FromSeconds(250));
            // Sometimes the tableviewcell is covered by the bottom bar but visible to calabash
            if (TestIsVisibleToElement(tableBottomBar, countryQuery)) {
                var filterRectangle = Calabash.Query(Query.FilterList).First().Rectangle;
                var centerX = filterRectangle.X + filterRectangle.Width / 2.0;
                var initialY = filterRectangle.Y + filterRectangle.Height * 0.98;
                var finalY = filterRectangle.Y + filterRectangle.Height * 0.10; 
                Calabash.Pan(new Point((int)centerX, (int)initialY), new Point((int)centerX, (int)finalY));
            }
            TapAndWait(countryQuery, () => TestIsVisible(Query.FilterPopover));
            var stateQuery = string.Format("* marked:'{0}'", state);
            TapAndWait(Query.FilterState, () => TestIsVisible(stateQuery));
            TapAndWait(stateQuery, () => TestIsVisible(Query.FilterPopover));
            TapAndWait(Query.FilterDone, () => TestIsVisible(Query.CustomerSearchField));
        }

        public string City {
            get { return GetField(Query.FilterCity); }
            set { SetField(Query.FilterCity, value); }
        }

        public string PostalCode {
            get { return GetField(Query.FilterPostalCode); }
            set { SetField(Query.FilterPostalCode, value); }
        }

        public string MainInstitution {
            get { return GetField(Query.FilterMainInstitution); }
            set { SetField(Query.FilterMainInstitution, value); }
        }

        public void FilterByCity(bool reset = false) {
            var results = Calabash.SelectCities();
            JArray cityArray = JArray.Parse(results);
            string city = cityArray.Take(1).Select(c => c["city"].ToString()).First();
            OpenFilter(reset);
            // Reset button is showing up in the same spot as the city button
            SwipeDownUntil(Query.FilterPopover, Query.FilterPostalCode, postTimeout: TimeSpan.FromSeconds(3));
            TapAndWait(Query.FilterCity, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(1.5)); 
            City = city;
            Thread.Sleep(TimeSpan.FromSeconds(0.4)); // step pause
            // In case the auto correct thing tries to change the city name
            if (TestIsVisible(Query.AutoCorrect)) {
                TapAndWait(Query.AutoCorrect, () => !TestIsVisible(Query.AutoCorrect));
            }
            TapAndWait(Query.FilterDone, () => TestIsVisible(Query.CustomerSearchField));
        }

        public void FilterByPostalCode(bool reset = false) {
            var results = Calabash.SelectPostalCode();
            JArray postalArray = JArray.Parse(results);
            string postalCode = postalArray.Take(1).Select(c => c["postal_area"].ToString()).First();
            OpenFilter(reset);
            SwipeDownUntil(Query.FilterPopover, Query.FilterPostalCode);
            TapAndWait(Query.FilterPostalCode, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(1));
            PostalCode = postalCode;
            TapAndWait(Query.FilterDone, () => TestIsVisible(Query.CustomerSearchField));
        }

        public void FilterByMainInstitution(string mainInstitution, bool reset = false) {
            OpenFilter(reset);
            SwipeDownUntil(Query.FilterPopover, Query.FilterMainInstitution);
            TapAndWait(Query.FilterMainInstitution, () => IsKeyboardVisible());
            MainInstitution = mainInstitution;
            TapAndWait(Query.FilterDone, () => TestIsVisible(Query.CustomerSearchField));
        }

        public void FilterByAffiliationType(string affiliationType, bool reset = false) {
            OpenFilter(reset);
            TapAndWait(Query.FilterAffiliation, () => TestIsVisible(Query.ClearButton));
            // Clear any existing selection
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            TapAndWait(Query.ClearButton, () => TestIsVisible(Query.FilterAffiliation));
            TapAndWait(Query.FilterAffiliation, () => TestIsVisible(Query.ClearButton));
            // Not sure why I need this sleep here but it breaks locally otherwise
            Thread.Sleep(TimeSpan.FromSeconds(2));
            var affiliationQuery = string.Format("* marked:'{0}'", affiliationType);
            SwipeDownUntil(Query.FilterList, affiliationQuery);
            TapAndWait(affiliationQuery, () => TestIsVisible(Query.FilterPopover));
            TapAndWait(Query.FilterDone, () => TestIsVisible(Query.CustomerSearchField));
        }

        public SummaryPage TapCustomerRow(int index) {
            var customerRowQuery = Query.CustomerRow + " index:" + index.ToString();
            TestIsVisible(customerRowQuery);
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            return AppConvention.TapActivateAndWait<SummaryPage>(
                Application, customerRowQuery);
        }

        public CallPage NavigateToCallPage() {
            string callCustomersResults = Calabash.SelectCallCustomers(2);
            JArray callCustomers = JArray.Parse(callCustomersResults);
            CallPage.CallCustomers = callCustomers;
            string teamMatesResults = Calabash.SelectTeamMate(1);
            JArray teamMates = JArray.Parse(teamMatesResults);
            Calabash.TurnOnUseIndividualAsASpeaker();
            string employeeIdResults = Calabash.SearchForEmployeeId();
            JArray employeeId = JArray.Parse(employeeIdResults);
            Calabash.TurnOffCheckForNegativeInventory();
            Calabash.TurnOnOwnerEmployeeDropDown();
            string sampleProductResults = Calabash.SelectSampleProductWithLotNumber(1);
            JArray sampleProduct = JArray.Parse(sampleProductResults);
            string customerName = callCustomers[0]["Name"].ToString();
            SearchFor(customerName);
            AppConvention.Wait(Application, () => TestIsVisible(String.Format("* marked:'{0}' index:0", customerName)));
            var summaryPage = TapCustomerRow(0);
            return summaryPage.NavigateToCallPage();
        }

        public DashboardPage NavigateToDashboardPage() {
            return AppConvention.TapActivateAndWait<DashboardPage>(
                Application, Query.DashboardButton);
        }

        public string[] SampleOrderEntryCustomers(string rawOrderEntryCustomers) {
            string[] customerInfo = new string[2];
            int customerCount = JArray.Parse(rawOrderEntryCustomers).Count;
            if (customerCount == 0)
                Assert.Fail("No order entry customers to sample from");
            Random random = new Random();
            int randVal = random.Next(0, customerCount - 1);
            string sampledCustomerName = JArray.Parse(rawOrderEntryCustomers)[randVal]["name"].ToString();
            string sampledCustomerType = JArray.Parse(rawOrderEntryCustomers)[randVal]["customer_type"].ToString();
            customerInfo[0] = sampledCustomerName;
            customerInfo[1] = sampledCustomerType;
            return customerInfo;
        }

        public void AssertCustomerCount(int expectedCount) {
            int observedCount = Convert.ToInt32(Calabash.Query(Query.CustomerCountLabel).First().Label);
            // TODO: Use either debug assert or assert fail - figure out which works on test cloud
            if (observedCount != expectedCount)
                Assert.Fail("Expected and observed customer counts do not match");
                //Debug.Assert(false, "Expected and Observed customer counts do not match");
        }

        public void AssertCustomer(string customerName) {
            TestIsVisible(string.Format("UITableViewCell {{text contains '{0}'}}", customerName));
        }

        public void AssertContactButtons() {
            int customerCount = Calabash.Query(Query.TableViewCells).Count();
            int callIconCount = Calabash.Query(Query.CustomerCallIcon).Count();
            if (customerCount != callIconCount)
                Assert.Fail("The customer count and call icon count do not match");
        }

        public MoreActionsPopover MoreActions(string customerName) {
            string moreActionsQuery = string.Format("view {{text contains '{0}'}} parent {1} descendant {2} index:0",
                customerName, Query.TableViewCells, Query.MoreActions);
            return AppConvention.TapActivateAndWait<MoreActionsPopover>(
                Application, moreActionsQuery);
        }

        public void SetDefaultCustomerType(string defaultCustomerType) {
            if (String.IsNullOrEmpty(defaultCustomerType)) {
                Calabash.SetDefaultCustomerType(defaultCustomerType);
            }
        }

        public void ResetServiceRuleStore() {
            var results = Calabash.ResetServiceRuleStore();
            if (results != "\"Reset Service RuleStore\"")
                Assert.Fail("Reset service doesn't match the expected results");
        }

        public void SetFilter(string defaultCustomerType) {
            FilterByIndividuals = false;
            FilterByOrganizations = false;
            if (defaultCustomerType == "PHAR")
                FilterByPharmacies = true;
            else if (defaultCustomerType == "PRES")
                FilterByProfessionals = true;
        }
    }

    public class MoreActionsPopover : Popover {
        public string newOrder;
        public string okButton;
        private static class Query {
            internal static string Loaded = "view:'_UIPopoverView'";
            internal static string Presentations = "UIAlertButton marked:'Presentations'";
            internal static string NewAppointment = "UIAlertButton marked:'New Appointment'";
            internal static string NearThisCustomer = "UIAlertButton marked:'Near This Customer'";
            internal static string NewOrder = "UIAlertButton marked:'New Order'";
            internal static string NewOrderiOS8 = "Dendrite.IPhone.Forms.ShinyButton marked:'New Order'";
            internal static string NoAccountNotification = "view marked:'There is no account defined for this customer.'";
            internal static string OKButton = "view:'_UIModalItemTableViewCell' marked:'OK'";
            internal static string OKButtoniOS8 = "view:'_UIAlertControllerActionView' marked:'OK'";
        }

        internal MoreActionsPopover(MITouch application, AppContainer container)
            : base(application, container) {
            // Change certain queries depending on iOS 8 or iOS 7
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                if (Environment.GetEnvironmentVariable("XTC_DEVICE_OS").Contains("7.")) {
                    newOrder = Query.NewOrder;
                    okButton = Query.OKButton;
                }
                else {
                    newOrder = Query.NewOrderiOS8;
                    okButton = Query.OKButtoniOS8;
                }
            } else {
                // testing locally on iOS8 otherwise you may need to change this
//                newOrder = Query.NewOrderiOS8;
//                okButton = Query.OKButtoniOS8;
                newOrder = Query.NewOrder;
                okButton = Query.OKButton;
            }
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public NewOrderPopover CreateNewOrder() {
            return AppConvention.TapActivateAndWait<NewOrderPopover>(
                Application, newOrder);
        }

        public void CreateInactiveNewOrder() {
            Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
            TapAndWait(newOrder, () => TestIsVisible(Query.NoAccountNotification), timeout: TimeSpan.FromSeconds(15), 
                postTimeout: TimeSpan.FromSeconds(1));
        }

        public SearchPage OK() {
            return AppConvention.TapActivateAndWait<SearchPage>(
                Application, okButton);
        }
    }

    public class NewOrderPopover : DropShadowPopover {
        private string m_orderChannel;
        private string m_account;
        private string m_orderPriceList;
        private string m_exchangeAndReturn;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'New Order'";
            internal static string Continue = "view:'Dendrite.IPhone.Forms.ShinyButton' marked:'Continue'";
        }

        internal NewOrderPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { 
                Thread.Sleep(TimeSpan.FromSeconds(2.5)); // step pause
                return TestIsVisible(Query.Loaded); 
            }
        }

        public string OrderChannel {
            get {
                return m_orderChannel;
            }
            set {
                SelectDropDown("Order Channel", value, clearExisting: true, postTimeout: TimeSpan.FromSeconds(0.5));
                m_orderChannel = value;
            }
        }

        public string Account {
            get {
                return m_account;
            }
            set {
                // This actually is available for some orders - programmatic way to set it if available;
                // Checks if the drop down icon is there
                if (TestIsVisible("view marked:'Account' parent view:'Dendrite.IPhone.Forms.CdlTableViewCell' descendant UIButton")) {
                    SelectDropDown("Account", value, postTimeout: TimeSpan.FromSeconds(0.5));
                    m_account = value;
                }
            }
        }

        public string OrderPriceList {
            get {
                return m_orderPriceList;
            }
            set {
                // Not always an available option
                if (TestIsVisible("view marked:'Order Pricelist'")) {
                    SelectDropDown("Order Pricelist", value, postTimeout: TimeSpan.FromSeconds(0.5));
                    m_orderPriceList = value;
                }
            }
        }

        public string ExchangeAndReturn {
            get {
                return m_exchangeAndReturn;
            }
            set {
                // Not always an available option
                if (TestIsVisible("view marked:'Exchange and return'")) {
                    SelectDropDown("Exchange and return", value, clearExisting: true, postTimeout: TimeSpan.FromSeconds(0.5));
                    m_exchangeAndReturn = value;
                }
            }
        }

        public EditOrderEntryPage Continue() {
            return AppConvention.TapActivateAndWait<EditOrderEntryPage>(
                Application, Query.Continue);
        }
    }
}