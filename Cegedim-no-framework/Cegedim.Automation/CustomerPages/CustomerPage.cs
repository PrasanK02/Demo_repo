 using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using Xamarin.Automation;
using NUnit.Framework;

namespace Cegedim.Automation {

    public class CustomerPage : CegedimPage { 

        private static class Query {
            internal static string SideNavigationColumn = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'navItems'";
            internal static string SideNavigationItem = SideNavigationColumn + "descendant view:'Dendrite.IPhone.Forms.CdlCellImageLink'";
            internal static string SuspensionStatus = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'suspensionStatus'";
            internal static string IdentityLink = SideNavigationItem + " descendant label marked:'Identity'";
            internal static string LocationLink = SideNavigationItem + " descendant label marked:'Location'";
            internal static string RatingsLink = SideNavigationItem + " descendant label marked:'Ratings'";
            internal static string CallButton = "view:'Dendrite.IPhone.Forms.ShinyButton' descendant UILabel marked:'Call'";
            internal static string ActivityHistoryLink = SideNavigationItem + " descendant UILabel marked:'Activity History'";
            internal static string ContainerScrollColumn = "view:'Dendrite.IPhone.Forms.UIScrollableContent' index:0";
            internal static string DismissPopup = "UIDimmingView id:'PopoverDismissRegion'";
        }

        internal CustomerPage(MITouch application, AppContainer container)
            : base(application, container) {
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.SideNavigationColumn); }
        }    

        public CustomerIdentityPage NavigateToCustomerIdentity() {
            return AppConvention.TapActivateAndWait<CustomerIdentityPage>(
                Application, Query.IdentityLink);
        }

        public CustomerLocationPage NavigateToCustomerLocation() {
            return AppConvention.TapActivateAndWait<CustomerLocationPage>(
                Application, Query.LocationLink);
        }

        public CustomerRatingsPage NavigateToCustomerRatings() {
            return AppConvention.TapActivateAndWait<CustomerRatingsPage>(
                Application, Query.RatingsLink);
        }

        public CustomerActivityHistoryPage NavigateToCustomerActivityHistory() {
            return AppConvention.TapActivateAndWait<CustomerActivityHistoryPage>(
                Application, Query.ActivityHistoryLink);
        }

        public CallPage NavigateToCallPage() {
            Wait(() => TestIsVisible(Query.CallButton));
            return AppConvention.TapActivateAndWait<CallPage>(
                Application, Query.CallButton);
        }

        public bool CustomerLevel {
            get { throw new NotImplementedException(); }
        }
        public bool ProductSuspention {
            get { throw new NotImplementedException(); }
        }
        public string Title {
            get { throw new NotImplementedException(); }
        }
        public string Speciality {
            get { throw new NotImplementedException(); }
        }

        public void CheckSection(string sectionName) {
            if (sectionName == "Suspension Status") {
                TestIsVisible(Query.SuspensionStatus);
            }
            else {
                Assert.Fail("Could not find the section name to check.");
            }
        }

        public void DismissPopup() {
            TapAndWait(Query.DismissPopup, () => !TestIsVisible(Query.DismissPopup), timeout: TimeSpan.FromSeconds(12));
        }

        private bool TestIsVisibleToScrollable(string queryString) {
            if (!TestIsVisible(queryString))
                return false;
            else {
                if (TestIsVisible(Query.ContainerScrollColumn)) {
                    // Check for intersection to scroll
                    var scrollRectangle = Calabash.Query(Query.ContainerScrollColumn).First().Rectangle;
                    var queryRectangle = Calabash.Query(queryString).First().Rectangle;
                    if (scrollRectangle.Contains(queryRectangle)) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    throw new Exception("No scrollable content to check for link");
                }
            }
        }

        private void SwipeDownUntilVisibleToScrollable(string rawSwipeOnQuery, string rawFinalQuery) {
            if (!TestIsVisible(rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var initialY = centerY + (height / 2.0 * 0.90);
            var finalY = centerY - (height / 2.0 * 0.90);

            AppConvention.Wait(Application, () => {
                if (TestIsVisibleToScrollable(rawFinalQuery)) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisibleToScrollable(rawFinalQuery);
                }
            });
        }

        private void SwipeUpUntilVisibleToScrollable(string rawSwipeOnQuery, string rawFinalQuery) {
            if (!TestIsVisible(rawSwipeOnQuery))
                return;

            var app = Calabash;
            var result = (Container.Descendants(rawSwipeOnQuery).First());

            var rectangle = result.Rectangle;

            var centerX = rectangle.X + rectangle.Width / 2;
            var height = result.Rectangle.Height;

            var centerY = rectangle.Y + rectangle.Height / 2;
            var finalY = centerY + (height / 2.0 * 0.90);
            var initialY = centerY - (height / 2.0 * 0.90);

            AppConvention.Wait(Application, () => {
                if (TestIsVisibleToScrollable(rawFinalQuery)) {
                    return true;
                } else {
                    app.Pan(new Point(centerX, (int)initialY), new Point(centerX, (int)finalY));
                    return TestIsVisibleToScrollable(rawFinalQuery);
                }
            });
        }

        public void MoreDetails(string expectedResults) {
            var moreDetailsLinkName = "More Details";
            var moreDetailsLinkQuery = String.Format("label marked:'{0}' index:0", moreDetailsLinkName);
            var lessDetailsLinkName = "Fewer Details";
            var lessDetailsLinkQuery = String.Format("label marked:'{0}' index:0", lessDetailsLinkName);
            var expectedQuery = String.Format("label marked:'{0}' index:0", expectedResults);
            // Flick to the link if its not immediately visible 
            // Added test is visible to scrollable since content appears visible even 
            // when you can't actually tap it
            // if not on the scrollable contact ui then just try to touch it
            // Here checking if More Details is already open
            try {
                if (!TestIsVisibleToScrollable(moreDetailsLinkQuery))
                    try {
                    SwipeDownUntilVisibleToScrollable(Query.ContainerScrollColumn, moreDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                } catch {
                    SwipeUpUntilVisibleToScrollable(Query.ContainerScrollColumn, moreDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                }
                TapAndWait(moreDetailsLinkQuery, () => TestIsVisible(expectedQuery), timeout: TimeSpan.FromSeconds(15));
            } catch {
                if (!TestIsVisibleToScrollable(lessDetailsLinkQuery))
                    try {
                    SwipeDownUntilVisibleToScrollable(Query.ContainerScrollColumn, lessDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                } catch {
                    SwipeUpUntilVisibleToScrollable(Query.ContainerScrollColumn, lessDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                }
                Wait(() => TestIsVisible(expectedQuery));
            }
        }

        public void LessDetails(string expectedResults) {
            var moreDetailsLinkName = "More Details";
            var moreDetailsLinkQuery = String.Format("label marked:'{0}' index:0", moreDetailsLinkName);
            var lessDetailsLinkName = "Fewer Details";
            var lessDetailsLinkQuery = String.Format("label marked:'{0}' index:0", lessDetailsLinkName);
            var expectedQuery = String.Format("label marked:'{0}' index:0", expectedResults);
            // Flick to the link if its not immediately visible 
            // Added test is visible to scrollable since content appears visible even 
            // when you can't actually tap it
            // if not on the scrollable contact ui then just try to touch it
            // Here checking if More Details is already open
            try {
                if (!TestIsVisibleToScrollable(lessDetailsLinkQuery))
                    try {
                    SwipeDownUntilVisibleToScrollable(Query.ContainerScrollColumn, lessDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                } catch {
                    SwipeUpUntilVisibleToScrollable(Query.ContainerScrollColumn, lessDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                }
                TapAndWait(lessDetailsLinkQuery, () => TestIsVisible(expectedQuery), timeout: TimeSpan.FromSeconds(15));
            } catch {
                if (!TestIsVisibleToScrollable(moreDetailsLinkQuery))
                    try {
                    SwipeDownUntilVisibleToScrollable(Query.ContainerScrollColumn, moreDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                } catch {
                    SwipeUpUntilVisibleToScrollable(Query.ContainerScrollColumn, moreDetailsLinkQuery);
                    Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                }
                Wait(() => TestIsVisible(expectedQuery));
            }
        }

        public void TapLink(string linkName, string expectedResults, bool? scrollableLink = true) {
            var linkQuery = String.Format("label marked:'{0}' index:0", linkName);
            var expectedQuery = String.Format("label marked:'{0}' index:0", expectedResults);
            // Flick to the link if its not immediately visible 
            // Added test is visible to scrollable since content appears visible even 
            // when you can't actually tap it
            // if not on the scrollable contact ui then just try to touch it
            if ((bool)scrollableLink) {
                if (!TestIsVisibleToScrollable(linkQuery))
                    try {
                        SwipeDownUntilVisibleToScrollable(Query.ContainerScrollColumn, linkQuery);
                        Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                    } catch {
                        SwipeUpUntilVisibleToScrollable(Query.ContainerScrollColumn, linkQuery);
                        Thread.Sleep(TimeSpan.FromSeconds(1)); // post timeout
                    }
                TapAndWait(linkQuery, () => TestIsVisible(expectedQuery), timeout: TimeSpan.FromSeconds(15));
            } else {
                if (TestIsVisible(linkQuery))
                    TapAndWait(linkQuery, () => TestIsVisible(expectedQuery));
            }
        }
    }
}