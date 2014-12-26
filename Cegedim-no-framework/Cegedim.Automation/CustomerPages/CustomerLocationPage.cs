using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Automation;
using NUnit.Framework;

namespace Cegedim.Automation {
    public class CustomerLocationPage : CustomerPage {
        private static class Query {
            internal static string Loaded = "view {accessibilityIdentifier BEGINSWITH 'PAGE:' AND accessibilityIdentifier ENDSWITH '/location.cdl'}";
        }

        internal CustomerLocationPage(MITouch application, AppContainer container)
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
    }
}
