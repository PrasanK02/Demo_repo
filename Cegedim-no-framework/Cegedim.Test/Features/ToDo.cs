using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
//using Cegedim.Automation;
using Cegedim.Automation;

namespace Cegedim {

    [TestFixture()]
    public class ToDo {
        IApp m_miTouch;

		private IApp app;

		[SetUp]
		public void Setup() {
			app = ConfigureApp
				.iOS
				.ApiKey("3250166bbe78a884fbd6089582a8df7c")
				.InstalledApp("com.cegedim.mi7")
				.StartApp();
		}
        // Method names here matter because of the test ordering
        [Test()]
        public void A_MakeTodoItem() {
			var loginPage = new NewLoginPage((iOSApp)app);
			loginPage.IsLoaded();
			app.Screenshot("I'm on the login page");

			loginPage.Username = "jmayo";
			loginPage.Password = "cegedim";
			app.Screenshot("I've entered valid credentials");
			loginPage.SubmitCredentials();

            var todoPage = Background();
            todoPage.DeleteAllTodosWithSubject("Present Test Cloud");
            app.Screenshot("I delete all todos with subject 'Present Test Cloud'");

            var todoPopover = todoPage.CreateTodoPopover();
            app.Screenshot("I tap the add a todo item button and see the To do dialog");

            todoPopover.Subject = "Present Test Cloud";
            app.Screenshot("I set the item subject to 'Present Test Cloud'");

            todoPopover.Description = "make sure we cover all the gotchas";
            app.Screenshot("I set the item description to 'make sure we cover all the gotchas'");

            todoPopover.DueDate = NextTuesday();
            app.Screenshot("I set the due date for next tuesday");

            todoPopover.Reminder = "30 Minutes";
            app.Screenshot("I want to be reminder 30 minutes before the meeting");

            todoPopover.Channel = "Face to Face";
            app.Screenshot("I set the todo channel to Face to Face");

            todoPopover.AddAssigneeFromDatabase();
            app.Screenshot("I assign the task to a teammate");

            var todoPageRevisited = todoPopover.DoneReturnToTodoPage();
            todoPageRevisited.ConfirmTodoItem("Present Test Cloud");
            app.Screenshot("I'm done creating the todo item and I see it in the Todos list");
        }

        [Test]
        public void ChangeTheTodoChannel() {
//            var todoPage = Background();
//            todoPage.TodoType = "Received";
//            todoPage.ConfirmTodoItem("Present Test Cloud");
//            m_miTouch.Screenshot("There is a 'Present Test Cloud' todo item in my received list");
//
//            var editTodoPopover = todoPage.EditTodoPopover("Present Test Cloud");
//            editTodoPopover.Channel = "Fax";
//            var todoPageRevisited = editTodoPopover.DoneReturnToTodoPage();
//            todoPageRevisited.ConfirmTodoItem("Present Test Cloud");
//            m_miTouch.Screenshot("I edit the todo item and change the channel to Fax");
//
//            todoPageRevisited.MoreDetails();
//            todoPageRevisited.ConfirmDetail("Fax");
//            m_miTouch.Screenshot("I should see the channel changed in the todo details");
        }

        [Test]
        public void DeleteTodoItem() {
			//app.Repl();
			var loginPage = new NewLoginPage((iOSApp)app);
			loginPage.IsLoaded();
			app.Screenshot("I'm on the login page");

			loginPage.Username = "jmayo";
			loginPage.Password = "cegedim";
			app.Screenshot("I've entered valid credentials");

			loginPage.SubmitCredentials();

            var todoPage = Background();
            todoPage.TodoType = "Received";
            todoPage.ConfirmTodoItem("Present Test Cloud");
            app.Screenshot("There is a 'Present Test Cloud' todo item in my received list");

            todoPage.DeleteTodoItem("Present Test Cloud");
            app.Screenshot("I delete the current todo item");

            todoPage.TodoType = "Received";
            app.Screenshot("I select the received todo type");

            todoPage.ConfirmNoTodoItem("Present Test Cloud");
            app.Screenshot("I shouldnt see the todo item");
        }

        public NewTodoPage Background() {
            //m_miTouch = Globals.App;
            //var dashboard = Globals.QuickSetUp();
			var dashboard = new NewDashboardPage(app);

            app.Screenshot("I'm on the dashboard page");

            var todoPage = dashboard.NavigateToTodoPage();
            app.Screenshot("I am on the todo page");

            return todoPage;
        }

        public DateTime NextTuesday() {
            var currentDateTime = DateTime.Now;
            var daysUntilNextTuesday = ((int)DayOfWeek.Tuesday - (int)currentDateTime.DayOfWeek + 7) % 7;
            if (daysUntilNextTuesday == 0)
                daysUntilNextTuesday = 7;
            // Set Next Tuesday at 11:30 AM
            var nextTuesday = currentDateTime.AddDays((double)daysUntilNextTuesday).Date + new TimeSpan(11, 30, 0);
            return nextTuesday;
        }
    }
}

