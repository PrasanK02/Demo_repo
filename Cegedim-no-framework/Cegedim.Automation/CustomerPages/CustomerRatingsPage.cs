using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Automation;
using NUnit.Framework;

namespace Cegedim.Automation {
    public class CustomerRatingsPage : CustomerPage {
        private static class Query {
            internal static string Loaded = "view {accessibilityIdentifier BEGINSWITH 'PAGE:' AND accessibilityIdentifier ENDSWITH '/rating.cdl'}";
            internal static string TherapeutiClassDropDown = "* marked:'Therapeutic Class' sibling view:'Dendrite.IPhone.Forms.CdlDropDownList'";
            internal static string TherapeuticClassPopover = "view:'Dendrite.IPhone.Forms.CdlGrid' id:'select_theurapetic_class'";
            internal static string TherapeuticClassBase = "view:'Dendrite.IPhone.Forms.CdlBaseTitle+TitleLabel'";
        }

        public string m_TherapeticClass = "";

        internal CustomerRatingsPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public string TherapeuticClass {
            get {
                TapAndWait(Query.TherapeutiClassDropDown, () => TestIsVisible(Query.TherapeuticClassPopover));
                return m_TherapeticClass;
            }
            set {
                TapAndWait(Query.TherapeutiClassDropDown, () => TestIsVisible(Query.TherapeuticClassPopover));
                string queryString = Query.TherapeuticClassPopover + String.Format(" descendant view:'*' marked:'{0}'", value);
                TapAndWait(queryString, () => !TestIsVisible(Query.TherapeuticClassPopover), timeout: TimeSpan.FromSeconds(15));
                m_TherapeticClass = value;
            }
        }

        public void Verify(string markedName) {
            string queryString = Query.Loaded + String.Format(" descendant view:'*' marked:'{0}'", markedName);
            if (!TestIsVisible(queryString))
                Assert.Fail(queryString + " did not appear.");
        }

        public void VerifyTherapeuticClass(string therapeuticClassName) {
            string queryString = Query.TherapeuticClassBase + string.Format(" marked:'{0}'", therapeuticClassName);
            if (!TestIsVisible(queryString))
                Assert.Fail(queryString + " did not appear.");
        }
    }
}