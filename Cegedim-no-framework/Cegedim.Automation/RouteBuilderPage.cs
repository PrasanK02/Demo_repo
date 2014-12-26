using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using NUnit.Framework;
using Xamarin.Automation.Calabash;

namespace Cegedim.Automation {

    public class RouteBuilderPage : CegedimPage {

        private static class Query {
            internal static string Loaded = "view id:'PAGE:routing/routelist.cdl'";
            internal static string GeneralRoute = "view:'Dendrite.IPhone.Forms.CdlMultiRowTextBoxRO' descendant view";
            internal static string GridQuery = "view marked:'detailContent' descendant view:'Dendrite.IPhone.Forms.CdlGrid' index:0";
            internal static string Edit = "view marked:'wf_edit'";
            internal static string RouteContent = "view marked:'PAGE:routing/routecentraldialog.cdl' view marked:'dialogContent'";
            internal static string DeleteRoute = "view marked:'Delete Route'";
            internal static string Delete = "view:'_UIModalItemTableViewCell' marked:'Delete'";
            internal static string CreateNewRoute = "view marked:'+'";
            internal static string EditRoute = "view marked:'Edit'";
            internal static string GeneralCustomerRow = "view id:'customers' child view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell'";
        }

        internal RouteBuilderPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public string SelectRouteCustomers(int numberOfCustomers) {
            return Calabash.SelectRouteCustomers(numberOfCustomers);
        }

        public string SelectRouteCustomers(string routeName) {
            return Calabash.SelectRouteCustomers(routeName);
        }

        public string SelectRouteFromDatabase(string routeName) {
            return Calabash.SelectRoute(routeName);
        }

        public string RouteQueryNamed(string routeName) {
            return string.Format("{0} marked:'{1}'", Query.GeneralRoute, routeName);
        }

        public void DeleteAllRoutesNamed(string routeName) {
            JArray routes = JArray.Parse(SelectRouteFromDatabase(routeName));
            int routesCount = routes.Count;
            for (int i = 0; i < routesCount; i ++) {
                Wait(() => TestIsVisible(RouteQueryNamed(routeName)));
                TapAndWait(RouteQueryNamed(routeName), () => TestIsVisible(Query.GridQuery));
                Wait(() => TestIsVisible(Query.Edit));
                TapAndWait(Query.Edit, () => TestIsVisible(Query.RouteContent));
                Wait(() => TestIsVisible(Query.DeleteRoute));
                TapAndWait(Query.DeleteRoute, () => TestIsVisible(Query.Delete), postTimeout: TimeSpan.FromSeconds(0.4));
                TapAndWait(Query.Delete, () => !TestIsVisible(Query.Delete), timeout: TimeSpan.FromSeconds(15), postTimeout: TimeSpan.FromSeconds(0.2));
            }
        }

        public NewRoutePopover CreateNewRoutePopover() {
            return AppConvention.TapActivateAndWait<NewRoutePopover>(Application, Query.CreateNewRoute);
        }

         public void ConfirmRouteWithDatabase(string routeName, string routingCustomers, int numberOfCustomersAdded) {
            var routeCustomers = JArray.Parse(SelectRouteCustomers(routeName)).Take(numberOfCustomersAdded).Select(x => x["full_name"].ToString());
            var routeCustomersFromDatabase = JArray.Parse(routingCustomers).Take(numberOfCustomersAdded).Select(x => x["full_name"].ToString());
            if (routeCustomersFromDatabase.Except(routeCustomers).Count() != 0 || routeCustomers.Except(routeCustomersFromDatabase).Count() != 0)
                Assert.Fail("Not all the correct customers were added to route");
        }

        public void SelectRoute(string routeName) {
            string routeQuery = string.Format("{0} marked:'{1}'", Query.GeneralRoute, routeName);
            TapAndWait(routeQuery, () => TestIsVisible(Query.EditRoute));
        }

        public void ConfirmRouteDoesNotExist(string routeName) {
            Thread.Sleep(TimeSpan.FromSeconds(0.3));
            string routeQuery = string.Format("view marked:'{0}'", routeName);
            if (TestIsVisible(routeQuery))
                Assert.Fail("Deleted route still exists");
        }

        public void ConfirmRouteDoesNotExistInDatabase(string routeName) {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            JArray selectedRoute = JArray.Parse(Calabash.SelectRoute(routeName));
            if (selectedRoute.Count != 0)
                Assert.Fail("Deleted route still exists in the database");
        }

        public EditRoutePopover CreateEditRoutePopover() {
            return AppConvention.TapActivateAndWait<EditRoutePopover>(Application, Query.EditRoute);
        }

        public SummaryPage NavigateToCustomerSummaryPage(int rowIndex) {
            string rowQuery = string.Format("{0} index:{1}", Query.GeneralCustomerRow, rowIndex);
            return AppConvention.TapActivateAndWait<SummaryPage>(Application, rowQuery);
        }

        public void ConfirmIsLoaded() {
            Wait(() => IsLoaded);
        }
    }

    public class EditRoutePopover : DropShadowPopover {
        private string m_name;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'Edit Route'";
            internal static string AddCustomer = "view marked:'KEY:ADD'";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string SearchField = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_lastname' descendant view:'Dendrite.IPhone.Forms.CdlTextBox' index:0";
            //internal static string SearchField = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_lastname'";
            internal static string SearchFieldBackUp = "view marked:'Search customer'";
            internal static string ClearText = "view:'UITextField' isFirstResponder:1 child view:'UIButton'";
            internal static string Done = "UINavigationButton marked:'Done'";
            internal static string CheckedBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox' id:'VAL:True'";
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
            internal static string Color = "view {accessibilityIdentifier contains 'COLOR'}";
            internal static string ColorDropDown = "view:'MI.CustomControls.RoutingColorPickerControlMobile' id:'DATA:route/color'";
            internal static string DeleteRoute = "view marked:'Delete Route'";
            internal static string Delete = "view:'_UIModalItemTableViewCell' marked:'Delete'";
        }

        internal EditRoutePopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }    

        public string Name {
            get { return m_name; }
            set { 
                AddText("Name", value, tapActionKey: true);
                Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                m_name = value;
            }
        }

        public void SelectColor(int index = 0) {
            string colorQuery = Query.Color + " index:" + index.ToString();
            TapAndWait(Query.ColorDropDown, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(colorQuery, () => !TestIsVisible(Query.Popover));
        }

        public void AddCustomersByIndex(string rawCustomers, int[] indices) {
            Wait(() => TestIsVisible(Query.AddCustomer), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(Query.AddCustomer, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.6));
            var customersFullNameArray = JArray.Parse(rawCustomers).Select(x => x["full_name"]).ToList();
            foreach(var index in indices) {
                string fullName = customersFullNameArray[index].ToString();
                string fullNameCheckBoxQuery = string.Format("view marked:'{0}' parent UITableViewCell descendant {1}", fullName, Query.CheckBox);
                char[] parseChar = {','};
                string searchName = fullName.Split(parseChar)[0];
                MITouch.iosApp.Repl();
                try {
                    Wait(() => TestIsVisible(Query.SearchField), postTimeout: TimeSpan.FromSeconds(0.7));
                    // Custom tap since the middle of text field is unresponsive
                    // TODO: We should get a definitive ID here to touch from the Devs
                    var searchRect = MITouch.iosApp.Query(c => c.Raw(Query.SearchField)).First().Rect;
                    var x = searchRect.CenterX - searchRect.Width / 4.0;
                    var y = searchRect.Y + searchRect.Height / 1.2; // Center Y doesn't even seem to work locally
                    MITouch.iosApp.TapCoordinates((float)x, (float)y);
                    Wait(() => IsKeyboardVisible());
                    //TapAndWait(Query.SearchField, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                } catch {
                    Wait(() => TestIsVisible(Query.SearchFieldBackUp), postTimeout: TimeSpan.FromSeconds(0.7));
                    TapAndWait(Query.SearchFieldBackUp, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                }
                if (TestIsVisible(Query.ClearText))
                    TapAndWait(Query.ClearText, () => !TestIsVisible(Query.ClearText));
                SetField(Query.SearchField, searchName);
                Calabash.Tap(CalabashButton.Enter);
                Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.4));
                Wait(() => TestIsVisible(fullNameCheckBoxQuery));
                string checkMarked = string.Format("{0} descendant {1}", fullNameCheckBoxQuery, Query.CheckedBox);
                TapAndWait(fullNameCheckBoxQuery, () => TestIsVisible(checkMarked));
            }
            TapAndWait(Query.Done, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        public new void Done() {
            base.Done();
            Wait(() => TestIsVisible(string.Format("view marked:'{0}' index:0", m_name)));
        }

        public RouteBuilderPage DeleteRoute() {
            Wait(() => TestIsVisible(Query.DeleteRoute), postTimeout: TimeSpan.FromSeconds(0.7));
            TapAndWait(Query.DeleteRoute, () => TestIsVisible(Query.Delete), postTimeout: TimeSpan.FromSeconds(0.3));
            return AppConvention.TapActivateAndWait<RouteBuilderPage>(Application, Query.Delete);
        }
    }

    public class NewRoutePopover : DropShadowPopover {
        private string m_name;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'New Route'";
            internal static string AddCustomer = "view marked:'KEY:ADD'";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string SearchField = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'filter_lastname'";
            internal static string ClearText = "view:'UITextField' isFirstResponder:1 child view:'UIButton'";
            internal static string Done = "UINavigationButton marked:'Done'";
            internal static string CheckedBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox' id:'VAL:True'";
            internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
            internal static string Color = "view {accessibilityIdentifier contains 'COLOR'}";
            internal static string ColorDropDown = "view:'MI.CustomControls.RoutingColorPickerControlMobile' id:'DATA:route/color'";
        }

        internal NewRoutePopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public string Name {
            get { return m_name; }
            set { 
                AddText("Name", value, tapActionKey: true);
                Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                m_name = value;
            }
        }

        public void SelectColor(int index = 0) {
            string colorQuery = Query.Color + " index:" + index.ToString();
            TapAndWait(Query.ColorDropDown, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(colorQuery, () => !TestIsVisible(Query.Popover));
        }

        public void AddCustomers(int numberOfCustomers, string rawCustomers) {
            Wait(() => TestIsVisible(Query.AddCustomer), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(Query.AddCustomer, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.6));
            var customersArray = JArray.Parse(rawCustomers).Take(numberOfCustomers);
            foreach(var customer in customersArray) {
                string fullName = customer["full_name"].ToString();
                string fullNameCheckBoxQuery = string.Format("view marked:'{0}' parent UITableViewCell descendant {1}", fullName, Query.CheckBox);
                char[] parseChar = {','};
                string searchName = fullName.Split(parseChar)[0];
                TapAndWait(Query.SearchField, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                if (TestIsVisible(Query.ClearText))
                    TapAndWait(Query.ClearText, () => !TestIsVisible(Query.ClearText));
                SetField(Query.SearchField, searchName);
                Calabash.Tap(CalabashButton.Enter);
                Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.4));
                Wait(() => TestIsVisible(fullNameCheckBoxQuery));
                string checkMarked = string.Format("{0} descendant {1}", fullNameCheckBoxQuery, Query.CheckedBox);
                TapAndWait(fullNameCheckBoxQuery, () => TestIsVisible(checkMarked));
            }
            TapAndWait(Query.Done, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        public void ConfirmRouteExists(string routeName) {
            if (!TestIsVisible(string.Format("view marked:'{0}' index:0"), routeName))
                Assert.Fail("Route is not visible");
        }

        public new void Done() {
            base.Done();
            Wait(() => TestIsVisible(string.Format("view marked:'{0}' index:0", m_name)));
        }
    }
}