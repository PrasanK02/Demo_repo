using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Automation;
using NUnit.Framework;

namespace Cegedim.Automation {
    public class CustomerActivityHistoryPage : CustomerPage {
        private static class Query {
            internal static string Loaded = "view {accessibilityIdentifier BEGINSWITH 'PAGE:' AND accessibilityIdentifier ENDSWITH '/activity.cdl'}";
            internal static string BackButton = "view:'UILabel' marked:'Back'";
            internal static string SegmentedBar = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar'";
        }

        internal CustomerActivityHistoryPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public void Verify(string markedName) {
            string queryString = Query.Loaded + String.Format(" descendant view:'*' marked:'{0}'", markedName);
            if (!TestIsVisible(queryString))
                Assert.Fail(queryString + " did not appear.");
        }

        public void TapAndVerify(string buttonName) {
            string buttonValue;
            if(buttonName == "Current Address" || buttonName == "Activity")
                buttonValue = "VAL:0";
            else
                buttonValue = "VAL:1";
            string buttonQuery = Query.Loaded + String.Format(" descendant * marked:'{0}'", buttonName);
            TapAndWait(buttonQuery, () => {
                string barQuery = buttonQuery + "parent " + Query.SegmentedBar;
                if(TestIsVisible(barQuery))
                    return buttonValue == Calabash.Query(barQuery).First().Id;
                else
                    return false;
            });
        }

        public SearchPage NavigateBack() {
            return AppConvention.TapActivateAndWait<SearchPage>(
                Application, Query.BackButton);
        }
    }
}