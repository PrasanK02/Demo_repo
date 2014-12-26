using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    public class Search {
        MITouch m_miTouch;
        Uri m_uri;

        [Test()]
        public void CustomerSearch() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.SearchFor("o");
            m_miTouch.Screenshot("I search for o");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByDepartments = true;
            m_miTouch.Screenshot("I select Departments");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByDepartments = false;
            searchPage.FilterByPharmacies = true;
            m_miTouch.Screenshot("I select Pharmacies");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByProfessionals = true;
            m_miTouch.Screenshot("I also select medical professionals");

            searchPage.FilterByProfessionals = false;
            searchPage.FilterByPharmacies = false;
            searchPage.FilterByIndividuals = true;
            m_miTouch.Screenshot("I select all individuals");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByOrganizations = true;
            m_miTouch.Screenshot("I also select all organizations");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByOrganizations = false;
            m_miTouch.Screenshot("I also unselect all organizations");

            searchPage.ClearSearchField();
            m_miTouch.Screenshot("I clear the search field");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByCountryState(true);
            m_miTouch.Screenshot("I set the country and state filter");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByCity(true);
            m_miTouch.Screenshot("I set the city filter");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByPostalCode(true);
            m_miTouch.Screenshot("I set the postal code filter");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByMainInstitution("A", true);
            m_miTouch.Screenshot("I set the main institution filter");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");

            searchPage.FilterByAffiliationType("Department", true);
            m_miTouch.Screenshot("I set the affiliation type filter");

            searchPage.ConfirmResults();
            m_miTouch.Screenshot("I confirm the count and the first customer is valid");
        }

        [Test]
        public void ViewCustomerCardTest() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            var summaryPage = searchPage.TapCustomerRow(0);
            m_miTouch.Screenshot("I tap the first customer row");

            summaryPage.AddOfficeInformation();
            m_miTouch.Screenshot("I add the contact information");

            summaryPage.CheckContactInformation();
            m_miTouch.Screenshot("I check the contact information");

            summaryPage.DeleteOfficeInformation();
            m_miTouch.Screenshot("I delete the contact information");

            summaryPage.VerifyDeletedInformation();
            m_miTouch.Screenshot("I verify the contact was deleted");
        }

        [Test]
        public void ViewCustomerCardIdentity() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            var summaryPage = searchPage.TapCustomerRow(0);
            m_miTouch.Screenshot("I tap the first customer row");

            var customerIdentityPage = summaryPage.NavigateToCustomerIdentity();
            m_miTouch.Screenshot("I tap the identity customer tab");

            customerIdentityPage.CheckSection("Suspension Status");
            m_miTouch.Screenshot("I see the Suspension Status displayed");

            //customerIdentityPage.TapLink("More Details", "Contact Restrictions");
            customerIdentityPage.MoreDetails("Contact Restrictions");
            m_miTouch.Screenshot("I tap the More Details link and see the Contact Restrictions");

            //customerIdentityPage.TapLink("Fewer Details", "More Details");
            customerIdentityPage.LessDetails("More Details");
            m_miTouch.Screenshot("I tap the Fewer Details link and see the More Details display");

            customerIdentityPage.TapLink("Routes", "Select Route");
            m_miTouch.Screenshot("I tap the Routes link and the Select Route displays");

            //customerIdentityPage.AddedRoute = false;
            customerIdentityPage.TexasRoute = true;
            customerIdentityPage.TapDone();
            m_miTouch.Screenshot("I select the Texas route and select done");

            customerIdentityPage.TapLink("Routes", "Select Route");
            m_miTouch.Screenshot("I tap the Routes link and the Select Route displays");

            customerIdentityPage.TexasRoute = false;
            customerIdentityPage.TapDone();
            m_miTouch.Screenshot("I select the Texas route again and it does not display");

            customerIdentityPage.TapLink("Sampling", "State License #", false);
            m_miTouch.Screenshot("I tap the sampling popup and see the state license");

            customerIdentityPage.DismissSampling();
            m_miTouch.Screenshot("I dismiss the pop up and do not see the state license");

            customerIdentityPage.TapLink("General", "Last Name");
            customerIdentityPage.FillInitialInformation();
            customerIdentityPage.TapDone();
            m_miTouch.Screenshot("I updated the general customer identity information");
        }

        [Test]
        public void ViewCustomerCardLocation() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            var summaryPage = searchPage.TapCustomerRow(0);
            m_miTouch.Screenshot("I tap the first customer row");

            var customerLocationPage = summaryPage.NavigateToCustomerLocation();
            customerLocationPage.Verify("Business Hours and Best Times");
            m_miTouch.Screenshot("I navigate to the customer location page and see business hours and best times");

            customerLocationPage.TapLink("Legend", "Best Times");
            m_miTouch.Screenshot("I tap the Legend popup and see Best Times");

            customerLocationPage.DismissPopup();
            m_miTouch.Screenshot("I dismiss the popup and no longer see the popup");
        }

        [Test]
        public void ViewCustomerCardRatings() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            var summaryPage = searchPage.TapCustomerRow(0);
            m_miTouch.Screenshot("I tap the first customer row");

            var customerRatingsPage = summaryPage.NavigateToCustomerRatings();
            customerRatingsPage.Verify("Therapeutic Class Ratings");
            m_miTouch.Screenshot("I navigate to the customer ratings page and verify the therapeutic class");

            customerRatingsPage.TapLink("HC", "Therapeutic Class");
            m_miTouch.Screenshot("I tap the Therapeutic Class HC popup and see Therapeutic class popup");

            customerRatingsPage.DismissPopup();
            customerRatingsPage.TherapeuticClass = "OAD";
            m_miTouch.Screenshot("I tap the Therapeutic class OAD");

            customerRatingsPage.VerifyTherapeuticClass("OAD");
            m_miTouch.Screenshot("I checked that OAD displays in the scrollable content");

            customerRatingsPage.TherapeuticClass = "HC";
            m_miTouch.Screenshot("I tap the Therapeutic class HC");

            customerRatingsPage.VerifyTherapeuticClass("HC");
            m_miTouch.Screenshot("I checked that HC displays in the scrollable content");

            customerRatingsPage.TapLink("Show All", "Show Mine");
            m_miTouch.Screenshot("I tap Show All and I see Show Mine display");

            customerRatingsPage.TapLink("Show Mine", "Show All");
            m_miTouch.Screenshot("I tap Show Mine and I see Show All display");
        }

        [Test]
        public void ViewCustomerCard() {
            var searchPage = Background();
            m_miTouch.Screenshot("I am on the search page");

            searchPage.SearchFor("a");
            m_miTouch.Screenshot("I search for a");

            var summaryPage = searchPage.TapCustomerRow(0);
            m_miTouch.Screenshot("I tap the first customer row");

            var customerActivityHistoryPage = summaryPage.NavigateToCustomerActivityHistory();
            customerActivityHistoryPage.Verify("Last Call");
            m_miTouch.Screenshot("I navigate to the customer activity history page and see the last call display");

            customerActivityHistoryPage.TapLink("Show All", "Show Mine");
            m_miTouch.Screenshot("I touch Show All and then Show Mine displays");

            customerActivityHistoryPage.TapLink("Show Mine", "Show All");
            m_miTouch.Screenshot("I touch Show Mine and then Show All displays");

            customerActivityHistoryPage.TapAndVerify("All Addresses");
            m_miTouch.Screenshot("I tap All Address and the button is active");

            customerActivityHistoryPage.TapAndVerify("Current Address");
            m_miTouch.Screenshot("I tap Current Address and the button is active");

            customerActivityHistoryPage.NavigateBack();
            m_miTouch.Screenshot("I tap the go back button to the customer search page");
        }

        public SearchPage Background() {
            m_miTouch = Globals.App;
            var dashboard = Globals.QuickSetUp();
            var searchPage = dashboard.NavigateToSearchPage();
            return searchPage;
        }
    }
}

