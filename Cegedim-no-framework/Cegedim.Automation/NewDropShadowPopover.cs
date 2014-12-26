using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using Xamarin.UITest;

namespace Cegedim.Automation {
	public class NewDropShadowPopover {
		public IApp app;
		private static class Query {
			internal static string Loaded = "UIDropShadowView";
			internal static string Popover = "view:'_UIPopoverView'";
			internal static string Done = "UINavigationButton marked:'Done'";
			internal static string Cancel = "UINavigationButton marked:'Cancel'";
			internal static string TextField = "view:'Dendrite.IPhone.Forms.CdlTextBox'";
			internal static string EditItem = "view:'Dendrite.IPhone.Forms.CdlEditItem'";
			internal static string ClearText = "UITextField isFirstResponder:1 child button";
			internal static string ClearDropDown = "UINavigationButton marked:'Clear'";
			internal static string DropDown = "view:'Dendrite.IPhone.Forms.CdlDropDownList'";
		}

		internal NewDropShadowPopover(IApp app)
		{
			this.app = app;
		}

		public bool IsLoaded {
			get { return app.TestIsVisible(x=>x.Raw(Query.Loaded)); }
		}

		public void Done() {
			app.TapAndWaitForElementNotPresent(x=>x.Raw(Query.Done), x=>x.Raw(Query.Loaded));
		}

		public void Cancel() {
			app.TapAndWaitForElementNotPresent(x=>x.Raw(Query.Cancel), x=>x.Raw(Query.Loaded));
		}

		public void AddText(string fieldName, string text, bool? clearText = null, bool tapActionKey = false,
			string textType = "TextField") {
			// textType can be TextField or TextBox
			if (textType == "TextField") {
				// Default TextField behavior is to clearText
				bool clearTextBool = clearText ?? true;
				string fieldQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
				if (!app.TestIsVisible(c => c.Raw(fieldQuery)))
					app.SwipeDownUntil(Query.Loaded, fieldQuery, postTimeout: TimeSpan.FromSeconds(2));
				Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
				app.Tap(c => c.Raw(fieldQuery));
				app.WaitFor (() => app.IsKeyboardVisible(), postTimeout: TimeSpan.FromSeconds(0.4));
				if (clearTextBool && app.TestIsVisible (c => c.Raw(Query.ClearText))) {
					app.Tap (c => c.Raw(Query.ClearText));
					app.WaitFor (() => !app.TestIsVisible (c => c.Raw(Query.ClearText)));
				}
				app.EnterText (c => c.Raw(fieldQuery), text);
			} else {
				throw new Exception("text field type not known. Choose TextField or TextBox");
			}
			if (tapActionKey)
				app.PressEnter ();
		}


		public void SelectDropDown(string fieldName, string itemName, bool clearExisting = true, 
			TimeSpan? timeout = null, TimeSpan? postTimeout = null) {
			timeout = timeout ?? TimeSpan.FromSeconds(10);
			postTimeout = postTimeout ?? TimeSpan.FromSeconds(10);
			string dropDownQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
			if (!app.TestIsVisible(c => c.Raw(dropDownQuery)))
				app.SwipeDownUntil(Query.Loaded, dropDownQuery, postTimeout: TimeSpan.FromSeconds(2));
			app.WaitFor(() => app.TestIsVisible(c => c.Raw(dropDownQuery)), postTimeout: TimeSpan.FromSeconds(0.3));
			app.TapAndWait(c => c.Raw(dropDownQuery), c => c.Raw(Query.Popover));
			if (app.TestIsVisible(c => c.Raw(Query.ClearDropDown)) && clearExisting) {
				app.Tap (c => c.Raw(Query.ClearDropDown));
				app.WaitFor (() => !app.TestIsVisible(c => c.Raw(Query.Popover)), postTimeout: TimeSpan.FromSeconds(1));
				app.TapAndWait(c => c.Raw(dropDownQuery), c => c.Raw(Query.Popover));
			}
			string itemQuery = string.Format("{0} descendant view marked:'{1}'", Query.Popover, itemName);
			if (!app.TestIsVisible(c => c.Raw(itemQuery)))
				app.SwipeDownUntil(Query.Popover, itemQuery, timeout: timeout);
			app.Tap(c => c.Raw(itemQuery));
			app.WaitFor (() => !app.TestIsVisible(c => c.Raw(Query.Popover)));
			Thread.Sleep(((TimeSpan)postTimeout));
		}

		public void SelectDropDown(string fieldName, int index, bool clearExisting = true,
			TimeSpan? timeout = null, TimeSpan? postTimeout = null) {
			timeout = timeout ?? TimeSpan.FromSeconds(10);
			postTimeout = postTimeout ?? TimeSpan.FromSeconds(10);
			string dropDownQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
			if (!app.TestIsVisible(c => c.Raw(dropDownQuery)))
				app.SwipeDownUntil(Query.Loaded, dropDownQuery, postTimeout: TimeSpan.FromSeconds(2));
			app.TapAndWait(c => c.Raw(dropDownQuery), c => c.Raw(Query.Popover));
			if (app.TestIsVisible(c => c.Raw(Query.ClearDropDown)) && clearExisting) {
				app.Tap (c => c.Raw(Query.ClearDropDown));
				app.WaitFor (() => !app.TestIsVisible(c => c.Raw(Query.Popover)));
				app.TapAndWait(c => c.Raw(dropDownQuery), c => c.Raw(Query.Popover));
			}
			string itemQuery = string.Format("{0} descendant UITableViewCell index:{1}", Query.Popover, index);
			if (!app.TestIsVisible(c => c.Raw(itemQuery)))
				app.SwipeDownUntil(Query.Popover, itemQuery, timeout: timeout);
			app.Tap(c => c.Raw(itemQuery));
			app.WaitFor (() => !app.TestIsVisible(c => c.Raw(Query.Popover)));
			Thread.Sleep(((TimeSpan)postTimeout));
		}
	}

	public class NewPopover {
		public IApp app;
		private static class Query {
			internal static string Loaded = "view:'_UIPopoverView'";
			internal static string Popover = "view:'_UIPopoverView'";
			internal static string Done = "UINavigationButton marked:'Done'";
			internal static string Cancel = "UINavigationButton marked:'Cancel'";
			internal static string TextField = "view:'Dendrite.IPhone.Forms.CdlTextBox'";
			internal static string EditItem = "view:'Dendrite.IPhone.Forms.CdlEditItem'";
			internal static string ClearText = "UITextField isFirstResponder:1 child button";
			internal static string ClearDropDown = "UINavigationButton marked:'Clear'";
			internal static string DropDown = "view:'Dendrite.IPhone.Forms.CdlDropDownList'";
		}

//		internal NewPopover(MITouch application, AppContainer container)
//			: base(application, container) {
//		}

		public NewPopover(IApp app)
		{
			this.app = app;
		}

		public bool IsLoaded {
			get { return app.TestIsVisible(x=>x.Raw(Query.Loaded)); }
		}

		public void Done() {
			app.TapAndWaitForElementNotPresent(x=>x.Raw(Query.Done), x => x.Raw(Query.Loaded));
		}

		public void Cancel() {
			app.TapAndWaitForElementNotPresent(x=>x.Raw(Query.Cancel), x => x.Raw(Query.Loaded));
		}

//		public void AddText(string fieldName, string text, bool? clearText = null, bool tapActionKey = false,
//			string textType = "TextField") {
//			// textType can be TextField or TextBox
//			if (textType == "TextField") {
//				// Default TextField behavior is to clearText
//				bool clearTextBool = clearText ?? true;
//				string fieldQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
//				if (!app.TestIsVisible(x=>x.Raw(fieldQuery)))
//					SwipeDownUntil(Query.Loaded, fieldQuery, postTimeout: TimeSpan.FromSeconds(2));
//				TapAndWait(fieldQuery, () => IsKeyboardVisible());
//				if (clearTextBool && TestIsVisible(Query.ClearText))
//					TapAndWait(Query.ClearText, () => !TestIsVisible(Query.ClearText));
//				SetField(fieldQuery, text);
//			} else {
//				throw new Exception("text field type not known. Choose TextField or TextBox");
//			}
//			if (tapActionKey)
//				Calabash.Tap(CalabashButton.Enter);
//		}
//
//
//		public void SelectDropDown(string fieldName, string itemName, bool clearExisting = true,
//			TimeSpan? timeout = null, TimeSpan? postTimeout = null) {
//			timeout = timeout ?? TimeSpan.FromSeconds(10);
//			postTimeout = postTimeout ?? TimeSpan.FromSeconds(10);
//			string dropDownQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
//			if (!TestIsVisible(dropDownQuery))
//				SwipeDownUntil(Query.Loaded, dropDownQuery, postTimeout: TimeSpan.FromSeconds(2));
//			Thread.Sleep(TimeSpan.FromSeconds(0.3));
//			TapAndWait(dropDownQuery, () => !TestIsVisible(Query.Cancel), postTimeout: TimeSpan.FromSeconds(1));
//			if (TestIsVisible(Query.ClearDropDown) && clearExisting) {
//				TapAndWait(Query.ClearDropDown, () => !TestIsVisible(Query.ClearDropDown), postTimeout: TimeSpan.FromSeconds(1));
//				TapAndWait(dropDownQuery, () => !TestIsVisible(Query.Cancel), postTimeout: TimeSpan.FromSeconds(0.5));
//			}
//			string itemQuery = string.Format("{0} descendant view marked:'{1}'", Query.Popover, itemName);
//			if (!TestIsVisible(itemQuery))
//				SwipeDownUntil(Query.Popover, itemQuery, timeout: timeout);
//			Thread.Sleep(TimeSpan.FromSeconds(0.3)); // step pause
//			TapAndWait(itemQuery, () => TestIsVisible(dropDownQuery));
//			Thread.Sleep(((TimeSpan)postTimeout));
//		}
//
//		public void SelectDropDown(string fieldName, int index, bool clearExisting = true,
//			TimeSpan? timeout = null, TimeSpan? postTimeout = null) {
//			timeout = timeout ?? TimeSpan.FromSeconds(10);
//			postTimeout = postTimeout ?? TimeSpan.FromSeconds(10);
//			string dropDownQuery = string.Format("{0} descendant view marked:'{1}' parent {2} index:0", Query.Loaded, fieldName, Query.EditItem);
//			if (!TestIsVisible(dropDownQuery))
//				SwipeDownUntil(Query.Loaded, dropDownQuery, postTimeout: TimeSpan.FromSeconds(2));
//			TapAndWait(dropDownQuery, () => !TestIsVisible(Query.Cancel), postTimeout: TimeSpan.FromSeconds(0.5));
//			if (TestIsVisible(Query.ClearDropDown) && clearExisting) {
//				TapAndWait(Query.ClearDropDown, () => !TestIsVisible(Query.ClearDropDown));
//				TapAndWait(dropDownQuery, () => !TestIsVisible(Query.Cancel), postTimeout: TimeSpan.FromSeconds(0.5));
//			}
//			string itemQuery = string.Format("{0} descendant UITableViewCell index:{1}", Query.Popover, index);
//			if (!TestIsVisible(itemQuery))
//				SwipeDownUntil(Query.Popover, itemQuery, timeout: timeout);
//			TapAndWait(itemQuery, () => TestIsVisible(dropDownQuery));
//			Thread.Sleep(((TimeSpan)postTimeout));
//		}
	}
}
