using System;
using System.Linq;
using System.Drawing;
using Xamarin.Automation;
using NUnit.Framework;
using System.Threading;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Utils;

namespace Cegedim.Automation {

    public class PresentationsPage : CegedimPage { 

        private static class Query {
            internal static string Loaded = "view id:'PAGE:advanceddetailing/advpresentationssearch.cdl'";
            internal static string Presentation = "view:'MI.CustomControls.AdvPresentationListControl' descendant buttonLabel";
        }

        internal PresentationsPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }  

        public PresentationPage NavigateToPresentationPage(int index = 0) {
            string presentationQuery = string.Format("{0} index:{1}", Query.Presentation, index);
            return AppConvention.TapActivateAndWait<PresentationPage>(Application, presentationQuery);
        }
    }

    public class PresentationPage : CegedimPage { 

        private static class Query {
            internal static string Loaded = "view id:'PAGE:advanceddetailing/advpresentationviewer.cdl'";
            //internal static string Cancel = "view marked:'Cancel'";
            internal static string Cancel = "view:'Dendrite.IPhone.Forms.DrteWebView' css:'a#closebutton'";
            internal static string CancelPreview = "view:'Dendrite.IPhone.Forms.DrteWebView' css:'a#cancelButton'";
            internal static string Preview = "view:'Dendrite.IPhone.Forms.DrteWebView' css:'a#previewbutton'";
            internal static string Play = "view:'Dendrite.IPhone.Forms.DrteWebView' css:'a#playbutton'";
            internal static string MarkedPreview = "view marked:'Preview'";
            internal static string Delete = "view marked:'Delete'";
            internal static string Window = "UIWindow";
            internal static string VideoPlayButton = "button marked:'Play'";
            internal static string VideoPauseButton = "button marked:'Pause'";
            internal static string VideoView = "movieView marked:'Video'";
        }

        internal PresentationPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }  

        public PresentationsPage Cancel() {
            // TODO: Here I have to expose the raw iosApp because the midpoint calculated by Xamarin Automation
            // is incorrect since CenterX and CenterY are different than the midpoints of the Rect returned by 
            // the calabash server
            var windowRectangle = Calabash.Query("UIWindow").First().Rectangle;
            var x = windowRectangle.X + windowRectangle.Width / 2.0;
            var y = windowRectangle.Y + windowRectangle.Height / 40;
            var topBarPosition = new Point((int)x, (int)y);
            if (IsPreviewing()) {
                Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
                Calabash.Tap(topBarPosition);
                Wait(() => TestIsVisible(Query.CancelPreview));
                MITouch.iosApp.Tap(c => c.Raw(Query.CancelPreview));
                return new PresentationsPage(Application, Container);
            }
            else if (IsPlaying()) {
                Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
                Calabash.Tap(topBarPosition);
                Wait(() => TestIsVisible(Query.CancelPreview));
                MITouch.iosApp.Tap(c => c.Raw(Query.CancelPreview));
                Wait(() => TestIsVisible(Query.Delete));
                MITouch.iosApp.Tap(c => c.Raw(Query.Delete));
                Wait(() => !TestIsVisible(Query.Delete));
                return new PresentationsPage(Application, Container);
            }
            else {
                MITouch.iosApp.Tap(c => c.Raw(Query.Cancel));
                return new PresentationsPage(Application, Container);
            }
        }

        public string JSForIFrameElement(string frameId, string elementId) {
            string jsCommand = string.Format("document.getElementById('{0}').contentDocument.getElementById('{1}').toString();", 
                frameId, elementId);
            return jsCommand;
        }

        public string JSForIFrameElementText(string frameId, string elementId) {
            string jsCommand = string.Format("document.getElementById('{0}').contentDocument.getElementById('{1}').innerText;",
                frameId, elementId);
            return jsCommand;
        }

        public string TextFromIFrameElementText(string jsCommand) {
            string results = MITouch.iosApp.Query(c => c.Class("webView").InvokeJS(jsCommand)).First();
            return results;
        }

        public void TouchIFrameElement(string jsCommand) {
            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
            string queryFromIFrameElement = MITouch.iosApp.Query(c => c.Class("webView").InvokeJS(jsCommand)).First();
            string jsTouchCommand = string.Format("eval(\"{0}\")", queryFromIFrameElement);
            if (jsTouchCommand.Contains("javascript"))
                MITouch.iosApp.Query(c => c.Class("webView").InvokeJS(jsTouchCommand));
            else {
                MITouch.iosApp.Tap(c => c.Class("webView"));
            }
        }

        public bool IsPreviewing() {
            return Calabash.Query(Query.MarkedPreview).Count() > 1;
        }

        public bool IsPlaying() {
            Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
            try {
                Wait(() => TestIsVisible(Query.Cancel), timeout: TimeSpan.FromSeconds(2));
            } catch {
                return true;
            }
            return !TestIsVisible(Query.Play);
        }

        public void Play() {
            Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
            MITouch.iosApp.Tap(c => c.Raw(Query.Play));
            Wait(() => !TestIsVisible(Query.Play), postTimeout: TimeSpan.FromSeconds(0.5));
        }

        public void Preview() {
            MITouch.iosApp.Tap(c => c.Raw(Query.Preview));
            // Check if previewing
            Wait(() => Calabash.Query(Query.MarkedPreview).Count() > 1);
        }

        public void SwipeSlide(string direction = "Left") {
            var windowRect = Calabash.Query(Query.Window).First().Rectangle;
            var centerY = windowRect.Y + windowRect.Height / 2.0;
            var leftX = windowRect.X + windowRect.Width / 11;
            var rightX = windowRect.X + windowRect.Width / 10.0 * 9.5;
            Point leftPoint = new Point((int)leftX, (int)centerY);
            Point rightPoint = new Point((int)rightX, (int)centerY);
            if (direction == "Left")
                Calabash.Pan(rightPoint, leftPoint);
            else if (direction == "Right" || direction == "right")
                Calabash.Pan(leftPoint, rightPoint);
            else
                Assert.Fail("No swipe direction entered");
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
        }

        public void ConfirmIrisSlide() {
            string expectedResult = "Bonjour Docteur,\nquelle rubrique voulez-vous consulter ?";
            string jsCommand = JSForIFrameElementText("frame_0_0", "accroche");
            string result = TextFromIFrameElementText(jsCommand);
            if (expectedResult != result)
                Assert.Fail("Not on the Iris home slide");
        }

        public void ConfirmLeCoeurSlide() {
            string expectedResult = "Le cœur est un organe creux et musculaire qui assure";
            string jsCommand = JSForIFrameElementText("frame_0_1", "textblock");
            string result = TextFromIFrameElementText(jsCommand);
            if (expectedResult.Contains(result))
                Assert.Fail("Not on the Le Coeur slide");
        }

        public void NavigateToLeCoeur() {
            string jsCommand = JSForIFrameElement("frame_0_0", "bt_sequence2");
            TouchIFrameElement(jsCommand);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            ConfirmLeCoeurSlide();
        }

        public void PlayLeCoeurVideo() {
            ConfirmLeCoeurSlide();
            string jsCommandToTouchVideo = JSForIFrameElement("frame_0_1", "bt_1");
            TouchIFrameElement(jsCommandToTouchVideo);
            Wait(() => TestIsVisible(Query.VideoPlayButton), postTimeout: TimeSpan.FromSeconds(4));
            TapAndWait(Query.VideoPlayButton, () => TestIsVisible(Query.VideoPauseButton));
        }

        public void PauseVideo(int numberOfSeconds) {
            Thread.Sleep(TimeSpan.FromSeconds(numberOfSeconds));
            if (!TestIsVisible(Query.VideoPauseButton)) {
                MITouch.iosApp.Tap(c => c.Raw(Query.VideoView));
                Wait(() => TestIsVisible(Query.VideoPauseButton), postTimeout: TimeSpan.FromSeconds(0.6));
            }
                //TapAndWait(Query.VideoView, () => TestIsVisible(Query.VideoPauseButton), postTimeout: TimeSpan.FromSeconds(0.5));
            //TapAndWait(Query.VideoPauseButton, () => !TestIsVisible(Query.VideoPauseButton));
            MITouch.iosApp.Tap(c => c.Raw(Query.VideoPauseButton));
            Wait(() => !TestIsVisible(Query.VideoPauseButton));
        }
    }
}