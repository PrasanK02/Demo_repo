using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace Cegedim.Automation {

    public class NewDashboardPage {
        private IApp app;

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

        public NewDashboardPage(IApp application) {
            app = application;
        }

        public void IsLoaded() {
            app.WaitForElement(c => c.Raw(Query.Loaded));
        }

		public NewSearchPage NavigateToSearchPage() {
			Thread.Sleep(TimeSpan.FromSeconds(1)); 
			app.Tap (c => c.Raw (Query.SearchPage));
			var searchPage = new NewSearchPage (app);
			searchPage.IsLoaded ();
			return searchPage;
		}

		public NewTodoPage NavigateToTodoPage() {
			app.TapAndWait(x=>x.Raw(Query.TodoPage),x=>x.Raw(Query.TodoPage));
			return new NewTodoPage(app);
		}
            
    }
}