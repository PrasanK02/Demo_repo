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
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace Cegedim.Automation {
	// TODO: Refactor CallPage to make more object oriented with the pop overs
	public class NewCallPage  {
		private IApp app;
		public static JArray CallCustomers;
		public static JArray SampleRequestProduct;
		public static JArray SampleProduct;
		private string m_callPurpose = "Not set";
		private string m_accompaniedBy;
		private int m_documentId;
		private int m_medicalInquiryCopies = 1;
		//private Lazy<DropShadowPopover> m_toDoPopover;
		private static class Query {
			internal static string Loaded = "view {accessibilityIdentifier BEGINSWITH 'PAGE:' AND accessibilityIdentifier ENDSWITH '/calls.cdl'}";
			internal static string BackButton = "view:'UILabel' marked:'Back'";
			internal static string SegmentedBar = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar'";
			internal static string DisplayOptionsLink = "view marked:'Display Options'";
			internal static string DisplayOptions = "view marked:'DATA:vt_event_detail_product/vc_checked'";
			internal static string DoneButton = "UINavigationButton marked:'Done'";
			internal static string NextButton = "UINavigationButton marked:'Next'";
			internal static string SideNavigationColumn = "view:'Dendrite.IPhone.Forms.CdlFrame' id:'navItems'";
			internal static string ScrollableColumn = "view:'Dendrite.IPhone.Forms.UIScrollableContent' index:0";
			internal static string ProfiledAttendees = SideNavigationColumn + " descendant label marked:'Profiled Attendees'";
			internal static string Presentation = SideNavigationColumn + " descendant label marked:'Presentation'";
			internal static string SampleDisbursed = SideNavigationColumn + " descendant label marked:'Sample Disbursed'";
			internal static string SampleRequest = SideNavigationColumn + " descendant label marked:'Sample Request'";
			internal static string MedicalInquiry = SideNavigationColumn + " descendant label marked:'Medical Inquiry'";
			internal static string NextCallObjective = SideNavigationColumn + " descendant label marked:'Next Call Objective'";
			internal static string CallDetails = SideNavigationColumn + " descendant * marked:'Call Details'";
			internal static string Surveys = SideNavigationColumn + " descendant * marked:'Surveys'";
			internal static string ItemsDisbursed = SideNavigationColumn + " descendant * marked:'Items Disbursed'";
			internal static string CallDialogue = SideNavigationColumn + " descendant * marked:'Call Dialogue'";
			internal static string Speakers = SideNavigationColumn + " descendant * marked:'Speakers'";
			internal static string CallNotes = SideNavigationColumn + " descendant label marked:'Call Notes'";
			internal static string ToDo = SideNavigationColumn + " descendant label marked:'To Do'";
			internal static string MarketingRequest = SideNavigationColumn + " descendant label marked:'Marketing Request'";
			internal static string NonProfiledAttendees = SideNavigationColumn + " descendant label marked:'Non-Profiled Attendees'";
			internal static string Expenses = SideNavigationColumn + " descendant label marked:'Expenses'";
			internal static string NavigationBar = "UINavigationBar";
			internal static string AttendeeHeader = "view:'Dendrite.IPhone.Forms.CdlFrame' marked:'dialogHeader'";
			internal static string SearchAll = AttendeeHeader + " descendant label marked:'Search All'";
			internal static string SearchAllField = "UIFieldEditor";
			internal static string CallTab = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' child label {text beginswith 'Call'}";
			internal static string PostCall = "view:'Dendrite.IPhone.Forms.CdlSegmentedBar' child label {text beginswith 'Post Call'}";
			internal static string Popover = "view:'_UIPopoverView'";
			internal static string SpeakerPopover = Popover + " descendant view marked:'Search' index:0";
			internal static string HonorariumPopover = Popover + " descendant view marked:'Honorarium' index:0";
			internal static string ProductPopover = Popover + " descendant view marked:'Product' index:0";
			internal static string TopicPopover = Popover + " descendant view marked:'Topic' index:0";
			internal static string SpeakerSearchField = Popover + " descendant UITextFieldLabel marked:'Search'";
			internal static string PopoverCells = Popover + " descendant UITableViewCell";
			internal static string DropShadowView = "UIDropShadowView";
			internal static string SampleRequestPopover = "view marked:'PAGE:calls/samplesrequestededit.cdl'";
			internal static string AddProduct = SampleRequestPopover + " descendant * marked:'KEY:ADD'";
			internal static string AddMedicalInquiryProduct = DropShadowView + " descendant * marked:'Add Product' index:0";
			internal static string AddMedicalInquiryNote = DropShadowView + " descendant * marked:'Add Note' index:0";
			internal static string QuantityPopover = Popover + " descendant * marked:'Quantity' index:0";
			internal static string MedicalInquiryNavigationBar = "UINavigationBar marked:'Medical Inquiry'";
			internal static string MedicalInquiryQuestionField = DropShadowView + " descendant view:'Dendrite.IPhone.Forms.DetachableLabel' marked:'Question (required)'";
			internal static string MedicalInquiryInstructionsField = DropShadowView + " descendant view:'Dendrite.IPhone.Forms.DetachableLabel' marked:'Special Handling Instructions'";
			internal static string MedicalInquiryType = DropShadowView + " descendant * marked:'Inquiry Type' sibling view:'Dendrite.IPhone.Forms.CdlDropDownList'";
			internal static string MedicalInquiryOwnerEmployee = DropShadowView + " descendant * marked:'Owner Employee' sibling view:'Dendrite.IPhone.Forms.CdlDropDownList'";
			internal static string MedicalInquiryChannel = DropShadowView + " descendant * marked:'Inquiry Channel' sibling view:'Dendrite.IPhone.Forms.CdlDropDownList'";
			internal static string MedicalInquiryDeliveryPreference = "view marked:'dialogWrapper' descendant label {text beginswith 'Preference'}";
			internal static string MedicalInquiryDeliveryEmail = "view marked:'dialogWrapper' descendant label {text beginswith 'Email Address'}";
			internal static string MedicalInquiryDeliveryPriority = "";
			internal static string MedicalInquiryDeliveryCopies = "";
			internal static string CallDetailsPurpose = ScrollableColumn + " descendant view marked:'__purpose'";
			internal static string CallDialogueCheckBoxes = "view marked:'DATA:survey_response/vc_check_response/cdlg'";
			internal static string CallDialogueRadioButtons = "view marked:'DATA:survey_response/vc_radio_response/mobi'";
			internal static string CallDialogueSurveyEntry = "view marked:'surveyEntryResponseText'";
			internal static string NextQuestion = "view marked:'Next Question' index:0";
			internal static string PreviousQuestion = "view marked:'Previous Question'";
			internal static string DropDownSelector = "view:'Dendrite.IPhone.Forms.CdlMultiRowDropDownList'";
			internal static string DropDownSelectoriOS8 = "view:'Dendrite.IPhone.Forms.CdlDropDownList'";
			internal static string FinishButton = "UIButton label marked:'Finish'";
			internal static string ClearButton = "UINavigationButton marked:'Clear'";
			// Switch used to be marked Preference when the spelling of email address had spacing due to using uia to enter string
			internal static string PrioritySwitch = "UISwitch marked:'Priority' parent view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+DefaultCheckBox'";
			internal static string MedicalInquiryCopies = "view marked:'dialogWrapper' descendant label {text beginswith 'Number of Copies'}";
			internal static string MedicalInquiryCopiesField = MedicalInquiryCopies + " sibling view:'Dendrite.IPhone.Forms.CdlTextBox'";
			internal static string ClearTextButton = "UITextField isFirstResponder:1 child button";
			internal static string SignatureButton = "view marked:'dialogWrapper' descendant label {text beginswith 'Signature'}";
			internal static string SignatureField = "view marked:'__signatureCtrl'";
			internal static string Accept = "UINavigationButton marked:'Accept'";
			internal static string NextCallObjectiveDate = "view marked:'dialogWrapper' descendant label {text beginswith 'Date'}";
			internal static string NextCallObjectiveNotes = "view marked:'dialogWrapper' descendant label {text beginswith 'Notes'}";
			internal static string NextCallObjectiveNotesField = NextCallObjectiveNotes + " sibling view:'Dendrite.IPhone.Forms.CdlMultiRowTextBox'";
			internal static string CallNotesField = Popover + " descendant view:'UIWebDocumentView'";
			internal static string ToDoSubjectField = DropShadowView + " descendant view id:'__subjectEdit'";
			internal static string ToDoDescriptionField = DropShadowView + " descendant view id:'__descriptionEdit'";
			internal static string ToDoDueDate = DropShadowView + " descendant view id:'__dueDateEdit'";
			internal static string ToDoDueTime = DropShadowView + " descendant view id:'__dueTimeEdit'";
			internal static string ToDoReminder = DropShadowView + " descendant view id:'DATA:vt_reminder/vc_reminder_time/todo'";
			internal static string Calendar = "view:'MI.CustomControls.CalendarView'";
			internal static string SignatureLabel = "UILabel marked:'Signature'";
			internal static string AddToolBarButton = "UIToolbarButton marked:'Add'";
			internal static string AccompaniedBy = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'DATA:event/accompanied_by'";
			internal static string DocumentId = "UILabel marked:'Document Id'";
			internal static string CallDetailsSignatureButton = "view:'MI.CustomControls.DmSignatureCheckBoxLockItemControlView'";
			internal static string CustomerLink = "view marked:'mainHeadingLink'";
			internal static string SwitchCustomer = "UIToolbarTextButton marked:'Switch Customer'";
			internal static string SaveForLater = "UILabel marked:'Save for Later'";
			internal static string ManageInvitationsIcon = "imageView {accessibilityIdentifier ENDSWITH 'managecustomers.png'} index:0";
			internal static string AttendeeCheckBox = "view marked:'DATA:event_attendee/vc_selected'";
			internal static string CheckBox = "view:'Dendrite.IPhone.Forms.CdlCheckBoxFactory+MultiCheckBox'";
			internal static string InvitationStatusButton = "view:'Dendrite.IPhone.Forms.CdlButton' id:'btnInvitationStatus'";
			internal static string AttendeeAttendedButton = "view marked:'KEY:ATTD'";
		}

		public NewCallPage(IApp application) {
			app = application;
		}

		public void IsLoaded() {
			app.WaitForElement(c => c.Raw(Query.Loaded));
		}

//		public string CallPurpose {
//			get {
//				return m_callPurpose;
//			}
//			set {
//				if (!TestIsVisibleToElement(Query.ScrollableColumn, Query.CallDetailsPurpose)) {
//					try {
//						SwipeUpUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, Query.CallDetailsPurpose, 
//							postTimeout: TimeSpan.FromSeconds(0.5));
//					} catch {
//						SwipeDownUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, Query.CallDetailsPurpose, 
//							postTimeout: TimeSpan.FromSeconds(0.5));
//					}
//				}
//				Thread.Sleep(TimeSpan.FromSeconds(0.2)); // step pause
//				TapAndWait(Query.CallDetailsPurpose, () => TestIsVisible(Query.Popover));
//				string selectPurposeQuery = String.Format(Query.Popover + " descendant * marked:'{0}'", value);
//				if (!TestIsVisible(selectPurposeQuery))
//					throw new Exception("cannot select this purpose for the call");
//				TapAndWait(selectPurposeQuery, () => !TestIsVisible(Query.Popover));
//				m_callPurpose = value;
//			}
//		}
//
//		public int DocumentId {
//			get {
//				return m_documentId;
//			}
//			set {
//				TapAndWait(Query.DocumentId, () => TestIsVisible(Query.DropShadowView));
//				var documentIdPopover = new DropShadowPopover(Application, Container);
//				documentIdPopover.AddText("Document Id", value.ToString(), clearText: true, tapActionKey: true);
//				Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
//				documentIdPopover.Done();
//				m_documentId = value;
//				Wait(() => TestIsVisible("view marked:'Save for Later' index:0"));
//			}
//		}
//
//		public string AccompaniedBy {
//			get {
//				return m_accompaniedBy;
//			}
//			set {
//				if (!TestIsVisibleToElement(Query.ScrollableColumn, Query.AccompaniedBy)) {
//					try {
//						SwipeUpUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, Query.AccompaniedBy);
//					} catch {
//						SwipeDownUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, Query.AccompaniedBy);
//					}
//				}
//				TapAndWait(Query.AccompaniedBy, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
//				if (TestIsVisible(Query.ClearButton)) {
//					TapAndWait(Query.ClearButton, () => !TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
//					TapAndWait(Query.AccompaniedBy, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
//				}
//				string itemQuery = string.Format("label marked:'{0}' index:0", value);
//				Wait(() => TestIsVisible(itemQuery), postTimeout: TimeSpan.FromSeconds(0.5));
//				TapAndWait(itemQuery, () => !TestIsVisible(Query.Popover));
//				AssertElementExists(string.Format("view marked:'{0}' index:0", value));
//			}
//		}
//
//		public bool Priority {
//			get {
//				if (TestIsVisible(Query.PrioritySwitch + " id:'VAL:True'"))
//					return true;
//				else
//					return false;
//			}
//			set {
//				if (TestIsVisible(Query.PrioritySwitch + " id:'VAL:True'") && !value)
//					TapAndWait(Query.PrioritySwitch, () => TestIsVisible(Query.PrioritySwitch + " id:'VAL:False'"));
//				else if (TestIsVisible(Query.PrioritySwitch + " id:'VAL:False'") && value)
//					TapAndWait(Query.PrioritySwitch, () => TestIsVisible(Query.PrioritySwitch + " id:'VAL:True'"));
//				else if(!TestIsVisible(Query.PrioritySwitch + " id:'VAL:False'") && !TestIsVisible(Query.PrioritySwitch + " id:'VAL:True'") && value)
//					TapAndWait(Query.PrioritySwitch, () => TestIsVisible(Query.PrioritySwitch + " id:'VAL:True'"));
//
//			}
//		}
//
//		public int MedicalInquiryCopies {
//			get {
//				return m_medicalInquiryCopies;
//			}
//			set {
//				// Assuming Medical Inquiry Popover is open
//				TapAndWait(Query.MedicalInquiryCopies, () => IsKeyboardVisible());
//				Thread.Sleep(TimeSpan.FromSeconds(1)); // wait for animation
//				// Clear any existing number
//				AppConvention.Wait(Application, () => TestIsVisible(Query.ClearTextButton));
//				TapAndWait(Query.ClearTextButton, () => !TestIsVisible(Query.ClearTextButton));
//				Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//				SetField(Query.MedicalInquiryCopiesField, value.ToString());
//				Calabash.Tap(CalabashButton.Enter);
//				m_medicalInquiryCopies = value;
//			}
//		}
//
//		public void SignCallDetails() {
//			Wait(() => TestIsVisible(Query.CallDetailsSignatureButton), postTimeout: TimeSpan.FromSeconds(0.3));
//			TapAndWait(Query.CallDetailsSignatureButton, () => TestIsVisible(Query.SignatureField));
//			var signatureRectangle = Calabash.Query(Query.SignatureField).First().Rectangle;
//			var initialX = signatureRectangle.X + signatureRectangle.Width * 0.1;
//			var finalX = signatureRectangle.X + signatureRectangle.Width * 0.9;
//			var initialY = signatureRectangle.Y + signatureRectangle.Height * 0.1;
//			var finalY = signatureRectangle.Y + signatureRectangle.Height * 0.9;
//			Calabash.Pan(new Point((int)initialX, (int)initialY), new Point((int)finalX, (int)finalY));
//			TapAndWait(Query.Accept, () => TestIsVisible("view marked:'Yes'"));
//		}
//
//		public void DetailFirstProduct(int? optionNumber = 0) {
//			optionNumber = (int)optionNumber;
//			string displayOptionQuery = Query.DisplayOptions + " index:" + optionNumber.ToString();
//			if (!TestIsVisibleToElement(Query.ScrollableColumn, displayOptionQuery))
//				SwipeDownUntilVisibleToElement(Query.ScrollableColumn, Query.ScrollableColumn, Query.DisplayOptionsLink,
//					postTimeout: TimeSpan.FromSeconds(2.5));
//			TapAndWait(Query.DisplayOptionsLink, () => TestIsVisible(Query.DisplayOptions + " index:0"));
//			TapAndWait(displayOptionQuery, () => TestIsVisible(displayOptionQuery));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(displayOptionQuery));
//		}
//
//		public void AddProfiledAttendee(int index) {
//			var callCustomers = CallPage.CallCustomers;
//			string customerName = callCustomers[index - 1]["Name"].ToString();
//			TapAndWait(Query.ProfiledAttendees, () => TestIsVisible(Query.NavigationBar));
//			TapAndWait(Query.SearchAll, () => IsKeyboardVisible());
//			SetField(Query.SearchAllField, customerName);
//			Calabash.Tap(CalabashButton.Enter);
//			string userSearchResult = String.Format("view marked:'DATA:vt_query/vc_display_name' descendant label {{text beginswith '{0}'}} index:0", customerName);
//			string userCheckBox = userSearchResult + " parent * index:2 child * marked:'DATA:vt_query/vc_selected'";
//			string userCheckBoxOn = userCheckBox + " descendant * id:'VAL:True'";
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(userCheckBox, () => TestIsVisible(userCheckBoxOn));
//			Thread.Sleep(TimeSpan.FromSeconds(0.5)); // Step Pause
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(userSearchResult));
//		}
//
//		public void NavigateToPostCallTab() {
//			if(TestIsVisible(Query.PostCall)) {
//				TapAndWait(Query.PostCall, () => TestIsVisible(Query.CallDetails));
//			}
//		}
//
//		public void TapPopoverCell(int index) {
//			string cellQuery = String.Format(Query.PopoverCells + " index:'{0}'", index);
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(cellQuery, () => TestIsVisible(Query.TopicPopover));
//		}
//
//		public void AddSpeaker() {
//			var callCustomers = CallPage.CallCustomers;
//			string customerName = callCustomers[1]["Name"].ToString();
//			string customerId = callCustomers[1]["customerid"].ToString();
//			NavigateToPostCallTab();
//			TapAndWait(Query.Speakers, () => TestIsVisible(Query.Popover));
//			SetField(Query.SpeakerSearchField, customerName);
//			Calabash.Tap(CalabashButton.Enter);
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			string queryCustomerId = String.Format("{0} descendant view marked:'KEY:{1}'", Query.Popover, customerId);
//			AppConvention.Wait(Application, () => TestIsVisible(queryCustomerId));
//			TapAndWait(queryCustomerId, () => TestIsVisible(Query.HonorariumPopover));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(Query.NextButton, () => TestIsVisible(Query.ProductPopover));
//			TapPopoverCell(0);
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//			AssertElementExists(String.Format("* {{text contains '{0}'}} index:0", customerName));
//			AssertElementExists("view:'Dendrite.IPhone.Forms.CdlBaseTitle+TitleLabel' marked:'Speakers' index:0");
//		}
//
//		public void SelectSampleRequest() {
//			var callCustomers = CallPage.CallCustomers;
//			string alignmentId = callCustomers[0]["alignmentid"].ToString();
//			string sampleRequestProduct = Calabash.SelectAvailableSampleRequestProduct(alignmentId);
//			CallPage.SampleRequestProduct = JArray.Parse(sampleRequestProduct);
//		}
//
//		public void SelectSampleProduct() {
//			// This query seems to only actually depend on the count you want to limit to
//			string sampleProduct = Calabash.SelectSampleProductWithLotNumber(1);
//			CallPage.SampleProduct = JArray.Parse(sampleProduct);
//		}
//
//		public void AddSampleRequest() {
//			TapAndWait(Query.SampleRequest, () => TestIsVisible(Query.AddProduct));
//			string sampleProduct = CallPage.SampleRequestProduct[0]["vc_product_name"].ToString();
//			string productQuery = String.Format("view:'Dendrite.IPhone.Forms.CdlMultiRowTextBoxRO' descendant * marked:'{0}'", sampleProduct);
//			TapAndWait(Query.AddProduct, () => TestIsVisible(productQuery));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(productQuery, () => TestIsVisible(Query.QuantityPopover));
//			Thread.Sleep(TimeSpan.FromSeconds(1.5)); // Step Pause
//			TapAndWait("UIButton marked:'5'", () => TestIsVisible("UITextFieldLabel marked:'5'"));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.QuantityPopover));
//			AppConvention.Wait(Application, () => TestIsVisible(Query.DoneButton));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.SampleRequestPopover));
//			AssertElementExists(String.Format("* marked:'{0}' index:0", sampleProduct));
//		}
//
//		public void ChooseMedicalInquiryProduct(int index) {
//			string productQuery = String.Format(Query.Popover + " descendant UITableViewCell index:'{0}'", index);
//			TapAndWait(productQuery, () => TestIsVisible(Query.Popover));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//		}
//
//		public void AddMedicalNote(string noteText) {
//			AppConvention.Wait(Application, () => TestIsVisible(Query.AddMedicalInquiryNote));
//			TapAndWait(Query.AddMedicalInquiryNote, () => TestIsVisible(Query.Popover));
//			AppConvention.Wait(Application, () => IsKeyboardVisible());
//			SetField(Query.Popover + " descendant UIWebDocumentView", noteText);
//			Calabash.Tap(CalabashButton.Enter);
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//		}
//
//		public void SelectInquiryType(int index) {
//			string contentQuery = Query.DropShadowView + " descendant * marked:'dialogContent' index:0 descendant view:'Dendrite.IPhone.Forms.UIScrollableContent'";
//			Thread.Sleep(TimeSpan.FromSeconds(2));
//			if (!TestIsVisibleToElement(contentQuery, Query.MedicalInquiryType)) {
//				SwipeDownUntilVisibleToElement(contentQuery, contentQuery, Query.MedicalInquiryType);
//				Thread.Sleep(TimeSpan.FromSeconds(3)); // step pause to wait for animation
//			}
//			// ive locally reproed that you need to click this twice - add a break point to verify
//			//TapAndWait(Query.MedicalInquiryType, () => TestIsVisible("* index:0")); // Remove this tap after finish test suite
//			string itemQuery = String.Format(Query.Popover + " descendant UITableViewCell index:'{0}'", index);
//			Wait(() => TestIsVisible(Query.MedicalInquiryType));
//			// You have to tap the center quickly of the DropDownItem to repro it - throwing in try catch meanwhile to finish
//			// the test suite
//			try {
//				TapAndWait(Query.MedicalInquiryType, () => TestIsVisible(itemQuery));
//			} catch {
//				TapAndWait(Query.MedicalInquiryType, () => TestIsVisible(itemQuery));
//			}
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(itemQuery, () => !TestIsVisible(Query.Popover));
//		}
//
//		public void SelectInquiryChannel(int index) {
//			if (!TestIsVisible(Query.MedicalInquiryChannel))
//				SwipeDownUntilVisibleToElement(Query.DropShadowView, Query.DropShadowView, Query.MedicalInquiryChannel);
//			// Clear any existing selections
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.MedicalInquiryChannel, () => TestIsVisible(Query.Popover));
//			TapAndWait(Query.ClearButton, () => !TestIsVisible(Query.Popover));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.MedicalInquiryChannel, () => TestIsVisible(Query.Popover));
//			string itemQuery = String.Format(Query.Popover + " descendant UITableViewCell index:'{0}'", index);
//			TapAndWait(itemQuery, () => !TestIsVisible(Query.Popover));
//		}
//
//		public void SetDeliveryOptions() {
//			AppConvention.Wait(Application, () => TestIsVisible(Query.MedicalInquiryDeliveryPreference));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.MedicalInquiryDeliveryPreference, () => TestIsVisible(Query.Popover));
//			string preferenceQuery = Query.Popover + " descendant UITableViewCell index:0";
//			string checkMarkQuery = preferenceQuery + " descendant UIImageView index:0";
//			string preferenceQuerySecondRow = Query.Popover + " descendant UITableViewCell index:1";
//			string checkMarkQuerySecondRow = preferenceQuery + " descendant UIImageView index:0";
//			TapAndWait(preferenceQuery, () => TestIsVisible(checkMarkQuery));
//			TapAndWait(preferenceQuerySecondRow, () => TestIsVisible(checkMarkQuerySecondRow));
//			TapAndWait(Query.DoneButton, () => TestIsVisible(Query.MedicalInquiryDeliveryEmail));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step Pause
//			TapAndWait(Query.MedicalInquiryDeliveryEmail, () => IsKeyboardVisible());
//			string emailField = Query.MedicalInquiryDeliveryEmail + " sibling view:'Dendrite.IPhone.Forms.CdlTextBox'";
//			// Using convention set field here to hopefully avoid the auto correct
//			AppConvention.SetField(Container, emailField, "tony.wu@cegedim.com"); 
//			Calabash.Tap(CalabashButton.Enter);
//		}
//
//		public void SelectMedicalInquiryProduct(int index) {
//			string productQuery = String.Format(Query.Popover + " descendant UITableViewCell index:'{0}'", index);
//			string checkMarkQuery = productQuery + " descendant UIImageView index:0";
//			TapAndWait(productQuery, () => TestIsVisible(checkMarkQuery));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//		}
//
//		public void SignMedicalInquiry() {
//			TapAndWait(Query.SignatureButton, () => TestIsVisible(Query.SignatureField), timeout: TimeSpan.FromSeconds(15));
//			var signatureRectangle = Calabash.Query(Query.SignatureField).First().Rectangle;
//			var initialX = signatureRectangle.X + signatureRectangle.Width * 0.1;
//			var finalX = signatureRectangle.X + signatureRectangle.Width * 0.9;
//			var initialY = signatureRectangle.Y + signatureRectangle.Height * 0.1;
//			var finalY = signatureRectangle.Y + signatureRectangle.Height * 0.9;
//			Calabash.Pan(new Point((int)initialX, (int)initialY), new Point((int)finalX, (int)finalY));
//			TapAndWait(Query.Accept, () => TestIsVisible("UIButton marked:'1'"));
//			// Enter the Pin
//			TapAndWait("UIButton marked:'1'", () => Calabash.Query("UITextFieldLabel").Count() == 1);
//			TapAndWait("UIButton marked:'2'", () => Calabash.Query("UITextFieldLabel").Count() == 2);
//			TapAndWait("UIButton marked:'3'", () => Calabash.Query("UITextFieldLabel").Count() == 3);
//			TapAndWait("UIButton marked:'4'", () => TestIsVisible(Query.SignatureLabel), timeout: TimeSpan.FromSeconds(15));
//		}
//
//		public void AddMedicalInquiry() {
//			Wait(() => TestIsVisible(Query.MedicalInquiry));
//			TapAndWait(Query.MedicalInquiry, () => TestIsVisible(Query.MedicalInquiryQuestionField));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			SetField(Query.MedicalInquiryQuestionField, "What is the side effect of the product ActHIB?");
//			SetField(Query.MedicalInquiryInstructionsField, "Keep it out of reach of children");
//			Wait(() => TestIsVisible(Query.AddMedicalInquiryProduct));
//			TapAndWait(Query.AddMedicalInquiryProduct, () => TestIsVisible(Query.Popover));
//			SelectMedicalInquiryProduct(0);
//			AddMedicalNote("Some notes for ActHIB");
//			SelectInquiryType(0);
//			SelectInquiryChannel(0);
//			SetDeliveryOptions();
//			Priority = true;
//			MedicalInquiryCopies = 5;
//			SignMedicalInquiry();
//		}
//
//		public void AddCallDialogues() {
//			Wait(() => TestIsVisible(Query.CallDialogue), timeout: TimeSpan.FromSeconds(4));
//			string checkBoxQuery = Query.CallDialogueCheckBoxes + " index:0";
//			TapAndWait(Query.CallDialogue, () => TestIsVisible(checkBoxQuery));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(checkBoxQuery, ()=> TestIsVisible(Query.DropShadowView));
//			TapAndWait(Query.NextQuestion, () => TestIsVisible(Query.PreviousQuestion));
//			string dropDownSelector;
//			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XTC_DEVICE_OS"))) {
//				if (Environment.GetEnvironmentVariable("XTC_DEVICE_OS").Contains("7."))
//					dropDownSelector = Query.DropDownSelector;
//				else
//					dropDownSelector = Query.DropDownSelectoriOS8;
//			} else {
//				// testing locally on iOS8 otherwise you may need to change this
//				dropDownSelector = Query.DropDownSelectoriOS8;
//			}
//			Wait(() => TestIsVisible(dropDownSelector));
//			TapAndWait(dropDownSelector, () => TestIsVisible(Query.Popover));
//			string dropDownResponse = Query.Popover + " descendant UITableViewCell index:0";
//			TapAndWait(dropDownResponse, () => !TestIsVisible(Query.Popover));
//			AppConvention.Wait(Application, () => TestIsVisible(Query.NextQuestion));
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(Query.NextQuestion, () => !TestIsVisible(Query.DropDownSelector), timeout: TimeSpan.FromSeconds(10));
//			string radioButtonQuery = Query.CallDialogueRadioButtons + " index:0";
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			TapAndWait(radioButtonQuery, () => TestIsVisible(Query.DropShadowView));
//			TapAndWait(Query.NextQuestion, () => TestIsVisible(Query.CallDialogueSurveyEntry));
//			// Throwing try catch because of web exception that occurs - could be bug with iOS 8 testing tools in UIAutomation?
//			try {
//				SetField(Query.CallDialogueSurveyEntry, "I am very happy about your products, MI touch is the best in class");
//			} catch {
//			}
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.DropShadowView));
//			AssertElementExists("* marked:'XTC'");
//
//		}
//
//		public SearchPage Finish() {
//			TapAndWait(Query.FinishButton, () => !TestIsVisible(Query.FinishButton));
//			return AppConvention.TapActivateAndWait<SearchPage>(
//				Application, Query.BackButton);
//		}
//
//		public PlannerPage FinishOnPlanner() {
//			return AppConvention.TapActivateAndWait<PlannerPage>(
//				Application, Query.FinishButton);
//		}
//
//		public void AddCallObjective() {
//			Wait(() => TestIsVisible(Query.CallTab));
//			TapAndWait(Query.CallTab, () => TestIsVisible(Query.NextCallObjective));
//			TapAndWait(Query.NextCallObjective, () => TestIsVisible(Query.DropShadowView), postTimeout: TimeSpan.FromSeconds(1));
//			var today = DateTime.Today;
//			var daysUntilNextTuesday = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7;
//			if (daysUntilNextTuesday == 0)
//				daysUntilNextTuesday = 7;
//			// Set Next Tuesday at 11:30 AM
//			var nextTuesday = today.AddDays((double)daysUntilNextTuesday).Date + new TimeSpan(11, 30, 0);
//			TapAndWait(Query.NextCallObjectiveDate, () => TestIsVisible(Query.Popover), timeout: TimeSpan.FromSeconds(10));
//			// Create Calendar type
//			var calendar = new Calendar(Application, this, Container.GetDescendant(Query.Popover));
//			calendar.Year = nextTuesday.Year;
//			calendar.Month = nextTuesday.ToString("MMMM"); // Returns month as a name
//			calendar.Day = nextTuesday.Day;
//			// TODO: Add the hour to the date which wasn't implemented in Ruby at the time of my pull
//			TapAndWait(Query.DoneButton, () => TestIsVisible(Query.NextCallObjectiveNotes));
//			TapAndWait(Query.NextCallObjectiveNotes, () => IsKeyboardVisible());
//			Thread.Sleep(TimeSpan.FromSeconds(1)); // Step pause
//			SetField(Query.NextCallObjectiveNotesField, "Make sure you buy the doctor a coffee next time :)");
//			Wait(() => TestIsVisible(Query.DoneButton), postTimeout: TimeSpan.FromSeconds(0.3));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.DropShadowView), timeout: TimeSpan.FromSeconds(15));
//			AssertElementExists("view marked:'Make sure you buy the doctor a coffee next time :)' index:0");
//		}
//
//		public void AddItemDisbursed(int productQuantity) {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.ItemsDisbursed));
//			TapAndWait(Query.ItemsDisbursed, () => TestIsVisible(Query.Popover));
//			string itemQuery = Query.Popover + " descendant label marked:'Promotional Item'";
//			string firstProduct = Query.Popover + " descendant UITableViewCell index:0";
//			TapAndWait(itemQuery, () => TestIsVisible(firstProduct));
//			Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//			TapAndWait(firstProduct, () => TestIsVisible(Query.Popover  + " descendant * marked:'Quantity' index:0"));
//			string quantityQuery = string.Format(Query.Popover + " descendant UIButton marked:'{0}'", productQuantity.ToString());
//			string quantityLabel = string.Format(Query.Popover + " descendant UITextFieldLabel marked:'{0}'", productQuantity.ToString());
//			Thread.Sleep(TimeSpan.FromSeconds(2)); // Step pause
//			TapAndWait(quantityQuery, () => TestIsVisible(quantityLabel));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//			AssertElementExists("view marked:'Promotional Item'");
//		}
//
//		public void AddCallNotes(string callNotes) {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.CallNotes));
//			TapAndWait(Query.CallNotes, () => IsKeyboardVisible());
//			SetField(Query.CallNotesField, callNotes);
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//			string callNotesQuery = string.Format("view marked:'{0}'", callNotes);
//			AssertElementExists(callNotesQuery);
//		}
//
//		public CallToDoPopover CreateToDo() {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.ToDo));
//			return AppConvention.TapActivateAndWait<CallToDoPopover>(
//				Application, Query.ToDo);
//		}
//
//		public MarketingRequestPopover CreateMarketingRequest() {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.MarketingRequest));
//			return AppConvention.TapActivateAndWait<MarketingRequestPopover>(
//				Application, Query.MarketingRequest);
//		}
//
//		public NonProfiledAttendeesPopover CreateNonProfiledAttendees() {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.NonProfiledAttendees));
//			TapAndWait(Query.NonProfiledAttendees, () => TestIsVisible(Query.Popover));
//			Wait(() => TestIsVisible(Query.AddToolBarButton));
//			return AppConvention.TapActivateAndWait<NonProfiledAttendeesPopover>(
//				Application, Query.AddToolBarButton);
//		}
//
//		public ExpensesPopover CreateExpenses() {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.MarketingRequest));
//			return AppConvention.TapActivateAndWait<ExpensesPopover>(
//				Application, Query.Expenses);
//		}
//
//		public SampleDisbursedPopover CreateSampleDisbursed() {
//			TapAndWait(Query.CallTab, () => TestIsVisible(Query.SampleDisbursed));
//			SelectSampleProduct();
//			return AppConvention.TapActivateAndWait<SampleDisbursedPopover>(
//				Application, Query.SampleDisbursed);
//		}
//
//		public CallDetailsPopover CreateCallDetails() {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.CallDetails));
//			return AppConvention.TapActivateAndWait<CallDetailsPopover>(
//				Application, Query.CallDetails);
//		}
//
//		public void AddToDo(string subjectText, string descriptionText) {
//			TapAndWait(Query.PostCall, () => TestIsVisible(Query.ToDo));
//			TapAndWait(Query.ToDo, () => TestIsVisible(Query.DropShadowView));
//			SetField(Query.ToDoSubjectField, subjectText);
//			SetField(Query.ToDoDescriptionField, descriptionText);
//			TapAndWait(Query.ToDoDueDate, () => TestIsVisible(Query.Calendar));
//			var calendar = new Calendar(Application, this, Container.GetDescendant(Query.Popover));
//			var today = DateTime.Today;
//			var daysUntilNextTuesday = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7;
//			if (daysUntilNextTuesday == 0)
//				daysUntilNextTuesday = 7;
//			// Set Next Tuesday at 11:30 AM
//			var nextTuesday = today.AddDays((double)daysUntilNextTuesday).Date + new TimeSpan(11, 30, 0);
//			calendar.Year = nextTuesday.Year;
//			calendar.Month = nextTuesday.ToString("MMMM"); // Returns month as a name
//			calendar.Day = nextTuesday.Day; // Automatically closes calendar in this instance
//			// TODO: Set arbitrary due time the ruby client at the time of my fork didn't have it
//			// doing trivial time in this case
//			TapAndWait(Query.ToDoDueTime, () => TestIsVisible(Query.Popover));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.Popover));
//			TapAndWait(Query.ToDoReminder, () => TestIsVisible(Query.Popover));
//			TapAndWait(Query.ClearButton, () => !TestIsVisible(Query.Popover));
//			TapAndWait(Query.ToDoReminder, () => TestIsVisible(Query.Popover + " descendant UITableViewCell index:2"));
//			TapAndWait(Query.Popover + " descendant UITableViewCell index:2", () => !TestIsVisible(Query.Popover));
//		}
//
//		public void SwitchCustomerFromDatabase() {
//			TapAndWait(Query.CustomerLink, () => TestIsVisible(Query.SwitchCustomer));
//			TapAndWait(Query.SwitchCustomer, () => IsKeyboardVisible());
//			string customerName = CallPage.CallCustomers[1]["Name"].ToString();
//			string customerId = CallPage.CallCustomers[1]["customerid"].ToString();
//			SetField(Query.SearchAllField, customerName);
//			Calabash.Tap(CalabashButton.Enter); // Hit search
//			string customerQuery = string.Format(Query.Popover + " descendant view marked:'KEY:{0}'", customerId);
//			string addressQuery = "UITableViewCell index:0";
//			Wait(() => TestIsVisible(customerQuery), postTimeout: TimeSpan.FromSeconds(0.5));
//			TapAndWait(customerQuery, () => !TestIsVisible(customerQuery), postTimeout: TimeSpan.FromSeconds(0.3));
//			TapAndWait(addressQuery, () => !TestIsVisible(Query.Popover));
//			Wait(() => TestIsVisible(Query.Loaded)); // Check we are still on the call page
//		}
//
//		public CallReadPage SaveForLater() {
//			return AppConvention.TapActivateAndWait<CallReadPage>(
//				Application, Query.SaveForLater);
//		}
//
//		public void VerifyAppointment(string category, string expectedValue) {
//			string itemQuery = string.Format("view marked:'{0}' sibling view text:'{1}' index:0", category, expectedValue);
//			AssertElementExists(itemQuery);
//		}
//
//		public void VerifyAppointment(string expectedValue) {
//			string itemQuery = string.Format("view marked:'{0}'", expectedValue);
//			AssertElementExists(itemQuery);
//		}
//
//		public void VerifyAppointmentAttendees() {
//			var customerName = NewAppointmentPopover.CustomerName;
//			var attendeeName = NewAppointmentPopover.AttendeeName;
//			string customerQuery = string.Format("view:'MI.CustomControls.CdlIPhoneBadgeControl' {{text contains '{0}'}}", customerName);
//			string attendeeQuery = string.Format("view:'MI.CustomControls.CdlIPhoneBadgeControl' {{text contains '{0}'}}", attendeeName);
//			AssertElementExists(customerQuery);
//			AssertElementExists(attendeeQuery);
//		}
//
//		public void VerifyAppointmentDate(DateTime date) {
//			// Hopefully this date formating works regardless of locale
//			string dateString = string.Format("{0}/{1}/{2}", date.Month, date.Day, date.Year);
//			AssertElementExists(string.Format("view {{text contains '{0}'}} index:0", dateString));
//		}
//
//		public void ManageInvitations() {
//			Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//			AssertElementExists(Query.ManageInvitationsIcon);
//			TapAndWait(Query.ManageInvitationsIcon, () => TestIsVisible(Query.DropShadowView), postTimeout: TimeSpan.FromSeconds(1));
//			SetField(Query.SearchAllField, NewAppointmentPopover.AttendeeName);
//			Calabash.Tap(CalabashButton.Enter);
//			Wait(() => TestIsVisible(Query.AttendeeCheckBox), postTimeout: TimeSpan.FromSeconds(0.5));
//			string checkedBox = string.Format("{0} id:'VAL:True'", Query.CheckBox);
//			TapAndWait(Query.AttendeeCheckBox, () => TestIsVisible(checkedBox));
//			TapAndWait(Query.InvitationStatusButton, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1));
//			if (!TestIsVisibleToElement(Query.Popover, Query.AttendeeAttendedButton))
//				SwipeDownUntilVisibleToElement(Query.Popover, Query.Popover, Query.AttendeeAttendedButton, 
//					postTimeout: TimeSpan.FromSeconds(2.5));
//			TapAndWait(Query.AttendeeAttendedButton, () => !TestIsVisible(Query.Popover));
//			Wait(() => TestIsVisible(Query.DoneButton), postTimeout: TimeSpan.FromSeconds(1));
//			TapAndWait(Query.DoneButton, () => !TestIsVisible(Query.DropShadowView));
//		}
	}

	public class NewCalendar {
		private int m_day;
		private IApp app;
		private Dictionary<string, int> monthTable;
		private static class Query {
			internal static string Loaded = "view:'MI.CustomControls.CalendarView'";
			internal static string DayQuery = "view:'MI.CustomControls.CalendarDayView'";
			internal static string NextMonth = "view:'MI.CustomControls.CalButton' index:2";
			internal static string PreviousMonth = "view:'MI.CustomControls.CalButton' index:0";
			internal static string NextYear = "view:'MI.CustomControls.CalButton' index:3";
			internal static string PreviousYear = "view:'MI.CustomControls.CalButton' index:1";
		}

		internal NewCalendar(IApp application) {
			app = application;
			monthTable = new Dictionary<string, int>();
			monthTable.Add("January", 1);
			monthTable.Add("February", 2);
			monthTable.Add("March", 3);
			monthTable.Add("April", 4);
			monthTable.Add("May", 5);
			monthTable.Add("June", 6);
			monthTable.Add("July", 7);
			monthTable.Add("August", 8);
			monthTable.Add("September", 9);
			monthTable.Add("October", 10);
			monthTable.Add("November", 11);
			monthTable.Add("December", 12);
		}

		public bool IsLoaded {
			get { return app.TestIsVisible(c => c.Raw(Query.Loaded)); }
		}

		public int Year {
			get {
				var rawMonthYearString = app.Query(c => c.Raw(Query.Loaded)).First().Id;
				char[] parseChar = {':', '/'};
				string[] monthYear = rawMonthYearString.Split(parseChar);
				string year = monthYear[2];
				return Convert.ToInt32(year);
			}
			set {
				Func<bool> tapToYear = () => {
					var currentYear = Year;
					if (currentYear != value) {
						if (value > currentYear) {
							app.Tap(c => c.Raw(Query.NextYear));
							app.WaitFor(() => currentYear != Year, postTimeout: TimeSpan.FromSeconds(0.2));
						} else {
							app.Tap(c => c.Raw(Query.PreviousYear));
							app.WaitFor(() => currentYear != Year, postTimeout: TimeSpan.FromSeconds(0.2));
						}
						return false;
					} else {
						return true;
					}
				};
				app.WaitFor(tapToYear, timeout: TimeSpan.FromSeconds(20));
			}
		}

		public string Month {
			get {
				var rawMonthYearString = app.Query(c => c.Raw(Query.Loaded)).First().Id;
				char[] parseChar = { ':', '/' };
				string[] monthYear = rawMonthYearString.Split(parseChar);
				string month = monthYear[1];
				return month;
			}
			set {
				Func<bool> tapToMonth = () => {
					var currentMonth = Month;
					if (currentMonth != value) {
						if (monthTable[currentMonth] < monthTable[value]) {
							app.Tap(c => c.Raw(Query.NextMonth));
							app.WaitFor(() => currentMonth != Month, postTimeout: TimeSpan.FromSeconds(0.2));
						}
						else {
							app.Tap(c => c.Raw(Query.PreviousMonth));
							app.WaitFor(() => currentMonth != Month, postTimeout: TimeSpan.FromSeconds(0.2));
						}
						return false;
					} else {
						return true;
					}
				};
				app.WaitFor(tapToMonth, timeout: TimeSpan.FromSeconds(40));
			}
		}

		public int Day {
			get {
				return m_day; // since I don't know a way to actually find the value via query
			}
			set {
				Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
				string validDayQuery = string.Format(Query.DayQuery + " id:'VAL:{0}'", value.ToString());
				var initialQuery = app.Query(c => c.Raw("*"));
				try {
					app.Tap(c => c.Raw(validDayQuery));
					app.WaitFor(() => app.Query(c => c.Raw("*")) != initialQuery);
				} catch {
					if (app.TestIsVisible (c => c.Raw(validDayQuery + " index:1"))) {
						app.Tap (c => c.Raw(validDayQuery + " index:1"));
						app.WaitFor (() => app.Query (c => c.Raw("*")) != initialQuery);
					}
				}
				m_day = value;
			}
		}
	}

//	public class CallToDoPopover : DropShadowPopover {
//		private string m_subject = "";
//		private string m_description = "";
//		private string m_type = "";
//		private string m_reminder = "";
//		private string m_channel = "";
//		private Lazy<DefaultCheckBox> m_showInPlanner;
//		private DateTime m_time;
//		private Lazy<Calendar> m_calendar;
//
//		private static class Query {
//			internal static string Loaded = "UINavigationBar marked:'To Do'";
//			internal static string Popover = "view:'_UIPopoverView'";
//			internal static string ShowInPlanner = "view:'Dendrite.IPhone.Forms.CdlEditItem' marked:'__sipNewTodo'";
//			internal static string DueDate = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'__dueDateEdit'";
//		}
//
//		internal CallToDoPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//			m_calendar = new Lazy<Calendar>(() => CalendarFactory(Query.Popover));
//			m_showInPlanner = new Lazy<DefaultCheckBox>(() => DefaultCheckBoxFactory(Query.ShowInPlanner));
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(2)); // step pause
//				return TestIsVisible(Query.Loaded); 
//			}
//		}
//
//		private DefaultCheckBox DefaultCheckBoxFactory(string query) {
//			return new DefaultCheckBox(Application, this, Container.GetDescendant(query));
//		}
//
//		private Calendar CalendarFactory(string query) {
//			return new Calendar(Application, this, Container.GetDescendant(query));
//		}
//
//		public string Subject {
//			get {
//				return m_subject;
//			}
//			set {
//				AddText("Subject", value, tapActionKey: true);
//				m_subject = value;
//			}
//		}
//
//		public string Description {
//			get {
//				return m_description;
//			}
//			set {
//				AddText("Description", value);
//				m_description = value;
//			}
//		}
//
//		public string Channel {
//			get {
//				return m_channel;
//			}
//			set {
//				SelectDropDown("Channel", value);
//				m_channel = value;
//			}
//		}
//
//		public string Reminder {
//			get {
//				return m_reminder;
//			}
//			set {
//				SelectDropDown("Reminder", value);
//				m_reminder = value;
//			}
//		}
//
//		public string Type {
//			get {
//				return m_type;
//			}
//			set {
//				SelectDropDown("Type", value);
//				m_type = value;
//			}
//		}
//
//		public bool ShowInPlanner {
//			get {
//				return m_showInPlanner.Value.IsChecked;
//			}
//			set {
//				Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//				m_showInPlanner.Value.IsChecked = value;
//			}
//		}
//
//		public DateTime DueDate {
//			get {
//				return m_time;
//			}
//			set {
//				TapAndWait(Query.DueDate, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1.5));
//				m_calendar.Value.Year = value.Year;
//				m_calendar.Value.Month = value.ToString("MMMM");
//				m_calendar.Value.Day = value.Day;
//				m_time = value;
//			}
//		}
//
//		public new void Done() {
//			base.Done();
//			AssertElementExists("view:'Dendrite.IPhone.Forms.CdlBaseTitle+TitleLabel' marked:'To Do' index:0");
//		}
//	}

//	public class MarketingRequestPopover : DropShadowPopover {
//		private string m_requestType;
//		private int m_product;
//		private Lazy<Calendar> m_calendar;
//		private int m_quantity;
//		private int m_location;
//		private string m_notes;
//		private DateTime m_time;
//
//		private static class Query {
//			internal static string Loaded = "UINavigationBar marked:'Marketing Request'";
//			internal static string Popover = "view:'_UIPopoverView'";
//			internal static string ExpectedDate = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'DATA:vt_triggered_activity/expected_delivery_date'";
//		}
//
//		internal MarketingRequestPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//			m_calendar = new Lazy<Calendar>(() => CalendarFactory(Query.Popover));
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(2)); // step pause
//				return TestIsVisible(Query.Loaded);
//			}
//		}
//
//		private Calendar CalendarFactory(string query) {
//			return new Calendar(Application, this, Container.GetDescendant(query));
//		}
//
//		public string RequestType {
//			get {
//				return m_requestType;
//			}
//			set {
//				SelectDropDown("Request Type", value);
//				m_requestType = value;
//			}
//		}
//
//		// Here this is public int so you can set the product by index instead of long product names
//		// This can easily be changed to your liking
//		public int Product {
//			get {
//				return m_product;
//			}
//			set {
//				SelectDropDown("Product", value, postTimeout: TimeSpan.FromSeconds(0.5));
//				m_product = value;
//			}
//		}
//
//		public int Quantity {
//			get {
//				return m_quantity;
//			}
//			set {
//				AddText("Quantity", value.ToString(), clearText: true, tapActionKey: true);
//				m_quantity = value;
//			}
//		}
//
//		// Here this is public int so you can set the product by index instead of long location names
//		// This can easily be changed to your liking
//		public int Location {
//			get {
//				return m_location;
//			}
//			set {
//				SelectDropDown("Location", value, postTimeout: TimeSpan.FromSeconds(1.5));
//				m_location = value;
//			}
//		}
//
//		public string Notes {
//			get {
//				return m_notes;
//			}
//			set {
//				AddText("Notes", value);
//				m_notes = value;
//			}
//		}
//
//		public DateTime ExpectedDeliveryDate {
//			get {
//				return m_time;
//			}
//			set {
//				TapAndWait(Query.ExpectedDate, () => TestIsVisible(Query.Popover), postTimeout: TimeSpan.FromSeconds(1.5));
//				m_calendar.Value.Year = value.Year;
//				m_calendar.Value.Month = value.ToString("MMMM");
//				m_calendar.Value.Day = value.Day;
//				m_time = value;
//			}
//		}
//
//		public new void Done() {
//			base.Done();
//			AssertElementExists("* marked:'Loyalty Card' index:0");
//		}
//	}
//
//	public class NonProfiledAttendeesPopover : Popover {
//		private string m_title;
//		private string m_name;
//		private string m_role;
//		private string m_notes;
//		private static class Query {
//			internal static string Loaded = "view:'_UIPopoverView'";
//			internal static string NewStaff = "UINavigationBar marked:'New Staff'";
//			internal static string Done = "UINavigationButton marked:'Done'";
//		}
//
//		internal NonProfiledAttendeesPopover(MITouch application, AppContainer container) 
//			: base(application, container) {
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//				return TestIsVisible(Query.NewStaff); 
//			}
//		}
//
//		public string Title {
//			get {
//				return m_title;
//			}
//			set {
//				string correctString = value.Replace("'", @"\'"); // Escape single quote
//				SelectDropDown("Title", correctString, postTimeout: TimeSpan.FromSeconds(1));
//				m_title = correctString;
//			}
//		}
//
//		public string Name {
//			get {
//				return m_name;
//			}
//			set {
//				AddText("Name", value, tapActionKey: true);
//				m_name = value;
//			}
//		}
//
//		public string Role {
//			get {
//				return m_role;
//			}
//			set {
//				SelectDropDown("Role", value, postTimeout: TimeSpan.FromSeconds(1));
//				m_role = value;
//			}
//		}
//
//		public string Notes {
//			get {
//				return m_notes;
//			}
//			set {
//				AddText("Notes", value, tapActionKey: true);
//				m_notes = value;
//			}
//		}
//
//		public new void Done() {
//			Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//			TapAndWait(Query.Done, () => TestIsVisible("view marked:'1 Staff Selected' index:0"), postTimeout: TimeSpan.FromSeconds(1));
//			base.Done();
//			AssertElementExists("view marked:'PA' index:0");
//		}
//	}

//	public class ExpensesPopover : Popover {
//		private string m_product;
//		private string m_type; 
//		private string m_allocation;
//		private int m_amount;
//		private string m_reason;
//		private Lazy<DefaultCheckBox> m_overallTradeSecret;
//		private static class Query {
//			internal static string Loaded = "view:'_UIPopoverView'";
//			internal static string TableViewCell = "UITableViewCell";
//			internal static string Next = "UINavigationButton marked:'Next'";
//			internal static string Previous = "UINavigationButton marked:'Previous'";
//			internal static string TradeSecret = "view:'Dendrite.IPhone.Forms.CdlEditItem' id:'__tradeSecretChkBox'";
//		}
//
//		internal ExpensesPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//			m_overallTradeSecret = new Lazy<DefaultCheckBox>(() => DefaultCheckBoxFactory(Query.TradeSecret));
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(1.5)); // step pause
//				return TestIsVisible(Query.Loaded);
//			}
//		}
//
//		private DefaultCheckBox DefaultCheckBoxFactory(string query) {
//			return new DefaultCheckBox(Application, this, Container.GetDescendant(query));
//		}
//
//		public string Product {
//			get {
//				return m_product;
//			}
//			set {
//				string itemQuery = string.Format(Query.TableViewCell + " descendant view marked:'{0}' index:0", value);
//				string checkMark = itemQuery + " parent UITableViewCell descendant UIImageView index:0";
//				TapAndWait(itemQuery, () => TestIsVisible(checkMark));
//			}
//		}
//
//		// Have to select product and tap next first
//		public string Type {
//			get {
//				return m_type;
//			}
//			set {
//				string itemQuery = string.Format(Query.TableViewCell + " descendant view marked:'{0}'", value);
//				// This click proceeds to next selector
//				TapAndWait(itemQuery, () => !TestIsVisible(itemQuery));
//			}
//		}
//
//		public string Allocation {
//			get {
//				return m_allocation;
//			}
//			set {
//				string itemQuery = string.Format(Query.TableViewCell + " descendant view marked:'{0}'", value);
//				string checkMark = itemQuery + " parent UITableViewCell descendant UIImageView index:0";
//				TapAndWait(itemQuery, () => TestIsVisible(checkMark));
//			}
//		}
//
//		// Have to select allocation then tap next first
//		public int Amount {
//			get {
//				return m_amount;
//			}
//			set {
//				AddText("Amount", value.ToString(), clearText: true, tapActionKey: true);
//				m_amount = value;
//			}
//		}
//
//		public bool OverallTradeSecret {
//			get {
//				return m_overallTradeSecret.Value.IsChecked;
//			}
//			set {
//				Thread.Sleep(TimeSpan.FromSeconds(0.5)); // step pause
//				m_overallTradeSecret.Value.IsChecked = value;
//			}
//		}
//
//		public string Reason {
//			get {
//				return m_reason;
//			}
//			set {
//				AddText("Reason", value, clearText: true, tapActionKey: true);
//				m_reason = value;
//				Thread.Sleep(TimeSpan.FromSeconds(1)); // step pause
//			}
//		}
//
//		public void Next() {
//			var initialQuery = Calabash.Query("*");
//			TapAndWait(Query.Next, () => initialQuery != Calabash.Query("*"));
//			Thread.Sleep(TimeSpan.FromSeconds(0.8)); // step pause
//		}
//
//		public new void Done() {
//			base.Done();
//			AssertElementExists("view:'Dendrite.IPhone.Forms.CdlBaseTitle+TitleLabel' marked:'Expenses' index:0");
//		}
//	}

//	public class SampleDisbursedPopover : Popover {
//		private string m_product;
//		private string m_type;
//		private string m_allocation;
//		private int m_amount;
//		private string m_sampleName;
//		private string m_lotNumber;
//		private string m_quantity;
//		private Lazy<DefaultCheckBox> m_overallTradeSecret;
//		private static class Query {
//			internal static string Loaded = "view:'_UIPopoverView'";
//			internal static string TableViewCell = "UITableViewCell";
//			internal static string Next = "UINavigationButton marked:'Next'";
//			internal static string Previous = "UINavigationButton marked:'Previous'";
//			internal static string SearchField = "UIFieldEditor";
//			internal static string Lot = "UINavigationBar marked:'Lot'";
//			internal static string Quantity = "UINavigationBar marked:'Quantity'";
//		}
//
//		internal SampleDisbursedPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//			m_sampleName = CallPage.SampleProduct.First["display_name"].ToString();
//			m_lotNumber = CallPage.SampleProduct.First["lot_number"].ToString();
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(1.5)); // step pause
//				return TestIsVisible(Query.Loaded);
//			}
//		}
//
//		public void AddSampleFromDatabase() {
//			SetField(Query.SearchField, m_sampleName);
//			Calabash.Tap(CalabashButton.Enter);
//			string itemQuery = string.Format(Query.TableViewCell + " marked:'KEY:{0}' index:0", m_sampleName);
//			Wait(() => TestIsVisible(itemQuery), postTimeout: TimeSpan.FromSeconds(1));
//			TapAndWait(itemQuery, () => TestIsVisible(Query.Lot), postTimeout: TimeSpan.FromSeconds(0.5));
//			string lotQuery = Query.TableViewCell + " index:0";
//			TapAndWait(lotQuery, () => TestIsVisible(Query.Quantity), postTimeout: TimeSpan.FromSeconds(0.5));
//			m_quantity = "5";
//			string quantityQuery = "UIButton marked:'5'";
//			string quantityVerificationQuery = "UITextFieldLabel marked:'5' index:0";
//			TapAndWait(quantityQuery, () => TestIsVisible(quantityVerificationQuery));
//		}
//
//		public new void Done() {
//			base.Done();
//			string sampleQuery = string.Format("view marked:'{0}' index:0", m_sampleName);
//			//string lotQuery = string.Format("view marked:'{0}' index:0", m_lotNumber);
//			string quantityQuery = string.Format("view marked:'{0}' index:0", m_quantity);
//			AssertElementExists(sampleQuery);
//			//AssertElementExists(lotQuery);
//			AssertElementExists(quantityQuery);
//		}
//	}
//
//	public class CallDetailsPopover : DropShadowPopover {
//		private int m_duration;
//		private string m_accompaniedSignature;
//		private static class Query {
//			internal static string CallDetailsNavBar = "UINavigationBar id:'Call Details'";
//		}
//
//		internal CallDetailsPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//			m_accompaniedSignature = Calabash.CheckDMSignature();
//		}
//
//		public override bool IsLoaded {
//			get {
//				Thread.Sleep(TimeSpan.FromSeconds(1.5)); // step pause
//				return TestIsVisible(Query.CallDetailsNavBar);
//			}
//		}
//
//		public int Duration {
//			get {
//				return m_duration;
//			}
//			set {
//				AddText("Duration (Min)", value.ToString(), clearText: true, tapActionKey: true);
//				m_duration = value;
//			}
//		}
//	}
}