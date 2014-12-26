using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Automation;
using Xamarin.Automation.Calabash;
using System.Threading.Tasks;

namespace Cegedim.Automation {

    public class User {
        private CalabashServer m_calabashServer;
        private string _employeeId = null;
        private string _teamId = null;
        private string _regionId = null;
        private string _businessUnitId = null;
        public string validPin { get; set; }
        public string employeeId {
            get {
                if (_employeeId == null) {
                    FetchUserDetails();
                    return _employeeId;
                }
                return _employeeId;
            }
            set {
                _employeeId = value;
            }
        }
        public string teamId {
            get {
                if (_teamId == null) {
                    FetchUserDetails();
                    return _teamId;
                }
                return _teamId;
            }
            set {
                _teamId = value;
            }
        }
        public string regionId {
            get {
                if (_regionId == null) {
                    FetchUserDetails();
                    return _regionId;
                }
                return _regionId;
            }
            set {
                _regionId = value;
            }
        }
        public string businessUnitId {
            get {
                if (_businessUnitId == null) {
                    FetchUserDetails();
                    return _businessUnitId;
                }
                return _businessUnitId;
            }
            set {
                _businessUnitId = value;
            }
        }
        public string username = "jmayo";

        internal User(CalabashServer calabash) {
            m_calabashServer = calabash;
        }

        public Dictionary<String, String> credentials() {
            var credentials = new Dictionary<string,string>();
            credentials.Add("username", "jmayo");
            credentials.Add("password", "cegedim");
            return credentials;
        }

        public void FetchUserDetails() {
            string results = m_calabashServer.SelectUserAccountDetails(this);
            JArray detailsArray = JArray.Parse(results);
            employeeId = detailsArray[0]["employee_id"].ToString();
            teamId = detailsArray[0]["team_id"].ToString();
            regionId = detailsArray[0]["region_id"].ToString();
            businessUnitId = detailsArray[0]["business_unit_id"].ToString();
        }
    }
}
