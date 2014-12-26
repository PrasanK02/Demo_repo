using System;
using System.Threading;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;

namespace Cegedim.Automation {

    public class ModifyPopover : CegedimSubPage {

        private static class Query {
            internal static string Loaded = "view:'_UIPopoverView'";
            internal static string DropDownList = "view:'Dendrite.IPhone.Forms.CdlDropDownList'";
            internal static string TextField = "view:'Dendrite.IPhone.Forms.CdlTextBox'";
            internal static string TableViewList = "view:'TableView' marked:'Empty list'";
            internal static string TableRows = "view:'Dendrite.IPhone.Forms.CdlTableViewCell'";
            internal static string EditItem = "view:'Dendrite.IPhone.Forms.CdlEditItem'";
        }

        internal ModifyPopover(MITouch application, CegedimEntity parent, AppElement element) 
            : base(application, parent, element) {
        }

        public override bool IsLoaded {
            get { return true; }
        }

        public void AddDropDown(int index, string headerTitle, string itemName) {
            var dropDownQuery = Query.DropDownList + " index:" + index.ToString();
            var itemQuery = string.Format("UILabel marked:'{0}' index:0", itemName);
            var headerQuery = string.Format("UINavigationItemView marked:'{0}' index:0", headerTitle);
            if (TestIsVisible(dropDownQuery)) {
                TapAndWait(dropDownQuery, () => TestIsVisible(headerQuery));
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Acts as post timeout
                SwipeDownUntil(Query.TableViewList, itemQuery);
                TapAndWait(itemQuery, () => TestIsVisible(Query.Loaded));
            }
            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
        }

        public void AddDropDown(string fieldName, string headerTitle, string itemName) {
            // Try to find the relative dropdown list from the field name in the form
            var dropDownQuery = string.Format("* marked:'{0}' parent {1} index:0 descendant {2} index:0", fieldName, Query.EditItem,Query.DropDownList);
            var itemQuery = string.Format("UILabel marked:'{0}' index:0", itemName);
            var headerQuery = string.Format("UINavigationItemView marked:'{0}' index:0", headerTitle);
            if (TestIsVisible(dropDownQuery)) {
                TapAndWait(dropDownQuery, () => AppConvention.TestIsVisible(Container, headerQuery));
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Acts as post timeout
                SwipeDownUntil(Query.TableViewList, itemQuery);
                TapAndWait(itemQuery, () => TestIsVisible(Query.Loaded));
            }
            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
        }

        public void AddTextField(int index, string itemName) {
            var textFieldQuery = Query.TextField + " index:" + index.ToString();
            if (TestIsVisible(textFieldQuery)) {
                SetField(textFieldQuery, itemName);
                Calabash.Tap(CalabashButton.Enter);
            }
        }

        public void AddTextField(string fieldName, string itemName) {
            // Try to find the relative textfield from the field name in the form
            var textFieldQuery = string.Format("view marked:'{0}' parent {1} index:0 descendant {2} index:0", fieldName, Query.EditItem, Query.TextField);
            if (TestIsVisible(textFieldQuery)) {
                SetField(textFieldQuery, itemName);
                Calabash.Tap(CalabashButton.Enter);
            }
        }
    }
}







