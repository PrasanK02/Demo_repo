using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using Xamarin.Automation.Calabash;

namespace Cegedim.Automation {

    public class EditOrderEntryPage : CegedimPage {
        public string deleteConfirmation;
        private static class Query {
            internal static string Loaded = "view id:'PAGE:orderentry/orderform.cdl'";
            internal static string BackButton = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'backButton'";
            internal static string SaveOnExitButton = "view:'Dendrite.IPhone.Forms.ShinyButton' index:0"; // No better binding available
            internal static string SaveButton = "view marked:'Order_Wizard_Save'";
            //internal static string OKButton = "view:'_UIModalItemTableViewCell' marked:'OK'";
            internal static string OKButton = "UILabel marked:'OK'";
            internal static string ProductsButton = "UILabel marked:'Products'";
            internal static string EditOrder = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'Edit_Order'";
            internal static string AddDelivery = "UILabel marked:' + '";
            internal static string OrderEntryTabs = "view:'MI.CustomControls.OfflineOrderEntryTabs+OETab'";
            internal static string MoreDetails = "view:'Dendrite.IPhone.Forms.CdlSegmentedBarWrapper' marked:'moreDetails'";
            internal static string OverallDiscount = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'DATA:vt_order_header/discount/offt' descendant view:'Dendrite.IPhone.Forms.CdlTextBox'";
            internal static string Refresh = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'refreshButton'";
            internal static string SendEmail = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'__sendEmail'";
            internal static string SummaryButton = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'wf_summary'";
            internal static string SummarySheet = "view marked:'PAGE:orderentry/ordersummaryformsheet.cdl'";
            internal static string SummaryCatalogueTab = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' label marked:'Catalogue'";
            internal static string SummaryCatalogueTabSelected = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' marked:'VAL:0'";
            internal static string SummaryProductsTab = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' label marked:'Products'";
            internal static string SummaryProductsTabSelected = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' marked:'VAL:1'";
            internal static string SummaryCategoriesTab = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' label marked:'Categories'";
            internal static string SummaryCategoriesTabSelected = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' marked:'VAL:2'";
            internal static string Close = "UINavigationButton marked:'Close'";
            internal static string Submit = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'SubmitButton'";
            internal static string SubmitWithoutSignature = "view:'Dendrite.IPhone.Forms.CdlButton' marked:'wf_oe_submit'";
            internal static string HomePage = "view id:'PAGE:home/home.cdl'";
            internal static string Delete = "view:'UIImageView' {accessibilityIdentifier ENDSWITH  'delete.png'} index:0";
            internal static string DeleteConfirmation = "view:'_UIModalItemTableViewCell' marked:'Delete'";
            internal static string DeleteConfirmationiOS8 = "view:'_UIAlertControllerActionView' marked:'Delete'";
        }

        internal EditOrderEntryPage(MITouch application, AppContainer container)
            : base(application, container) {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                if (Environment.GetEnvironmentVariable("XTC_DEVICE_OS").Contains("7."))
                    deleteConfirmation = Query.DeleteConfirmation;
                else
                    deleteConfirmation = Query.DeleteConfirmationiOS8;
            } else {
                // testing locally on iOS8 otherwise you may need to change this
                deleteConfirmation = Query.DeleteConfirmationiOS8;
            }
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public SearchPage SaveAndEdit() {
            TapAndWait(Query.BackButton, () => TestIsVisible(Query.SaveOnExitButton), postTimeout: TimeSpan.FromSeconds(1));
            return AppConvention.TapActivateAndWait<SearchPage>(Application, Query.SaveOnExitButton);
        }

        public void Save() {
            TapAndWait(Query.SaveButton, () => TestIsVisible(Query.OKButton), postTimeout: TimeSpan.FromSeconds(0.5));
            // TODO: Investigate why/if this won't respond to the initial touch
            try {
                TapAndWait(Query.OKButton, () => !TestIsVisible(Query.OKButton));
            } catch {
                TapAndWait(Query.OKButton, () => !TestIsVisible(Query.OKButton));
            }
        }

        public SendEmailPopover CreateSendEmailPopover() {
            return AppConvention.TapActivateAndWait<SendEmailPopover>(Application, Query.SendEmail);
        }

        public ProductCatalogPage AddProducts() {
            return AppConvention.TapActivateAndWait<ProductCatalogPage>(Application, Query.ProductsButton);
        }

        public void EditOrder() {
            TapAndWait(Query.EditOrder, () => TestIsVisible(Query.ProductsButton));
        }

        public DeliveryPopover CreateDeliveryPopover() {
            return AppConvention.TapActivateAndWait<DeliveryPopover>(Application, Query.AddDelivery);
        }

        public DeliveryPopover ReopenDeliveryPopover() {
            int tabCount = Calabash.Query(Query.OrderEntryTabs).Count();
            string tabQuery = string.Format("{0} index:{1}", Query.OrderEntryTabs, tabCount - 2);
            TapAndWait(tabQuery, () => TestIsVisible(tabQuery), postTimeout: TimeSpan.FromSeconds(0.5));
            return AppConvention.TapActivateAndWait<DeliveryPopover>(Application, tabQuery);
        }

        public OrderDetailsPopover CreateOrderDetailsPopover() {
            return AppConvention.TapActivateAndWait<OrderDetailsPopover>(Application, Query.MoreDetails);
        }

        public OverallDiscountPopover CreateOverallDiscountPopover() {
            return AppConvention.TapActivateAndWait<OverallDiscountPopover>(Application, Query.OverallDiscount);
        }

        public void Refresh() {
            var initialQuery = Calabash.Query("*");
            TapAndWait(Query.Refresh, () => initialQuery != Calabash.Query("*"));
        }

        public void CheckSummary() {
            TapAndWait(Query.SummaryButton, () => TestIsVisible(Query.SummaryCatalogueTabSelected), postTimeout: TimeSpan.FromSeconds(0.6));
            TapAndWait(Query.SummaryProductsTab, () => TestIsVisible(Query.SummaryProductsTabSelected));
            TapAndWait(Query.SummaryCategoriesTab, () => TestIsVisible(Query.SummaryCategoriesTabSelected));
            TapAndWait(Query.Close, () => TestIsVisible(Query.Loaded));
        }

        public DashboardPage Submit() {
            if (Calabash.Query(Query.Submit).Count() > 1) {
                // TODO: Submit with signature
                return new DashboardPage(Application, Container);
            }
            else {
                TapAndWait(Query.Submit, () => TestIsVisible(Query.SubmitWithoutSignature), postTimeout: TimeSpan.FromSeconds(0.5));
                TapAndWait(Query.SubmitWithoutSignature, () => TestIsVisible(Query.OKButton), postTimeout: TimeSpan.FromSeconds(0.5));
                return AppConvention.TapActivateAndWait<DashboardPage>(Application, Query.OKButton);
            }
        }

        public DashboardPage Delete() {
            TapAndWait(Query.Delete, () => TestIsVisible(deleteConfirmation), postTimeout: TimeSpan.FromSeconds(0.5));
            return AppConvention.TapActivateAndWait<DashboardPage>(Application, deleteConfirmation);
        }
    }

    public class SendEmailPopover : Popover {
        private string m_email;
        private static class Query {
            internal static string Loaded = "view marked:'PAGE:orderentry/orderemailpopup.cdl'";
            internal static string EmailField = "view id:'DATA:vt_order_entry_new_email/vc_email_address'";
        }

        internal SendEmailPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { 
                Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                return TestIsVisible(Query.Loaded); 
            }
        }

        public string EmailAddress {
            get {
                return m_email;
            }
            set {
                string emailQuery = string.Format("view marked:'{0}'", value);
                if (TestIsVisible(emailQuery))
                    SelectEmailAddress(value);
                else {
                    TapAndWait(Query.EmailField, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.EmailField, value);
                    Calabash.Tap(CalabashButton.Enter);
                    Wait(() => !IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                }
                m_email = value;
            }
        }

        public void SelectEmailAddress(string emailAddress) {
            string emailQuery = string.Format("view:'Dendrite.IPhone.Forms.GridControl.GridTableRowCell' id:'KEY:{0}'", emailAddress);
            if (!TestIsVisible(emailQuery))
                Assert.Fail("Email address not found");
            string checkMarkQuery = string.Format("{0} parent * index:0 descendant UIImageView index:0", emailQuery);
            TapAndWait(emailQuery, () => TestIsVisible(checkMarkQuery));
        }
    }

    public class OverallDiscountPopover : Popover {
        private double m_discount;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'Overall Discount'";
            internal static string Discount = "view:'Dendrite.IPhone.Forms.CdlEditItem' marked:'DATA:vt_order_header/discount/ipad'";
            internal static string DiscountNumberPad = "UINavigationBar marked:'Discount'";
            internal static string Done = "UINavigationButton marked:'Done'";
        }

        internal OverallDiscountPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { 
                Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                return TestIsVisible(Query.Loaded); 
            }
        }

        public double Discount {
            get {
                return m_discount;
            }
            set {
                TapAndWait(Query.Discount, () => TestIsVisible(Query.DiscountNumberPad), postTimeout: TimeSpan.FromSeconds(0.7));
                var valueCharArray = value.ToString().ToCharArray();
                foreach (char c in valueCharArray) {
                    string queryString = string.Format("UIButton marked:'{0}'", c);
                    string visibleString = string.Format("UITextFieldLabel {{text contains '{0}'}} index:0", c);
                    TapAndWait(queryString, () => TestIsVisible(visibleString), postTimeout: TimeSpan.FromSeconds(0.5));
                }
                m_discount = value;
                TapAndWait(Query.Done, () => TestIsVisible(Query.Loaded), postTimeout: TimeSpan.FromSeconds(0.6));
            }
        }
    }

    public class OrderDetailsPopover : DropShadowPopover {
        private string m_deliveryAddress;
        private string m_address1;
        private string m_city;
        private string m_postalCode;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'Order Details'";
        }

        internal OrderDetailsPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { 
                Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                return TestIsVisible(Query.Loaded); 
            }
        }

        public string DeliveryAddress {
            get { return m_deliveryAddress; }
            set {
                SelectDropDown("Delivery Address", value, postTimeout: TimeSpan.FromSeconds(1));
                m_deliveryAddress = value;
            }
        }

        public string Address1 {
            get { return m_address1; }
            set {
                AddText("Address 1", value, clearText: true, tapActionKey: true);
                m_address1 = value;
            }
        }

        public string City {
            get { return m_city; }
            set {
                AddText("City", value, clearText: true, tapActionKey: true);
                m_city = value;
            }
        }

        public string PostalCode {
            get { return m_postalCode; }
            set {
                AddText("Postal Code", value, clearText: true, tapActionKey: true);
                m_postalCode = value;
            }
        }
    }

    public class DeliveryPopover : Popover {
        private Lazy<Calendar> m_calendar;
        private DateTime m_date;
        public string deleteDelivery;
        private static class Query {
            internal static string Loaded = "UINavigationBar marked:'Delivery'";
            internal static string Popover = "view:'_UIPopoverView'";
            internal static string DeliveryDate = "view marked:'DATA:vt_order_delivery/delivery_date/odle'";
            internal static string Delete = "UIButton marked:'Delete'";
            internal static string DeleteDelivery = "UIAlertButton marked:'Delete Delivery'";
            internal static string DeleteDeliveryiOS8 = "view:'_UIAlertControllerActionView' marked:'Delete Delivery'";
        }

        internal DeliveryPopover(MITouch application, AppContainer container)
            : base(application, container) {
            m_calendar = new Lazy<Calendar>(() => CalendarFactory(Query.Popover));
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                if (Environment.GetEnvironmentVariable("XTC_DEVICE_OS").Contains("7."))
                    deleteDelivery = Query.DeleteDelivery;
                else
                    deleteDelivery = Query.DeleteDeliveryiOS8;
            } else {
                // testing locally on iOS8 otherwise you may need to change this
                deleteDelivery = Query.DeleteDeliveryiOS8;
            }
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        private Calendar CalendarFactory(string query) {
            return new Calendar(Application, this, Container.GetDescendant(query));
        }

        public DateTime DeliveryDate {
            get { return m_date; }
            set {
                TapAndWait(Query.DeliveryDate, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
                m_calendar.Value.Year = value.Year;
                m_calendar.Value.Month = value.ToString("MMMM");
                m_calendar.Value.Day = value.Day;
                m_date = value;
            }
        }

        public void Delete() {
            TapAndWait(Query.Delete, () => TestIsVisible(deleteDelivery));
            TapAndWait(deleteDelivery, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(0.5));
        }
    }

    public class ProductCatalogPage : DropShadowPopover {
        private static class Query {
            internal static string Loaded = "view marked:'PAGE:orderentry/productcatalogformsheet.cdl'";
            internal static string ProductCatalogTree = "view:'MI.CustomControls.ProductCatalogTreeLabel'";
            internal static string CatalogDisplay = "view:'Dendrite.IPhone.Forms.CdlGrid' id:'catalogDisplay'";
        }

        internal ProductCatalogPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public void SelectCatalogAtRandom() {
            int catalogCount = Calabash.Query(Query.ProductCatalogTree).Count();
            Random random = new Random();
            int sampleVal = random.Next(0, catalogCount - 1);
            string catalogQuery = string.Format("{0} index:{1}", Query.ProductCatalogTree, sampleVal.ToString());
            string itemQuery = string.Format("{0} descendant UITextFieldLabel index:0", Query.CatalogDisplay);
            Wait(() => TestIsVisible(catalogQuery), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(catalogQuery, () => TestIsVisible(itemQuery), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        // After picking a catalog
        public ProductDetailPopover SelectProductAtRandom() {
            string itemQuery = string.Format("{0} descendant UITableViewCell", Query.CatalogDisplay);
            int itemCount = Calabash.Query(itemQuery).Count();
            Random random = new Random();
            int sampleVal = random.Next(0, itemCount - 1);
            string selectedItemQuery = string.Format("{0} index:{1}", itemQuery, sampleVal.ToString());
            return AppConvention.TapActivateAndWait<ProductDetailPopover>(Application, selectedItemQuery);
        }
    }

    public class ProductDetailPopover : DropShadowPopover {
        private int m_deliveryQuantity;
        private int m_deliveryFreeQuantity;
        private int m_extraDiscountPercent;
        private int m_extraDiscount;
        private int m_quantityLeft;
        private int m_totalExtraDiscountPercent;
        private int m_totalExtraDiscountAmount;
        private static class Query {
            internal static string Loaded = "view marked:'PAGE:orderentry/productdetailformsheet.cdl'";
            internal static string DeliveryQuantityField = "view:'MI.CustomControls.QuantityControlView' marked:'DATA:vt_order_detail_delivery/quantity/oemt'";
            internal static string DeliveryFreeQuantity = "view marked:'__deliveryFreeQty'";
            internal static string ExtraDiscountPercent = "view marked:'__discountValuePercent'";
            internal static string ExtraDiscount = "view marked:'__discountValueAmount'";
            internal static string QuantityLeft = "view marked:'__quantityLeftByUser'";
            internal static string TotalExtraDiscountPercent = "view marked:'__totalExtraDiscount'";
            internal static string TotalExtraDiscountAmount = "view marked:'__totalExtraDiscountAmt'";
        }

        internal ProductDetailPopover(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public int DeliveryQuantity {
            get { return m_deliveryQuantity; }
            set {
                if (TestIsVisible(Query.DeliveryQuantityField)) {
                    TapAndWait(Query.DeliveryQuantityField, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.DeliveryQuantityField, value.ToString());
                    m_deliveryQuantity = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int DeliveryFreeQuantity {
            get { return m_deliveryFreeQuantity; }
            set {
                // Apparently certain options here are not always present
                if (TestIsVisible(Query.DeliveryFreeQuantity)) {
                    TapAndWait(Query.DeliveryFreeQuantity, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.DeliveryFreeQuantity, value.ToString());
                    m_deliveryQuantity = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int ExtraDiscountPercent {
            get { return m_extraDiscountPercent; }
            set {
                if (TestIsVisible(Query.ExtraDiscountPercent)) {
                    TapAndWait(Query.ExtraDiscountPercent, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.ExtraDiscountPercent, value.ToString());
                    m_extraDiscountPercent = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int ExtraDiscount {
            get { return m_extraDiscount; }
            set {
                if (TestIsVisible(Query.ExtraDiscount)) {
                    TapAndWait(Query.ExtraDiscount, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.ExtraDiscount, value.ToString());
                    m_extraDiscount = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int QuantityLeft {
            get { return m_quantityLeft; }
            set {
                if (TestIsVisible(Query.QuantityLeft)) {
                    TapAndWait(Query.QuantityLeft, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.QuantityLeft, value.ToString());
                    m_quantityLeft = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int TotalExtraDiscountPercent {
            get { return m_totalExtraDiscountPercent; }
            set {
                if (TestIsVisible(Query.TotalExtraDiscountPercent)) {
                    TapAndWait(Query.TotalExtraDiscountPercent, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.TotalExtraDiscountPercent, value.ToString());
                    m_totalExtraDiscountPercent = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }

        public int TotalExtraDiscountAmount {
            get { return m_totalExtraDiscountAmount; }
            set {
                if (TestIsVisible(Query.TotalExtraDiscountAmount)) {
                    TapAndWait(Query.TotalExtraDiscountAmount, () => IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.5));
                    SetField(Query.TotalExtraDiscountAmount, value.ToString());
                    m_totalExtraDiscountAmount = value;
                    Calabash.Tap(CalabashButton.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
                }
            }
        }
    }
}