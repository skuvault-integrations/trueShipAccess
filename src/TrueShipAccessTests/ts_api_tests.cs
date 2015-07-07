using System;
using System.Collections.Generic;
using TrueShipAccess;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;

namespace TrueShipAccessTests
{
    public static class TrueShipTests
    {
	    private static TrueShipConfiguration getConfig()
        {
            var config = new TrueShipConfiguration(
                Convert.ToDateTime("2015-01-01T00:00:00"),
                Convert.ToDateTime("2015-01-01T00:00:00"));

            return config;
        }

	    private static TrueShipCredentials GetCredentials()
	    {
			return new TrueShipCredentials(1, "1dde9c91fe72fd4168dc403d79b09b7d");
	    }

	    static void Main()
        {
	        TrueShipLogger logservice = new TrueShipLogger();
            logservice.clearLogs();

	        logservice.tsLogNoLineBreak("Loading Company Configuration...");
            var tsConfig = getConfig();

	        logservice.tsLogNoLineBreak("Creating TrueShip Controller...");
			var trueShipFactory = new TrueShipFactory(getConfig());
			var trusShipSerivces = trueShipFactory.CreateService(GetCredentials());

			//var tsBasicServices = new TruesShipBasicServices(GetCredentials(), getConfig() );
			//var tsOrderServices = new TrueShipOrderSync(GetCredentials(), getConfig());
			//var tsLocationServices = new TrueShipLocationSync(GetCredentials(), getConfig());

	        logservice.tsLogLineBreak("Beginning API Calls...");
            var fullModule = true;
            var verifyUpdates = true;
            if (fullModule == true)
            {
                #region tsBasicAPICalls
                logservice.tsLogNoLineBreak("Get the number of remaining orders.");
                var remainingOrders = trusShipSerivces.GetRemainingOrders();
	            remainingOrders.Wait();
                logservice.tsLogLineBreak(string.Format("API Calls Remaining: {0}", remainingOrders.Result.remaining_orders));

                //DOESN'T WORK, RETURNS 401 UNAUTHORIZED.  I bet if I specified an account id it would work, but I can't get one.
                //logservice.tsLogNoLineBreak("Get a list of accounts.");
                //tsBasicServices.GetAccounts(ts_config.BEARERTOKEN);

                logservice.tsLogNoLineBreak("Get a list of companies.");
                var someCompanies = trusShipSerivces.GetCompanies(0);
	            someCompanies.Wait();
                foreach (var company in someCompanies.Result)
                {
                    logservice.tsLogNoLineBreak(string.Format("Company Id: {0}", company.company_id));
                    logservice.tsLogNoLineBreak(string.Format("Created At: {0}", company.created_at));
                    logservice.tsLogNoLineBreak(string.Format("Id: {0}", company.Id));
                    logservice.tsLogNoLineBreak(string.Format("Company Name: {0}", company.Name));
                    logservice.tsLogLineBreak(string.Format("Updated At: {0}", company.updated_at));
                }
                #endregion

                #region ordersync
                logservice.tsLogNoLineBreak("Get one order by id.");
                var orderOne = trusShipSerivces.GetOrder("TRUE000001");
	            orderOne.Wait();
                logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", orderOne.Result.PrimaryId));
                logservice.tsLogLineBreak(string.Format("Status Shipped: {0}", orderOne.Result.StatusShipped.ToString()));

                logservice.tsLogNoLineBreak("Get a second order by id.");
				var orderTwo = trusShipSerivces.GetOrder("TRUE000002");
	            orderTwo.Wait();

                logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", orderTwo.Result.PrimaryId));
                logservice.tsLogLineBreak(string.Format("Status Shipped: {0}", orderTwo.Result.StatusShipped));

                logservice.tsLogNoLineBreak("Get a list of orders after a certain date.");
				var ordersDict = trusShipSerivces.GetOrders(tsConfig.LastOrderSync);
	            ordersDict.Wait();
                foreach (var order in ordersDict.Result)
                {
                    logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", order.PrimaryId));
                    logservice.tsLogNoLineBreak(string.Format("Shipped: {0}", order.StatusShipped));
                }
                logservice.tsLogLineBreak("");

				logservice.tsLogNoLineBreak("Get a list of orders by date range");
	            var orders = trusShipSerivces.GetOrdersAsync(DateTime.MinValue, DateTime.MaxValue).Result;
				foreach (var order in orders)
				{
					logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", order.PrimaryId));
					logservice.tsLogNoLineBreak(string.Format("Shipped: {0}", order.StatusShipped));
				}
				logservice.tsLogLineBreak("");

                #endregion

                #region locationsync
                logservice.tsLogNoLineBreak("Update A Few Orders With Locations.");
                var boxItemList = trusShipSerivces.GetUnshippedOrderItemsAfterDateTime(GetCredentials().CompanyId, "created_at", tsConfig.LastOrderSync);
	            boxItemList.Wait();

                var boxItemUpdates = new List<KeyValuePair<string, PickLocation>>();
                foreach (var oneboxitem in boxItemList.Result)
                {
                    var pickLocation = new PickLocation();
                    var somePickLocation = "wherever " + oneboxitem.PartNumber + " is!"; //Look up Sku locations
                    pickLocation.pick_location = somePickLocation; //Load Sku Locations into a Model
                    boxItemUpdates.Add(new KeyValuePair<string, PickLocation>(oneboxitem.ResourceUri, pickLocation));
                }
                trusShipSerivces.UpdateOrderItemPickLocations(boxItemUpdates);
                #endregion
            }
            if (verifyUpdates == true)
            {
                #region verify updates occurred
                //var timespantolookforupdates = DateTime.Now.AddSeconds(-240);
                logservice.tsLogLineBreak("");
                logservice.tsLogNoLineBreak("Confirm that the updates were successful.");
                var locationOrderUpdateConfirmation = trusShipSerivces.GetOrders(tsConfig.LastOrderSync);
	            locationOrderUpdateConfirmation.Wait();
                foreach (var order in locationOrderUpdateConfirmation.Result)
                {
                    logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", order.PrimaryId));
                    logservice.tsLogNoLineBreak(string.Format("Shipped: {0}", order.StatusShipped.ToString()));
                    foreach (var orderitem in order.Boxes)
                    {
                        foreach (var orderitem2 in orderitem.Items)
                        {
                            logservice.tsLogNoLineBreak(string.Format("SKU: {0}", orderitem2.PartNumber));
                            logservice.tsLogNoLineBreak(string.Format("Pick Location: {0}", orderitem2.PickLocation));
                        }
                    }
                }
                #endregion
            }
        }
    }
}