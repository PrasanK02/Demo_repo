using System;
using System.Linq;
using System.Threading;
using System.Drawing;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;
//using Xamarin.Automation;
//using Xamarin.Automation.Calabash;

namespace Cegedim.Automation
{
	public static class NewCegedimEntity{ 

		private static class Query {
			internal static string Keyboard = "view:'UIKeyboardAutomatic'";
			internal static string ScrollableColumn = "view:'Dendrite.IPhone.Forms.UIScrollableContent' index:0";
		}

		//public CegedimEntity(MITouch application, CegedimEntity parent, AppContainer container)
		//	: base(application, parent, container) {
		//}
		public static int default_time = 30;

		public static bool TestIsVisible(this IApp app, Func<AppQuery,AppQuery> act, int time = 30)
		{


	        app.WaitForElement(act, postTimeout: TimeSpan.FromSeconds(time));
			return true;
			
		}
		public static bool TapAndWait(this IApp app, Func<AppQuery,AppQuery> tapOnElement, Func<AppQuery,AppQuery> waitOnElement, int postTimeout = 30)
		{
			app.Tap(tapOnElement);
			return app.TestIsVisible(waitOnElement, postTimeout);
		}

		public static bool TapAndWaitForElementNotPresent(this IApp app, Func<AppQuery,AppQuery> tapOnElement, Func<AppQuery,AppQuery> waitOnElement, int postTimeOut = 30)
		{
				app.Tap(tapOnElement);
				return !app.TestIsVisible(waitOnElement, postTimeOut);
		}

		public static void SwipeDownUntil(this IApp app, string rawSwipeOnQuery, string rawFinalQuery, TimeSpan? timeout = null, 
			TimeSpan? postTimeout = null, double? ratio = null) {
			timeout = timeout ?? TimeSpan.FromSeconds(5);
			postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
			ratio = ratio ?? 0.96;
			if (!app.TestIsVisible(c => c.Raw(rawSwipeOnQuery)))
				return;
				
			var result = app.Query(c => c.Raw(rawSwipeOnQuery)).First();

			var rectangle = result.Rect;

			var centerX = rectangle.X + rectangle.Width / 2; 
			var height = rectangle.Height;

			var centerY = rectangle.Y + rectangle.Height / 2;
			var initialY = centerY + (height / 2.0 * ratio);
			var finalY = centerY - (height / 2.0 * ratio);
			Func<bool> scrollTo = () => {
					if (app.TestIsVisible(c => c.Raw(rawFinalQuery))) {
					return true;
				}
				else {
					app.DragCoordinates((float)centerX, (float)initialY, (float)centerX, (float)finalY);
						return app.TestIsVisible(c => c.Raw(rawFinalQuery));
				}
			};
			app.WaitFor(scrollTo, timeout: timeout);
			Thread.Sleep((TimeSpan)postTimeout);
		}

		public static void SwipeDownUntil(this IApp app, string rawSwipeOnQuery, Func<bool> predicate, TimeSpan? timeout = null, 
			TimeSpan? postTimeout = null, double? ratio = null) {
			timeout = timeout ?? TimeSpan.FromSeconds(8);
			postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
			ratio = ratio ?? 0.96;
			if (!app.TestIsVisible(c => c.Raw(rawSwipeOnQuery)))
				return;

			var result = (app.Query(c => c.Raw(rawSwipeOnQuery)).First());

			var rectangle = result.Rect;

			var centerX = rectangle.X + rectangle.Width / 2;
			var height = rectangle.Height;

			var centerY = rectangle.Y + rectangle.Height / 2;
			var initialY = centerY + (height / 2.0 * ratio);
			var finalY = centerY - (height / 2.0 * ratio);
			Func<bool> scrollTo = () => {
				if (predicate.Invoke()) {
					return true;
				} else {
					app.DragCoordinates((float)centerX, (float)initialY, (float)centerX, (float)finalY);
					return predicate.Invoke();
				}
			};
			app.WaitFor(scrollTo, timeout: timeout);
			Thread.Sleep((TimeSpan)postTimeout);
		}

		public static void SwipeUpUntil(this IApp app, string rawSwipeOnQuery, string rawFinalQuery, TimeSpan? timeout = null, 
			TimeSpan? postTimeout = null, double? ratio = null) {
			timeout = timeout ?? TimeSpan.FromSeconds(8);
			postTimeout = postTimeout ?? TimeSpan.FromSeconds(0.5);
			ratio = ratio ?? 0.96;
			if (!app.TestIsVisible(c => c.Raw(rawSwipeOnQuery)))
				return;
				
			var result = app.Query(c => c.Raw(rawSwipeOnQuery)).First();

			var rectangle = result.Rect;

			var centerX = rectangle.X + rectangle.Width / 2;
			var height = rectangle.Height;

			var centerY = rectangle.Y + rectangle.Height / 2;
			var finalY = centerY + (height / 2.0 * ratio);
			var initialY = centerY - (height / 2.0 * ratio);
			Func<bool> scrollTo = () => {
			    if (app.TestIsVisible(c => c.Raw(rawFinalQuery))) {
					return true;
				} else {
					app.DragCoordinates((float)centerX, (float)initialY, (float)centerX, (float)finalY);
					return app.TestIsVisible(c => c.Raw(rawFinalQuery));
				}
			};
			app.WaitFor(scrollTo, timeout: timeout);
			Thread.Sleep((TimeSpan)postTimeout);
		}

		public static bool IsKeyboardVisible(this IApp app) {
			// Test Is Visible will issue the query with respect to the container
			// This will break IsKeyboardVisible since its not the child of a lot of containers
			// return TestIsVisible(Query.Keyboard);
			return app.Query(c => c.Raw(Query.Keyboard)).Count() > 0;
		}

		// Quick hacky solution to hide keyboard now
		public static void HideKeyboard(this IApp app) {
			if (app.IsKeyboardVisible()) {
				//                int keysIndex = Container.Descendants("view:'UIKBKeyView'").Count();
				//                keysIndex -= 1;
				//                string query = string.Format("UIKBKeyView index:'{0}'", keysIndex);
				//TapAndWait(query, () => !IsKeyboardVisible());
				string script = "uia.keyboard().buttons()['Hide keyboard'].tap()";
				((iOSApp)app).InvokeUia(script);
			}
		}
	}
}
