using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Automation;

namespace Cegedim.Automation.Experimental {

    public static class Extensions {
        public static IHomePage Login(this ILoginPage loginPage, string userName, string password) {
            loginPage.UserName = userName;
            loginPage.Password = password;
            var homePage = loginPage.SubmitCredentials();
            return homePage;
        }
    }
    public interface ILoginPage : IAppEntity {
        string UserName { get; set; }
        string Password { get; set; }
        //bool RememberMyLogin { get; set; }
        IHomePage SubmitCredentials();
    }

    public interface IHomePage : IAppEntity {
    }
}
