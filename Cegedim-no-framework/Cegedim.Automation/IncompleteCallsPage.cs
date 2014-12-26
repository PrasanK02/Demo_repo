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
    public class IncompleteCallsPage : CegedimPage {
        private string LatestEventId;
        private static class Query {
            internal static string Loaded = "view id:'PAGE:home/incompletecalls.cdl'";
            internal static string BackButton = "view:'UILabel' marked:'Back'";
            internal static string FinishButton = "view marked:'btnFinishCall'";
            internal static string ErrorMessage = "UIImageView {accessibilityIdentifier ENDSWITH 'infomessagewarningnoblock.png'}";
        }

        internal IncompleteCallsPage(MITouch application, AppContainer container)
            : base(application, container) {
            LatestEventId = JArray.Parse(CallReadPage.LatestEventId).First["eventid"].ToString();
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public void FinishIncompleteCall() {
            string incompleteCallQuery = string.Format("view marked:'KEY:{0}'", LatestEventId);
            TapAndWait(incompleteCallQuery, () => TestIsVisible(Query.FinishButton), postTimeout: TimeSpan.FromSeconds(0.5));
            TapAndWait(Query.FinishButton, () => TestIsVisible(Query.Loaded) && !TestIsVisible(Query.ErrorMessage));
        }

        public DashboardPage NavigateToDashboardPage() {
            return AppConvention.TapActivateAndWait<DashboardPage>(
                Application, Query.BackButton);
        }
    }
}