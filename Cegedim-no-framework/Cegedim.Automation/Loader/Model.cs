using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using Xamarin.Automation.Reflection;
using Xamarin.Automation.Reflection.Emit;
using Xamarin.Automation.Reflection.Metadata;

namespace Cegedim.Automation.Experimental {

    public sealed class MITouch : App {
        private static AppEntityMetadata LoadEntities() {
            var types = new[] {
                typeof(LoginPage)
            };

            var bindingFlags = 
                BindingFlags.Static | 
                BindingFlags.NonPublic |
                BindingFlags.InvokeMethod;

            return new AppEntityMetadata(
                runtimeTypes: types,
                factory: factory =>
                    (AppEntityInfo)factory.RuntimeType.InvokeMember(
                        "Load", bindingFlags, Type.DefaultBinder, null, new[] { factory })
            );
        }

        //private static readonly string BinaryName = "MI-10.3.0016.resigned.ipa";
        private static readonly string BundleName = "com.cegedim.mi7";
        public static User primaryUser;

        public static MITouch Launch(string ipAddress = null) {
            var server = CalabashServer.IOSLaunch(
                null, //BinaryName,
                BundleName,
                ipAddress
            );

            primaryUser = new User((CalabashServer)server);
            return new MITouch(server);
        }
        public static MITouch Attach(string ipAddress = null) {
            var server = CalabashServer.IOSAttach(ipAddress);
            return new MITouch(server);
        }

        private MITouch(AppServer server)
            : base(server, LoadEntities()) {
        }

        public ILoginPage GetLoginPage() {
            return AppConvention.ActivateAndWait<LoginPage>(this);
        }
    }
    
    public sealed class LoginPage : AppEntity, ILoginPage {

        private static class Query {
            internal static readonly AppQuery LoginButton = new AppQueryRaw("view:'MI.Login+LoginSource+LoginButton'");
            internal static readonly AppQuery UserNameField = new AppQueryRaw("view:'UITextField' index:0");
            internal static readonly AppQuery PasswordField = new AppQueryRaw("view:'UITextField' index:1");
        }
        private static class Member {
            internal static readonly PropertyInfo UserName = typeof(LoginPage).GetProperty("UserName");
            internal static readonly PropertyInfo Password = typeof(LoginPage).GetProperty("Password");
            internal static readonly PropertyInfo IsLoaded = typeof(LoginPage).GetProperty("IsLoaded");
            internal static readonly MethodInfo SubmitCredentials = typeof(LoginPage).GetMethod("SubmitCredentials");
        }

        private static AppEntityInfo Load(AppEntityInfoFactory factory) {
            return factory.Build(
                loadedQuery: Query.LoginButton,
                fields: LoadFields(),
                tests: LoadTests(),
                buttons: LoadButtons()
            );
        }
        private static AppTestMetadata LoadTests() {
            return new AppTestMetadata(
                runtimeProperties: new[] { 
                    Member.IsLoaded
                },
                factory: o => {

                    // IsLoaded
                    if (o.RuntimeProperty == Member.IsLoaded)
                        return o.Build(Query.LoginButton);

                    return null;
                }
            );
        }
        private static AppFieldMetadata LoadFields() {
            return new AppFieldMetadata(
                runtimeProperties: new[] {
                    Member.UserName,
                    Member.Password
                },
                factory: o => {

                    // UserName
                    if (o.RuntimeProperty == Member.UserName)
                        return o.Build(Query.UserNameField);

                    // Password
                    if (o.RuntimeProperty == Member.Password)
                        return o.Build(Query.PasswordField);

                    return null;
                }
            );
        }
        private static AppButtonMetadata LoadButtons() {
            return new AppButtonMetadata(
                runtimeMethods: new[] { 
                    Member.SubmitCredentials
                },
                factory: o => {

                    // Button
                    if (o.RuntimeMethod == Member.SubmitCredentials)
                        return o.Build(Query.LoginButton, typeof(IHomePage));

                    return null;
                }
            );
        }

        internal LoginPage(MITouch application, AppContainer container)
            : base(application, null, container) {
        }

        public override bool IsLoaded {
            get { return Test(Info.GetTest(Member.IsLoaded)); }
        }
        public string UserName {
            get { return GetField(Info.GetField(Member.UserName)); }
            set { SetField(Info.GetField(Member.UserName), value); }
        }
        public string Password {
            get { return GetField(Info.GetField(Member.Password)); }
            set { SetField(Info.GetField(Member.Password), value); }
        }
        public IHomePage SubmitCredentials() {
            return (IHomePage)TapActivateAndWait(Info.GetButton(Member.SubmitCredentials));
        }
    }
    public sealed class HomePage : AppEntity, IHomePage {

        internal HomePage(MITouch application, AppContainer container)
            : base(application, null, application.Server) {
        }

        public override bool IsLoaded {
            get { throw new NotImplementedException(); }
        }
    }
}
