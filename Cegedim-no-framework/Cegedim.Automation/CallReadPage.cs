using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;

namespace Cegedim.Automation {
    public class CallReadPage : CegedimPage {
        public static string LatestEventId;
        private static class Query {
            internal static string Loaded = "view id:'PAGE:calls/callread.cdl'";
            internal static string BackButton = "view:'UILabel' marked:'Back'";
            internal static string CustomerSummaryPage = "view id:'PAGE:pres/summary.cdl'";
        }

        internal CallReadPage(MITouch application, AppContainer container)
            : base(application, container) {
            LatestEventId = Calabash.RetrieveLatestEventId();
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public SearchPage NavigateToSearchPage() {
            TapAndWait(Query.BackButton, () => TestIsVisible(Query.CustomerSummaryPage), postTimeout: TimeSpan.FromSeconds(1));
            return AppConvention.TapActivateAndWait<SearchPage>(
                Application, Query.BackButton);
        }
    }
}