using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    public class Presentations {
        MITouch m_miTouch;

        [Test()]
        public void OpenPresentations() {
            var presentationsPage = Background();
            var presentationPage = presentationsPage.NavigateToPresentationPage();
            m_miTouch.Screenshot("I open the first presentation");

            presentationPage.Cancel();
            m_miTouch.Screenshot("I cancel the presentation and see the presentations home page");
        }

        [Test]
        public void PreviewPresentation() {
            var presentationsPage = Background();
            var presentationPage = presentationsPage.NavigateToPresentationPage();
            m_miTouch.Screenshot("I open the first presentation");

            presentationPage.Preview();
            m_miTouch.Screenshot("I touch the preview button and see the preview mode");

            presentationPage.Cancel();
            m_miTouch.Screenshot("I cancel the presentation and see the presentations home page");
        }

        [Test]
        public void PlayPresentation() {
            var presentationsPage = Background();
            var presentationPage = presentationsPage.NavigateToPresentationPage(2);
            m_miTouch.Screenshot("I open the last presentation");

            presentationPage.Play();
            m_miTouch.Screenshot("I touch the play button");

            presentationPage.Cancel();
            m_miTouch.Screenshot("I cancel the presentation and see the presentations home page");
        }

        [Test]
        public void SwipeThroughPresentation() {
            var presentationsPage = Background();
            var presentationPage = presentationsPage.NavigateToPresentationPage(1);
            m_miTouch.Screenshot("I open the second presentation");

            presentationPage.Play();
            m_miTouch.Screenshot("I touch the play button");

            presentationPage.SwipeSlide();
            m_miTouch.Screenshot("I swipe left once");

            presentationPage.SwipeSlide();
            m_miTouch.Screenshot("I swipe left twice");

            presentationPage.SwipeSlide();
            m_miTouch.Screenshot("I swipe left a third time");

            presentationPage.SwipeSlide();
            m_miTouch.Screenshot("I swipe left a fourth time");

            presentationPage.SwipeSlide("right");
            m_miTouch.Screenshot("I swipe right once");

            presentationPage.Cancel();
            m_miTouch.Screenshot("I cancel the presentation and see the presentations home page");
        }

        [Test]
        public void UseJavascriptToInteractWithPresentation() {
            var presentationsPage = Background();
            var presentationPage = presentationsPage.NavigateToPresentationPage(1);
            m_miTouch.Screenshot("I open the second presentation");

            presentationPage.Play();
            m_miTouch.Screenshot("I touch the play button");

            presentationPage.ConfirmIrisSlide();
            m_miTouch.Screenshot("I see the first Iris slide");

            presentationPage.NavigateToLeCoeur();
            m_miTouch.Screenshot("I touch the Le Coeur menu item and see the correct slide");

            presentationPage.PlayLeCoeurVideo();
            m_miTouch.Screenshot("I start watching the Le Coeur video");

            presentationPage.PauseVideo(10);
            m_miTouch.Screenshot("After 10 seconds I pause the video");

            presentationPage.SwipeSlide("right");
            presentationPage.ConfirmIrisSlide();
            m_miTouch.Screenshot("I swipe back to the Iris home view");

        }

        public PresentationsPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            m_miTouch.Screenshot("I'm on the dashboard page");

            var presentationsPage = dashboard.NavigateToPresentationsPage();
            m_miTouch.Screenshot("I see the presentations home page");

            return presentationsPage;
        }
    }
}

