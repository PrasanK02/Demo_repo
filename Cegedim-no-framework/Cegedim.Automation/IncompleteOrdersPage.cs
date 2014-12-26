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
    public class IncompleteOrdersPage : CegedimPage {
        private static class Query {
            internal static string Loaded = "UINavigationBar {accessibilityIdentifier contains 'Incomplete Orders'}";
            internal static string TableViewCell = "UITableViewCell";
        }

        internal IncompleteOrdersPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        public EditOrderEntryPage EditOrderAtRandom() {
            int count = Calabash.Query(Query.TableViewCell).Count();
            Random random = new Random();
            int sampleVal = random.Next(0, count - 1);
            string orderQuery = string.Format("{0} index:{1}", Query.TableViewCell, sampleVal);
            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
            return AppConvention.TapActivateAndWait<EditOrderEntryPage>(Application, orderQuery);
        }
    }
}