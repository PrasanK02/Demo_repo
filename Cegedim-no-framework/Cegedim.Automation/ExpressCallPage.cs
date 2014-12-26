using System;
using Xamarin.Automation;

namespace Cegedim.Automation {

    // TODO: Make express call page when I have the appropriate app to develop this
    public class ExpressCallPage : CegedimPage {

        private static class Query {
            //internal static string Loaded = "view id:'navBar'";
        }

        internal ExpressCallPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { 
                //return TestIsVisible(Query.Loaded); 
                return true;
            }
        }
    }
}