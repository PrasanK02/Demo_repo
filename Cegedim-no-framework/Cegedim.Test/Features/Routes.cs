using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    public class Routes {
        MITouch m_miTouch;
        string routingCustomers;

        // the method naming here is important since the order of the tests matters
        [Test()]
        public void A_AddRoute() {
            var routeBuilderPage = Background();
            routeBuilderPage.DeleteAllRoutesNamed("Added Route");
            m_miTouch.Screenshot("I delete all routes named 'Added Route'");

            var newRoutePopover = routeBuilderPage.CreateNewRoutePopover();
            newRoutePopover.Name = "Added Route";
            newRoutePopover.SelectColor();
            m_miTouch.Screenshot("I create a new route popover with a name and color");

            newRoutePopover.AddCustomers(3, routingCustomers);
            m_miTouch.Screenshot("I've add 3 customers to the route");

            newRoutePopover.Done();
            m_miTouch.Screenshot("I'm done adding the route and see the route in the list");

            routeBuilderPage.ConfirmRouteWithDatabase("Added Route", routingCustomers, 3);
            m_miTouch.Screenshot("I find the route in the database");
        }

        [Test]
        public void B_EditRoute() {
            var routeBuilderPage = Background();
            routeBuilderPage.SelectRoute("Added Route");
            var editRoutePopover = routeBuilderPage.CreateEditRoutePopover();
            editRoutePopover.Name = "Changed Route";
            editRoutePopover.SelectColor(2);
            m_miTouch.Screenshot("I change the route to 'Changed Route'");

            int[] intArray = { 3, 4 };
            editRoutePopover.AddCustomersByIndex(routingCustomers, intArray);
            m_miTouch.Screenshot("I select route customers 4 and 5");

            editRoutePopover.Done();
            m_miTouch.Screenshot("I'm done editing the route and see the route in the list");

            routeBuilderPage.ConfirmRouteWithDatabase("Changed Route", routingCustomers, 5);
            m_miTouch.Screenshot("I find the route in the database");

            var editRoutePopoverRevisited = routeBuilderPage.CreateEditRoutePopover();
            m_miTouch.Screenshot("I reopen the edit route popover");

            editRoutePopoverRevisited.Name = "Added Route";
            m_miTouch.Screenshot("I change the route to Added Route");

            editRoutePopoverRevisited.Done();
            m_miTouch.Screenshot("I'm done editing the route and I see the route in the list");
        }

        [Test]
        public void C_NavigationToCustomer() {
            var routeBuilderPage = Background();
            routeBuilderPage.SelectRoute("Added Route");
            m_miTouch.Screenshot("I tap the route in the list");

            var summaryPage = routeBuilderPage.NavigateToCustomerSummaryPage(2);
            m_miTouch.Screenshot("I tap the row for route customer 3 and I see the customer summary page");

            summaryPage.Back();
            routeBuilderPage.ConfirmIsLoaded();
            m_miTouch.Screenshot("I tap the go back button and see the route builder page");
        }

        [Test]
        public void DeleteRoute() {
            var routeBuilderPage = Background();
            routeBuilderPage.SelectRoute("Added Route");
            m_miTouch.Screenshot("I tap the route in the list");

            var editRoutePopover = routeBuilderPage.CreateEditRoutePopover();
            var routeBuilderPageDeletedRoute = editRoutePopover.DeleteRoute();
            m_miTouch.Screenshot("I delete the route");

            routeBuilderPageDeletedRoute.ConfirmRouteDoesNotExist("Added Route");
            m_miTouch.Screenshot("I shouldnt see the route in the list");

            routeBuilderPageDeletedRoute.ConfirmRouteDoesNotExistInDatabase("Added Route");
            m_miTouch.Screenshot("I shouldnt find the route in the database");
        }

        public RouteBuilderPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            var routeBuilderPage = dashboard.NavigateToRouteBuilderPage();
            m_miTouch.Screenshot("I navigate to and see the route builder page");

            routingCustomers = routeBuilderPage.SelectRouteCustomers(5);
            m_miTouch.Screenshot("I select five route customers from the database");

            return routeBuilderPage;
        }
    }
}
  