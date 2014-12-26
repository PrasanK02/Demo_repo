using System;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Cegedim.Automation {

	public static class NewDatabaseQueries {

		public static string NewSelectUserAccountDetails(this iOSApp app, User user) {
			string sqlString = String.Format("select team_id, employee_id, region_id, business_unit_id from alignment" +
				" where employee_id IN (select employee_id from employee e join user_account_authentication u" +
				" on e.user_account_id = u.user_account_id where u.authentication_id = \"{0}\" )", user.username);
			string results = app.Invoke("executeSql2:", sqlString).ToString();
			if(String.IsNullOrEmpty(results))
				throw new Exception("User details not found");
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectUserAccountDetails(this iOSApp app, string userName) {
			string sqlString = String.Format("select team_id, employee_id, region_id, business_unit_id from alignment" +
				" where employee_id IN (select employee_id from employee e join user_account_authentication u" +
				" on e.user_account_id = u.user_account_id where u.authentication_id = \"{0}\" )", userName);
			string results = app.Invoke("executeSql2:", sqlString).ToString();
			if(String.IsNullOrEmpty(results))
				throw new Exception("User details not found");
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectCountriesAndStates(this iOSApp app) {
			string sqlString0 = "select distinct (select description from pda_code_store where code = p.state and code_role = 'state') as state, ";
			string sqlString1 = "(select description from pda_code_store where code = p.country and code_role = 'country') as country from postal_area p";
			string sqlString = sqlString0 + sqlString1;
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectCities(this iOSApp app) {
			string sqlString = "select distinct city from postal_area";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectPostalCode(this iOSApp app) {
			string sqlString = "select distinct postal_area from postal_area";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectCallCustomers(this iOSApp app, int limitNumber) {
			string limit = limitNumber.ToString();
			string sqlString = "SELECT customer.name as Name,ca.alignment_id as alignmentid, customer.customer_id as " +
				"customerid from state_license sl inner join customer customer on sl.customer_id=customer.customer_id " + 
				"inner join customer_alignment ca on ca.customer_id = customer.customer_id " +
				"where ca.status!=0 AND customer.status='ACTV' AND customer.validation_status != 'INVL' ORDER BY " +
				"customer.name limit " + limit;
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectTeamMate(this iOSApp app, int limitNumber) {
			User user = MITouch.primaryUser;
			string limit = limitNumber.ToString();
			string sqlString = String.Format("SELECT employee.employee_id FROM employee WHERE (employee.status='ACTV' AND EXISTS (SELECT alignment.employee_id" +
				" FROM alignment, team WHERE alignment.employee_id" +
				" = employee.employee_id AND alignment.team_id in ('{0}') AND (team.team_id=alignment.team_id AND (team.region_id " +
				"IS NULL OR ( team.region_id IN ('{1}') )) AND (team.business_unit_id IS NULL OR ( team.business_unit_id IN (" +
				"'{2}') )) ))) And employee.employee_id != '{3}' order by employee.last_name, employee.first_name" + 
				" limit {4}", user.teamId, user.regionId, user.businessUnitId, user.employeeId, limit);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewTurnOnUseIndividualAsASpeaker(this iOSApp app) {
			string sqlString = "update pda_module_store set parameter_value = 1 where parameter = 'Use Individual as a Speaker' and module = 'call parameters'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSearchForEmployeeId(this iOSApp app) {
			string sqlString = "select employee_id from employee" +
				" inner join user_account on user_account.user_account_id = employee.user_account_id limit 1";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewTurnOffCheckForNegativeInventory(this iOSApp app) {
			string sqlString = "update PDA_module_store set parameter_value=0 where parameter='check for negative inventory'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewTurnOnOwnerEmployeeDropDown(this iOSApp app) {
			User user = MITouch.primaryUser;
			string sqlString = String.Format("update team set type='KOL' where team_id={0}", user.teamId);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectSampleProductWithLotNumber(this iOSApp app, int count) {
			string sqlString = String.Format("SELECT DISTINCT product.display_name,sample_transaction.lot_number FROM sample_transaction inner " +
				"join product on sample_transaction.product_id = product.product_id WHERE (sample_transaction.lot_number IS NOT NULL)" + 
				" ORDER BY PRODUCT.DISPLAY_NAME limit {0}", count);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectAvailableSampleRequestProduct(this iOSApp app, string alignmentId) {
			// alignmentId should be customer["alignmentid"]
			string sqlString = String.Format("SELECT PRODUCT.DISPLAY_NAME vc_product_name FROM product_alignment_inher_view vt_valid_samples_requested, product " +
				"WHERE (VT_VALID_SAMPLES_REQUESTED.ALIGNMENT_ID= {0} AND VT_VALID_SAMPLES_REQUESTED.PRODUCT_ALIGNMENT_TYPE='BRC' " +
				"AND product.product_id=vt_valid_samples_requested.product_id AND PRODUCT.CLIENT_PRODUCT<>'0') limit 1", alignmentId);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewCheckDMSignature(this iOSApp app) {
			string sqlString = "select parameter_value from PDA_module_store where parameter = 'accompanied_calls_require_a_signature'"
				+ " and module='call mi touch'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewRetrieveLatestEventId(this iOSApp app) {
			string sqlString = "SELECT event_id as eventid from event ORDER BY status_change_date desc limit 1";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectCustomerNames(this iOSApp app, int sampleSize) { 
			string sqlString = string.Format("select name || ', ' || first_name as display_name from customer cu " +
				"join customer_alignment ca on cu.customer_id = ca.customer_id where cu.status = 'ACTV' and ca.status = 1 "
				+ "and cu.customer_type = 'PRES' order by name, first_name limit {0}", sampleSize.ToString());
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectAttendeeNames(this iOSApp app, string customerName, int sampleSize) {
			string customers = app.NewSelectCustomerNames(sampleSize);
			Random rand = new Random();
			int sampleValue = rand.Next(0, sampleSize - 1);
			string sampledAttendee = JArray.Parse(customers)[sampleValue]["display_name"].ToString();
			if (sampledAttendee == customerName) {
				int newSampleValue = (sampleValue + 1) % (sampleSize - 1);
				sampledAttendee = JArray.Parse(customers)[newSampleValue]["display_name"].ToString();
			}
			return sampledAttendee;
		}

		public static string NewSelectOrderEntryCustomers(this iOSApp app, int limit) {
			string sqlString = string.Format("select name, first_name, customer_type from customer c where c.status= 'ACTV' and exists "
				+ "(select 1 from account_customer ac, account a where ac.customer_id = c.customer_id and a.account_id = ac.account_id and a.status = "
				+ "'ACTV') order by name, first_name limit {0}", limit.ToString());
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectInactiveOrderEntryCustomers(this iOSApp app, int limit) {
			string sqlString = string.Format("select name, first_name, customer_type from customer c where c.status= 'ACTV' and exists " 
				+ "(select 1 from account_customer ac, account a where ac.customer_id = c.customer_id and a.account_id = ac.account_id and a.status "
				+ "= 'INAC') order by name, first_name limit {0}", limit.ToString());
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewGetDefaultCustomerType(this iOSApp app) {
			string sqlString = "select parameter_value from pda_module_store where module like 'qksr' and parameter like 'dflt'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSetDefaultCustomerType(this iOSApp app, string parameterValue) {
			string sqlString = string.Format("update pda_module_store set parameter_value = '{0}' "
				+ "where module = 'qksr' and parameter like 'dflt'", parameterValue);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewResetServiceRuleStore(this iOSApp app) {
			return app.Invoke("resetServices:", "RuleStore").ToString();
		}

		public static string NewUpdateDatabaseSubmitOrderWithOutSignature(this iOSApp app) {
			string sqlString = "update pda_module_store set parameter_value = 0 where module = 'order entry' and parameter "
				+ "like 'signature_module'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewUpdateDatabaseSubmitOrderWithSignature(this iOSApp app) {
			string sqlString = "update pda_module_store set parameter_value = 1 where module = 'order entry' and parameter like 'signature_module'";
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectToDoAssignees(this iOSApp app, string teamId, int limit) {
			string sqlString = string.Format("SELECT last_name || ', ' || first_name as display_name FROM employee "
				+ "WHERE (employee.status='ACTV' AND EXISTS (SELECT employee_id FROM alignment, team "
				+ "WHERE alignment.employee_id = employee.employee_id AND (alignment.team_id = {0}))) limit {1}",
				teamId, limit.ToString());
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectRouteCustomers(this iOSApp app, int numberOfCustomers) {
			string sqlString = string.Format("select name || ', ' || first_name as full_name " +
				"from customer cu join customer_alignment ca on cu.customer_id = ca.customer_id " +
				"where cu.status = 'ACTV' and ca.status = 1 and cu.customer_type = 'PRES' " +
				"order by name, first_name limit {0}", numberOfCustomers.ToString());
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectRouteCustomers(this iOSApp app, string routeName) {
			string sqlString = string.Format("select name || ', ' || first_name as full_name " +
				"from customer_route cr join customer cu on cr.customer_id = cu.customer_id " +
				"join route ro on cr.route_id = ro.route_id where ro.description = '{0}' and (ro.deleted is null " +
				"or ro.deleted <> 1)", routeName);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectRoute(this iOSApp app, string routeName) {
			string sqlString = string.Format("select route_id from route where description = '{0}' and (deleted is null or deleted <> 1)",
				routeName);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectTodo(this iOSApp app, string subject) {
			string sqlString = string.Format("select todo_id from todo where subject = '{0}' and (status <> 'DELE') ", subject);
			return app.Invoke("executeSql2:", sqlString).ToString();
		}

		public static string NewSelectNotificationDetails(this iOSApp app) {
			return app.Invoke("getBusinessObjectData:", "VT_HOME_PRIMARYCARE_ALERTS").ToString();
		}
	}
}

