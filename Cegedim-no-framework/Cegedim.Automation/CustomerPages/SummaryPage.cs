using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using Xamarin.Automation;
using NUnit.Framework;

namespace Cegedim.Automation {

    public class SummaryPage : CustomerPage { 

        private static class Query {
            internal static string Loaded = "view {accessibilityIdentifier BEGINSWITH 'PAGE:' AND accessibilityIdentifier ENDSWITH '/summary.cdl'}";
            internal static string MoreActions = "view:'CdlView' index:0 view:'UIImageView' {accessibilityIdentifier endswith 'moreactionsblack.png'} index:0";
            internal static string PersonalInformationTab = "UILabel marked:'Personal' index:0";
            internal static string OfficeInformationTab = "UILabel marked:'Office' index:0";
            internal static string RequiredPlaceholders = "UILabel marked:'Required' index:0";
            internal static string AddInformationButton = "view:'_UIPopoverView' descendant UIButton marked:'Add'";
            internal static string InformationPopover = "view:'_UIPopoverView'";
            internal static string InformationPopoverOffice = InformationPopover + " descendant * marked:'Office'";
            internal static string InformationPopoverPersonal = InformationPopover + " descendant * marked:'Personal'";
            internal static string InformationPopoverDone = "view:'_UIPopoverView' descendant UINavigationButton marked:'Done'";
            internal static string PhoneNumber = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'phone'";
            internal static string ContactRows = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'show_personal_officeControls' descendant UITableViewCellContentView";
            internal static string PhoneActionButton = "view:'Dendrite.IPhone.Forms.GridControl.CdlGridColumn' id:'phoneAction'";
            internal static string DeleteContactInformationButton = "UIAlertButton marked:'Delete Contact Info'";
            internal static string DeleteShinyButton = "view:'Dendrite.IPhone.Forms.ShinyButton' descendant UILabel marked:'Delete' index:0";
            internal static string SuspensionStatus = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'suspensionStatus'";
            internal static string Back = "view marked:'Back'";
        }

        private Lazy<ModifyPopover> m_informationPopover;
        private int m_phoneNumbers = 0;

        internal SummaryPage(MITouch application, AppContainer container)
            : base(application, container) {
            m_informationPopover = new Lazy<ModifyPopover>(() => ModifyPopoverFactory(Query.InformationPopover));
        }

        public override bool IsLoaded {
            get { return TestIsVisible(Query.Loaded); }
        }

        private ModifyPopover ModifyPopoverFactory(string query) {
            return new ModifyPopover(Application, this, Container.GetDescendant(query));
        }

        // TODO: Change the information input for AddOfficeInformation
        public void AddOfficeInformation() {
            TapAndWait(Query.MoreActions, () => TestIsVisible(Query.PersonalInformationTab));
            TapAndWait(Query.AddInformationButton, () => TestIsVisible(Query.RequiredPlaceholders));
            // Post timeout for touching the textfields etc...
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            // Select Pager in Phone Type Dropdown selector
            m_informationPopover.Value.AddDropDown(0, "Phone Type", "Home Phone");
            // Enter phone number in the first normal text field
            m_informationPopover.Value.AddTextField(0, "5555555555");
            m_informationPopover.Value.AddDropDown(1, "Category", "Office");
            TapAndWait(Query.InformationPopoverDone, () => !TestIsVisible(Query.InformationPopover));

        }

        public void CheckContactInformation() {
            // TODO: Implement the check of the contact information (wasn't implemented in ruby)
        }

        // Deletes a Phone number in the Office section of the contact pop over
        public void DeleteOfficeInformation() {
            var contactRowQuery = Query.ContactRows + " index:0";
            var phoneActionButton = Query.PhoneActionButton + " index:0";
            TapAndWait(Query.MoreActions, () => TestIsVisible(Query.PersonalInformationTab));
            TapAndWait(Query.InformationPopoverOffice, () => TestIsVisible(contactRowQuery));
            m_phoneNumbers = Container.Descendants(Query.PhoneActionButton).Count();
            TapAndWait(phoneActionButton, () => TestIsVisible(Query.DeleteShinyButton));
            // Post timeout for clicking the shiny button
            Thread.Sleep(TimeSpan.FromSeconds(1));
            TapAndWait(Query.DeleteShinyButton, () => TestIsVisible(Query.DeleteContactInformationButton));
            TapAndWait(Query.DeleteContactInformationButton, () => !TestIsVisible(Query.InformationPopover));
        }

        // Verifies the count of phoneActionButtons for the Office has changed aka one of them got deleted
        public void VerifyDeletedInformation() {
            TapAndWait(Query.MoreActions, () => TestIsVisible(Query.PersonalInformationTab));
            // TODO: check if informationpopoveroffice button is enabled not just there
            TapAndWait(Query.InformationPopoverOffice, () => TestIsVisible(Query.InformationPopoverOffice));
            var currentPhoneNumbers = Container.Descendants(Query.PhoneActionButton).Count();
            if (currentPhoneNumbers == m_phoneNumbers) {
                Assert.Fail("No phone numbers deleted.");
            }
        }

        public void Back() {
            TapAndWait(Query.Back, () => !TestIsVisible(Query.Loaded));
        }
    }
}