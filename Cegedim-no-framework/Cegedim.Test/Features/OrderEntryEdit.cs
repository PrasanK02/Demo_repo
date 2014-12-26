using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    public class OrderEntryEdit {
        MITouch m_miTouch;

        [Test()]
        public void NewOrderAddProducts() {
            var dashboardPage = Background();
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

            // TODO: Setting the Order Account doesn't seem to be an actual option in the Calabash test from the fork I have
            // Order account is available for only some orders
            newOrderPopover.Account = "Account 01";
            m_miTouch.Screenshot("I set the account option if possible for this order");

            newOrderPopover.OrderPriceList = "Pricelistname_100_2";
            m_miTouch.Screenshot("I set the order pricelist if possible for this order");

            newOrderPopover.ExchangeAndReturn = "Pricelistname_100_2";
            m_miTouch.Screenshot("I set the order exchange and return if possible for this order");

            var editOrderEntryPage = newOrderPopover.Continue();
            m_miTouch.Screenshot("I tap the continue button");

            editOrderEntryPage.Save();
            m_miTouch.Screenshot("I save the order");

            var productsCatalogPage = editOrderEntryPage.AddProducts();
            m_miTouch.Screenshot("I select to add products");

            productsCatalogPage.SelectCatalogAtRandom();
            m_miTouch.Screenshot("I randomly select a product catalog");

            var productDetailPopover = productsCatalogPage.SelectProductAtRandom();
            m_miTouch.Screenshot("I randomly select a proudct from the catalog and see the product detail popover");

            productDetailPopover.DeliveryQuantity = 100;
            productDetailPopover.DeliveryFreeQuantity = 10;
            productDetailPopover.ExtraDiscountPercent = 3;
            productDetailPopover.ExtraDiscount = 3;
            productDetailPopover.QuantityLeft = 3;
            productDetailPopover.TotalExtraDiscountPercent = 3;
            productDetailPopover.TotalExtraDiscountAmount = 10;
            m_miTouch.Screenshot("I set the product properties");

            productDetailPopover.Done();
            m_miTouch.Screenshot("I finish adding the product properties");

            productsCatalogPage.Done();
            m_miTouch.Screenshot("I finish the product catalog page");

            editOrderEntryPage.Save();
            m_miTouch.Screenshot("I save the order");

            // TODO: Work in Progress on the Ruby version at the time of my fork
            dashboardPage.SetDefaultCustomerType(defaultCustomerType);
            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I set the database state back for the New Order test");
        }

        [Test()]
        public void EditOrder() {
            var dashboardPage = Background();
            dashboardPage.UpdateDatabaseSubmitOrderWithOutSignature();
            m_miTouch.Screenshot("I set the database for order submit");

            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I reset the services");

            var incompleteOrdersPage = dashboardPage.NavigateToIncompleteOrdersPage();
            m_miTouch.Screenshot("I see the incomplete order");

            var editOrderEntryPage = incompleteOrdersPage.EditOrderAtRandom();
            m_miTouch.Screenshot("I select the order for editing");

            editOrderEntryPage.EditOrder();
            m_miTouch.Screenshot("I tap the edit order button");

            var deliveryPopover = editOrderEntryPage.CreateDeliveryPopover();
            m_miTouch.Screenshot("I open the delivery pop over");

            deliveryPopover.DeliveryDate = DateTime.Now.AddDays(21);
            m_miTouch.Screenshot("I set the delivery date to 21 days from now");

            deliveryPopover.Done();
            m_miTouch.Screenshot("I set the delivery options");

            var deliveryPopoverRevisited = editOrderEntryPage.ReopenDeliveryPopover();
            deliveryPopoverRevisited.Delete();
            m_miTouch.Screenshot("I delete the delivery");

            var productsCatalogPage = editOrderEntryPage.AddProducts();
            m_miTouch.Screenshot("I select to add products");

            productsCatalogPage.SelectCatalogAtRandom();
            m_miTouch.Screenshot("I randomly select a product catalog");

            var productDetailPopover = productsCatalogPage.SelectProductAtRandom();
            m_miTouch.Screenshot("I randomly select a proudct from the catalog and see the product detail popover");

            productDetailPopover.DeliveryQuantity = 100;
            productDetailPopover.DeliveryFreeQuantity = 10;
            productDetailPopover.ExtraDiscountPercent = 3;
            productDetailPopover.ExtraDiscount = 3;
            productDetailPopover.QuantityLeft = 3;
            productDetailPopover.TotalExtraDiscountPercent = 3;
            productDetailPopover.TotalExtraDiscountAmount = 10;
            m_miTouch.Screenshot("I set the product properties");

            productDetailPopover.Done();
            m_miTouch.Screenshot("I finish adding the product properties");

            productsCatalogPage.Done();
            m_miTouch.Screenshot("I finish the product catalog page");

            var orderDetailsPopover = editOrderEntryPage.CreateOrderDetailsPopover();
            m_miTouch.Screenshot("I open the order details");

            orderDetailsPopover.DeliveryAddress = "New Address";
            orderDetailsPopover.Address1 = "Address1";
            orderDetailsPopover.City = "City1";
            orderDetailsPopover.PostalCode = "12345";
            m_miTouch.Screenshot("I enter the new address information");

            orderDetailsPopover.Done();
            m_miTouch.Screenshot("I finish the order details");

            var overallDiscountPopover = editOrderEntryPage.CreateOverallDiscountPopover();
            m_miTouch.Screenshot("I've opened the discount popover");

            overallDiscountPopover.Discount = 1.5;
            m_miTouch.Screenshot("I've set the overall discount");

            overallDiscountPopover.Done();
            m_miTouch.Screenshot("I close the discount popover");

            editOrderEntryPage.Refresh();
            m_miTouch.Screenshot("I recalculate the order");

            var sendEmailPopover = editOrderEntryPage.CreateSendEmailPopover();
            //sendEmailPopover.SelectEmailAddress("id2@cegedim.com");
            sendEmailPopover.EmailAddress = "id2@cegedim.com";
            m_miTouch.Screenshot("I've selected an email address");

            sendEmailPopover.Done();
            m_miTouch.Screenshot("I've sent the email");

            editOrderEntryPage.CheckSummary();
            m_miTouch.Screenshot("I check the summary");

            var finalDashboardPage = editOrderEntryPage.Submit();
            m_miTouch.Screenshot("I submit the order without a signature and arrive on the dashboard page");
        }

        //[Test()]
        // TODO: I'm not getting any differences when using the signature page
        public void EditOrderWithSignature() {
            var dashboardPage = Background();
            dashboardPage.UpdateDatabaseSubmitOrderWithSignature();
            m_miTouch.Screenshot("I set the database for order submit");

            dashboardPage.ResetServiceRuleStore();
            m_miTouch.Screenshot("I reset the services");

            var incompleteOrdersPage = dashboardPage.NavigateToIncompleteOrdersPage();
            m_miTouch.Screenshot("I see the incomplete order");

            var editOrderEntryPage = incompleteOrdersPage.EditOrderAtRandom();
            m_miTouch.Screenshot("I select the order for editing");

            editOrderEntryPage.EditOrder();
            m_miTouch.Screenshot("I tap the edit order button");

            var deliveryPopover = editOrderEntryPage.CreateDeliveryPopover();
            m_miTouch.Screenshot("I open the delivery pop over");

            deliveryPopover.DeliveryDate = DateTime.Now.AddDays(21);
            m_miTouch.Screenshot("I set the delivery date to 21 days from now");

            deliveryPopover.Done();
            m_miTouch.Screenshot("I set the delivery options");

            var deliveryPopoverRevisited = editOrderEntryPage.ReopenDeliveryPopover();
            deliveryPopoverRevisited.Delete();
            m_miTouch.Screenshot("I delete the delivery");

            var productsCatalogPage = editOrderEntryPage.AddProducts();
            m_miTouch.Screenshot("I select to add products");

            productsCatalogPage.SelectCatalogAtRandom();
            m_miTouch.Screenshot("I randomly select a product catalog");

            var productDetailPopover = productsCatalogPage.SelectProductAtRandom();
            m_miTouch.Screenshot("I randomly select a proudct from the catalog and see the product detail popover");

            productDetailPopover.DeliveryQuantity = 100;
            productDetailPopover.DeliveryFreeQuantity = 10;
            productDetailPopover.ExtraDiscountPercent = 3;
            productDetailPopover.ExtraDiscount = 3;
            productDetailPopover.QuantityLeft = 3;
            productDetailPopover.TotalExtraDiscountPercent = 3;
            productDetailPopover.TotalExtraDiscountAmount = 10;
            m_miTouch.Screenshot("I set the product properties");

            productDetailPopover.Done();
            m_miTouch.Screenshot("I finish adding the product properties");

            productsCatalogPage.Done();
            m_miTouch.Screenshot("I finish the product catalog page");

            var orderDetailsPopover = editOrderEntryPage.CreateOrderDetailsPopover();
            m_miTouch.Screenshot("I open the order details");

            orderDetailsPopover.DeliveryAddress = "New Address";
            orderDetailsPopover.Address1 = "Address1";
            orderDetailsPopover.City = "City1";
            orderDetailsPopover.PostalCode = "12345";
            m_miTouch.Screenshot("I enter the new address information");

            orderDetailsPopover.Done();
            m_miTouch.Screenshot("I finish the order details");

            var overallDiscountPopover = editOrderEntryPage.CreateOverallDiscountPopover();
            m_miTouch.Screenshot("I've opened the discount popover");

            overallDiscountPopover.Discount = 1.5;
            m_miTouch.Screenshot("I've set the overall discount");

            overallDiscountPopover.Done();
            m_miTouch.Screenshot("I close the discount popover");

            editOrderEntryPage.Refresh();
            m_miTouch.Screenshot("I recalculate the order");

            var sendEmailPopover = editOrderEntryPage.CreateSendEmailPopover();
            //sendEmailPopover.SelectEmailAddress("id2@cegedim.com");
            sendEmailPopover.EmailAddress = "id2@cegedim.com";
            m_miTouch.Screenshot("I've selected an email address");

            sendEmailPopover.Done();
            m_miTouch.Screenshot("I've sent the email");

            editOrderEntryPage.CheckSummary();
            m_miTouch.Screenshot("I check the summary");

            var finalDashboardPage = editOrderEntryPage.Submit();
            m_miTouch.Screenshot("I submit the order and arrive on the dashboard page");
        }

        [Test()]
        public void DeleteOrder() {
            var dashboardPage = Background();
            var incompleteOrdersPage = dashboardPage.NavigateToIncompleteOrdersPage();
            m_miTouch.Screenshot("I see the incomplete order");

            var editOrderEntryPage = incompleteOrdersPage.EditOrderAtRandom();
            m_miTouch.Screenshot("I select the order for editing");

            var finalDashboardPage = editOrderEntryPage.Delete();
            m_miTouch.Screenshot("I delete the order");
        }

        public DashboardPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            int incompleteOrdersCount = dashboard.CountIncompleteOrders();
            m_miTouch.Screenshot(string.Format("I count {0} incomplete orders", incompleteOrdersCount.ToString()));

            return dashboard;
        }
    }
}

