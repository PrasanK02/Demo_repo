using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using NUnit.Framework;
//using Xamarin.Automation.Extensions;

namespace Cegedim.Automation {
     
    public class DashboardPage : CegedimPage {

		private static class Query {
			internal static string Loaded = "view id:'navBar'";
			internal static string PlannerPage = "view id:'homeLink_planner1'";
            internal static string SearchPage = "view marked:'global_search'";
            internal static string IncompleteCallsPage = "UIImageView {accessibilityIdentifier ENDSWITH 'incompletecalls.png'}";
            internal static string ExpressCall = "view marked:'homeLink_quickCalls'";
            internal static string IncompleteOrdersCount = "view:'Dendrite.IPhone.Forms.CdlHomeNav+HomeButton' marked:'homeLink_orderentry' label {text like '*'} index:1";
            internal static string IncompleteOrders = "view marked:'homeLink_orderentry'";
            internal static string PresentationsPage = "view marked:'homeLink_advpresentations'";
            internal static string RouteBuilderPage = "view marked:'homeLink_routeBuilder'";
            internal static string TodoPage = "view marked:'homeLink_todo'";
            internal static string NotificationsPage = "view marked:'homeLink_notifications'";
            internal static string NotificationsCount = "view:'Dendrite.IPhone.Forms.CdlHomeNav+HomeButton' marked:'homeLink_notifications' label {text like '*'} index:1";
		}

        internal DashboardPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

		public override bool IsLoaded {
			get { return TestIsVisible(Query.Loaded); }
		}

        public PlannerPage NavigateToPlannerPage() {
			return AppConvention.TapActivateAndWait<PlannerPage>(
				Application, Query.PlannerPage);
        }

        public PresentationsPage NavigateToPresentationsPage() {
            return AppConvention.TapActivateAndWait<PresentationsPage>(
                Application, Query.PresentationsPage);
        }

        public SearchPage NavigateToSearchPage() {
            Thread.Sleep(TimeSpan.FromSeconds(1)); 
            return AppConvention.TapActivateAndWait<SearchPage>(
                Application, Query.SearchPage);
        }

        public IncompleteCallsPage NavigateToIncompleteCallsPage() {
            return AppConvention.TapActivateAndWait<IncompleteCallsPage>(
                Application, Query.IncompleteCallsPage);
        }

        public TodoPage NavigateToTodoPage() {
            return AppConvention.TapActivateAndWait<TodoPage>(
                Application, Query.TodoPage);
        }

        public RouteBuilderPage NavigateToRouteBuilderPage() {
            return AppConvention.TapActivateAndWait<RouteBuilderPage>(
                Application, Query.RouteBuilderPage);
        }

        public ExpressCallPage NavigateToExpressCallPage() {
            return AppConvention.TapActivateAndWait<ExpressCallPage>(
                Application, Query.ExpressCall);
        }

        public NotificationsPage NavigateToNotificationsPage() {
            return AppConvention.TapActivateAndWait<NotificationsPage>(
                Application, Query.NotificationsPage);
        }

        public IncompleteOrdersPage NavigateToIncompleteOrdersPage() {
            if (CountIncompleteOrders() == 0)
                Assert.Fail("No incomplete orders");
            return AppConvention.TapActivateAndWait<IncompleteOrdersPage>(
                Application, Query.IncompleteOrders);
        }

        // Just a place holder until I can develop the full one
        public void NavigateToExpressCallPagePlaceHolder() {
        }

        public int CountIncompleteOrders() {
            var iconResults = Calabash.Query(Query.IncompleteOrdersCount);
            if (iconResults.Count() == 0)
                return 0;
            return Convert.ToInt32(iconResults.First().Label);
        }

        public int CountNotifications() {
            var iconResults = Calabash.Query(Query.NotificationsCount);
            if (iconResults.Count() == 0)
                return 0;
            return Convert.ToInt32(iconResults.First().Label);
        }

        public string GetOrderEntryCustomers(int limit) {
            return Calabash.SelectOrderEntryCustomers(limit);
        }

        public string GetInactiveOrderEntryCustomers(int limit) {
            return Calabash.SelectInactiveOrderEntryCustomers(limit);
        }

        public string GetDefaultCustomerType() {
            return JArray.Parse(Calabash.GetDefaultCustomerType()).First["parameter_value"].ToString();
        }

        public void SetDefaultCustomerType(string defaultCustomerType) {
            if (!string.IsNullOrEmpty(defaultCustomerType)) {
                Calabash.SetDefaultCustomerType(defaultCustomerType);
            }
        }

        public void ResetServiceRuleStore() {
            var results = Calabash.ResetServiceRuleStore();
            if (results != "\"Reset Service RuleStore\"")
                Assert.Fail("Reset service doesn't match the expected results");
        }

        public void UpdateDatabaseSubmitOrderWithOutSignature() {
            Calabash.UpdateDatabaseSubmitOrderWithOutSignature();
        }

        public void UpdateDatabaseSubmitOrderWithSignature() {
            Calabash.UpdateDatabaseSubmitOrderWithSignature();
        }   
    }
}

