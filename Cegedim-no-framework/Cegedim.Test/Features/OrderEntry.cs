using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    [Category("ExtendedTest")]
    public class OrderEntry {
        MITouch m_miTouch;

        [Test()]
        public void NewOrder() {
            var dashboardPage = Background();
            int incompleteOrdersCount = dashboardPage.CountIncompleteOrders();
            m_miTouch.Screenshot(string.Format("I count {0} incomplete orders", incompleteOrdersCount.ToString()));

            var orderEntryCustomers = dashboardPage.GetOrderEntryCustomers(10);
            m_miTouch.Screenshot("I get a list of order entry customers");

            var defaultCustomerType = dashboardPage.GetDefaultCustomerType();
            m_miTouch.Screenshot("I get the default customer type");

            // TODO: Work in Progress on the Ruby version at the time of my fork
            dashboardPage.SetDefaultCustomerType(defaultCustomerType);
            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I set the database state back for the New Order test");

            var searchPage = dashboardPage.NavigateToSearchPage();
            m_miTouch.Screenshot("I see the customer search page");

            string[] orderEntryCustomerInfo = searchPage.SampleOrderEntryCustomers(orderEntryCustomers);
            string sampledOrderEntryCustomer = orderEntryCustomerInfo[0];
            string sampledOrderEntryCustomerType = orderEntryCustomerInfo[1];
            // Turn on appropriate filters for this to work
            searchPage.SetFilter(sampledOrderEntryCustomerType);
            searchPage.SearchFor(sampledOrderEntryCustomer);
            m_miTouch.Screenshot("I search for an order entry customer");

            searchPage.AssertCustomerCount(1);
            m_miTouch.Screenshot("I verify the search count for the order entry customer is 1");

            searchPage.AssertCustomer(sampledOrderEntryCustomer);
            m_miTouch.Screenshot("I see the correct order entry contact");

            searchPage.AssertContactButtons();
            m_miTouch.Screenshot("I see a contact button for each customer");

            var moreActionsPopover = searchPage.MoreActions(sampledOrderEntryCustomer);
            m_miTouch.Screenshot("I tap more actions on the order entry customer");

            var newOrderPopover = moreActionsPopover.CreateNewOrder();
            m_miTouch.Screenshot("I start creating a new order");

            newOrderPopover.OrderChannel = "Call Center";
            m_miTouch.Screenshot("I set the order channel");

            newOrderPopover.Account = "Account 01";
            m_miTouch.Screenshot("I set the account option if possible for this order");

            newOrderPopover.OrderPriceList = "Pricelistname_100_2";
            m_miTouch.Screenshot("I set the order pricelist if possible for this order");

            newOrderPopover.ExchangeAndReturn = "Pricelistname_100_2";
            m_miTouch.Screenshot("I set the order exchange and return if possible for this order");

            var editOrderEntryPage = newOrderPopover.Continue();
            m_miTouch.Screenshot("I tap the continue button");

            var searchPageRevisited = editOrderEntryPage.SaveAndEdit();
            m_miTouch.Screenshot("I save the order and exit");

            var dashboardPageRevisted = searchPageRevisited.NavigateToDashboardPage();
            m_miTouch.Screenshot("I navigate back to the dashboard page");

            int updatedIncompleteOrderCount = dashboardPageRevisted.CountIncompleteOrders();
            m_miTouch.Screenshot("I check the incomplete orders count");

            if (updatedIncompleteOrderCount != incompleteOrdersCount + 1)
                Assert.Fail("The incomplete orders did not increment by one");
            m_miTouch.Screenshot("I observe the incomplete orders count is increased by one");

            // TODO: Work in Progress on the Ruby version at the time of my fork
            dashboardPage.SetDefaultCustomerType(defaultCustomerType);
            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I set the database state back for the New Order test");
        }

        [Test()]
        public void NewOrderWithInactiveAccount() {
            var dashboardPage = Background();
            int incompleteOrdersCount = dashboardPage.CountIncompleteOrders();
            m_miTouch.Screenshot(string.Format("I count {0} incomplete orders", incompleteOrdersCount.ToString()));

            var inactiveOrderEntryCustomers = dashboardPage.GetInactiveOrderEntryCustomers(10);
            m_miTouch.Screenshot("I get a list of inactive order entry customers");

            var defaultCustomerType = dashboardPage.GetDefaultCustomerType();
            m_miTouch.Screenshot("I get the default customer type");

            // TODO: Work in Progress on the Ruby version at the time of my fork
            dashboardPage.SetDefaultCustomerType(defaultCustomerType);
            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I set the database state back for the New Order test");

            var searchPage = dashboardPage.NavigateToSearchPage();
            m_miTouch.Screenshot("I see the customer search page");

            string[] inactiveOrderEntryCustomerInfo = searchPage.SampleOrderEntryCustomers(inactiveOrderEntryCustomers);
            string sampledInactiveOrderEntryCustomer = inactiveOrderEntryCustomerInfo[0];
            string sampledInactiveOrderEntryCustomerType = inactiveOrderEntryCustomerInfo[1];
            // Turn on appropriate filters for this to work
            searchPage.SetFilter(sampledInactiveOrderEntryCustomerType);
            searchPage.SearchFor(sampledInactiveOrderEntryCustomer);
            m_miTouch.Screenshot("I search for an order entry customer");

            searchPage.AssertCustomerCount(1);
            m_miTouch.Screenshot("I verify the search count for the order entry customer is 1");

            searchPage.AssertCustomer(sampledInactiveOrderEntryCustomer);
            m_miTouch.Screenshot("I see the correct order entry contact");

            searchPage.AssertContactButtons();
            m_miTouch.Screenshot("I see a contact button for each customer");

            var moreActionsPopover = searchPage.MoreActions(sampledInactiveOrderEntryCustomer);
            m_miTouch.Screenshot("I tap more actions on the order entry customer");

            moreActionsPopover.CreateInactiveNewOrder();
            m_miTouch.Screenshot("I tap the new order option and see the confirmation message");

            var searchPageRevisited = moreActionsPopover.OK();
            // TODO: Work in Progress on the Ruby version at the time of my fork
            searchPageRevisited.SetDefaultCustomerType(defaultCustomerType);
            searchPageRevisited.ResetServiceRuleStore();
            m_miTouch.Screenshot("I tap OK and set the database state back for new order");
        }

        public DashboardPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            return dashboard;
        }
    }
}

