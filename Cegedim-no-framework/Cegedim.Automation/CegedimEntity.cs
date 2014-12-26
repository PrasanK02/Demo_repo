using System;
using System.Linq;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using System.Drawing;

namespace Cegedim.Automation
{
    public abstract class CegedimEntity : AppEntity { 

        private static class Query {
            internal static string Keyboard = "view:'UIKeyboardAutomatic'";
            internal static string ScrollableColumn = "view:'Dendrite.IPhone.Forms.UIScrollableContent' index:0";
        }

        public CegedimEntity(MITouch application, CegedimEntity parent, AppContainer container)
            : base(application, parent, container) {
        }

        public CalabashServer Calabash {
            get { return (CalabashServer)Application.Server; }
        }

        public new MITouch Application {
            get { return (MITouch)base.Application; }
        }

        // Override Automation SetField in this case because of the UITextField issue
        public void SetField(string query, string value, TimeSpan? timeout = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(15); // Apple keyboard seems slow
            TapAndWait(query, () => IsKeyboardVisible(), timeout: timeout);
            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
            string script = string.Format("uia.keyboard().typeString('{0}')", value);
          //  MITouch.iosApp.InvokeUia(script);
            //AppConvention.SetField(Container, query, value);
           // MITouch.iosApp.InvokeUia(script);
            MITouch.iosApp.EnterText(c => c.Raw(query), value);
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
                if (Environment.GetEnvironmentVariable("XTC_DEVICE_OS").Contains("7.")) {
                    MITouch.iosApp.EnterText(c => c.Raw(query), value);
                }
                else {
                    MITouch.iosApp.InvokeUia(script);
                }
            } else {
                MITouch.iosApp.EnterText(c => c.Raw(query), value);
            }
        }
           
        protected bool TestIsVisible(params string[] queries) {
            return AppConvention.TestIsVisible(Container, queries);
        }

        protected void Wait(Func<bool> predicate, AppTimeout timeout = null, TimeSpan? postTimeout = null) {
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.2);
            AppConvention.Wait(Application, predicate, timeout: timeout);
            Thread.Sleep((TimeSpan)postTimeout);
        }

        // Check if queries are visible inside the queryContainer
        protected bool TestIsVisibleToElement(string queryContainer, string queryString) {
            if(TestIsVisible(queryContainer) && TestIsVisible(queryString)) {
                var containerRectangle = Calabash.Query(queryContainer).First().Rectangle;
                var queryRectangle = Calabash.Query(queryString).First().Rectangle;
                return containerRectangle.Contains(queryRectangle);
            }
            return false;
        }

        protected bool IsKeyboardVisible() {
            // Test Is Visible will issue the query with respect to the container
            // This will break IsKeyboardVisible since its not the child of a lot of containers
            // return TestIsVisible(Query.Keyboard);
            return Calabash.Query(Query.Keyboard).Count() > 0;
        }

        // Quick hacky solution to hide keyboard now
        protected void HideKeyboard() {
            if (IsKeyboardVisible()) {
//                int keysIndex = Container.Descendants("view:'UIKBKeyView'").Count();
//                keysIndex -= 1;
//                string query = string.Format("UIKBKeyView index:'{0}'", keysIndex);
                //TapAndWait(query, () => !IsKeyboardVisible());
                string script = "uia.keyboard().buttons()['Hide keyboard'].tap()";
                MITouch.iosApp.InvokeUia(script);
                //Calabash.InvokeUia(script);
            }
        }

        protected void TapAndWait(
            string query,
            Func<bool> predicate, 
            AppTimeout timeout = null, TimeSpan? postTimeout = null) {

            timeout = timeout ?? Application.DefaultTimeout();
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.2);
            AppConvention.TapAndWait(
                Application, Container, query, predicate, timeout);
            Thread.Sleep(((TimeSpan)postTimeout)); // step pause
        }

        protected string GetField(string query) {
            return AppConvention.GetField(Container, query);
        }

        public void AssertElementExists(string queryString) {
            try {
                //SwipeDownUntil(Query.ScrollableColumn, queryString);
                SwipeDownUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, queryString);
            } catch {
                //SwipeUpUntil(Query.ScrollableColumn, queryString);
                SwipeUpUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, queryString);
            }
        }

        //protected void SetField(string query, string value) {
        //    UIConvention.SetField(Container, query, value);
        //}

        protected void SwipeDownUntil(string rawSwipeOnQuery, string rawFinalQuery, TimeSpan? timeout = null, 
            TimeSpan? postTimeout = null, double? ratio = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
            ratio = ratio ?? 0.96;
            if (!TestIsVisible(rawSwipeOnQuery))
                return;
            
            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2; 
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var initialY = centerY + (height / 2.0 * ratio);
            var finalY = centerY - (height / 2.0 * ratio);
            Func<bool> scrollTo = () => {
                if (TestIsVisible(rawFinalQuery)) {
                    return true;
                }
                else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisible(rawFinalQuery);
                }
            };
            AppConvention.Wait(Application, scrollTo, timeout: timeout, pollingIntervalMilliseconds: 60);
            Thread.Sleep((TimeSpan)postTimeout);
        }

        protected void SwipeUpUntil(string rawSwipeOnQuery, string rawFinalQuery, TimeSpan? timeout = null, 
            TimeSpan? postTimeout = null, double? ratio = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(8);
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
            ratio = ratio ?? 0.96;
            if (!TestIsVisible(rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var finalY = centerY + (height / 2.0 * ratio);
            var initialY = centerY - (height / 2.0 * ratio);
            Func<bool> scrollTo = () => {
                if (TestIsVisible(rawFinalQuery)) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisible(rawFinalQuery);
                }
            };
            AppConvention.Wait(Application, scrollTo, timeout: timeout);
            Thread.Sleep((TimeSpan)postTimeout);
        }

        protected void SwipeDownUntil(string rawSwipeOnQuery, Func<bool> predicate, TimeSpan? timeout = null, 
            TimeSpan? postTimeout = null, double? ratio = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(8);
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
            ratio = ratio ?? 0.96;
            if (!TestIsVisible(rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var initialY = centerY + (height / 2.0 * ratio);
            var finalY = centerY - (height / 2.0 * ratio);
            Func<bool> scrollTo = () => {
                if (predicate.Invoke()) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return predicate.Invoke();
                }
            };
            AppConvention.Wait(Application, scrollTo, timeout: timeout);
            Thread.Sleep((TimeSpan)postTimeout);
        }

        protected void SwipeDownUntilVisibleToElement(string queryContainer, string rawSwipeOnQuery, string rawFinalQuery,
            TimeSpan? timeout = null, TimeSpan? postTimeout = null, double? ratio = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(8);
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
            ratio = ratio ?? 0.96;
            if (!TestIsVisibleToElement(queryContainer, rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var initialY = centerY + (height / 2.0 * ratio);
            var finalY = centerY - (height / 2.0 * ratio);
            Func<bool> scrollTo = () => {
                if (TestIsVisibleToElement(queryContainer, rawFinalQuery)) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisibleToElement(queryContainer, rawFinalQuery);
                }
            };
            AppConvention.Wait(Application, scrollTo, timeout: timeout);
            Thread.Sleep((TimeSpan)postTimeout);
        }

        protected void SwipeUpUntilVisibleToElement(string queryContainer, string rawSwipeOnQuery, string rawFinalQuery,
            TimeSpan? timeout = null, TimeSpan? postTimeout = null, double? ratio = null) {
            timeout = timeout ?? TimeSpan.FromSeconds(8);
            postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
            ratio = ratio ?? 0.96;
            if (!TestIsVisibleToElement(queryContainer, rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var finalY = centerY + (height / 2.0 * ratio);
            var initialY = centerY - (height / 2.0 * ratio);
            Func<bool> scrollTo = () => {
                if (TestIsVisibleToElement(queryContainer, rawFinalQuery)) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisibleToElement(queryContainer, rawFinalQuery);
                }
            };
            AppConvention.Wait(Application, scrollTo, timeout: timeout);
            Thread.Sleep((TimeSpan)postTimeout);
        }
    }

    public abstract class CegedimSubPage : CegedimEntity {

        public CegedimSubPage(MITouch application, CegedimEntity parent, AppElement element)
            : base(application, parent, element) {
        }

        public AppElement Element {
            get { return (AppElement)Container; }
        }
    }
    public abstract class CegedimPage : CegedimEntity {

        public CegedimPage(MITouch application, AppContainer container)
            : base(application, null, container) {
        }   
    }
}
