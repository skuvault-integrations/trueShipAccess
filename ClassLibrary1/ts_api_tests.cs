using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueShip.TrueShip_Access;
using TrueShipConfiguration;

namespace TrueShipAPITests
{
    public class TrueShipTests
    {

        #region createchannelconfig
        private static TrueShipConfiguration.tsConfiguration getConfig()
        {
            var config = new TrueShipConfiguration.tsConfiguration()
            {
                BEARERTOKEN = "1dde9c91fe72fd4168dc403d79b09b7d",
                LASTORDERSYNC = Convert.ToDateTime("2015-01-01T00:00:00"),
                LASTLOCATIONSYNC = Convert.ToDateTime("2015-01-01T00:00:00"),
                COMPANYID = 1
            };
            return config;
        }
        #endregion

        static void Main()
        {

            #region startlogger
            TSLogger.Logger logservice = new TSLogger.Logger();
            logservice.clearLogs();
            #endregion
            #region loadcompany
            logservice.tsLogNoLineBreak("Loading Company Configuration...");
            var ts_config = (tsConfiguration)getConfig();
            #endregion
            #region startsyncs
            logservice.tsLogNoLineBreak("Creating TrueShip Controller...");
            var tsBasicServices = new TruesShipBasicServices();
            var tsOrderServices = new TrueShipOrderSync();
            var tsLocationServices = new TrueShipLocationSync();
            #endregion

            logservice.tsLogLineBreak("Beginning API Calls...");
            var fullModule = true;
            var verifyUpdates = true;
            
            if (fullModule == true)
            {
                #region tsBasicAPICalls
                logservice.tsLogNoLineBreak("Get the number of remaining orders.");
                var remainingOrders = tsBasicServices.GetRemainingOrders(ts_config.BEARERTOKEN, ts_config.COMPANYID);
                logservice.tsLogLineBreak(string.Format("API Calls Remaining: {0}", remainingOrders.remaining_orders));

                //DOESN'T WORK, RETURNS 401 UNAUTHORIZED.  I bet if I specified an account id it would work, but I can't get one.
                //logservice.tsLogNoLineBreak("Get a list of accounts.");
                //tsBasicServices.GetAccounts(ts_config.BEARERTOKEN);

                logservice.tsLogNoLineBreak("Get a list of companies.");
                var someCompanies = tsBasicServices.GetCompanies(ts_config.BEARERTOKEN);
                foreach (var company in someCompanies)
                {
                    logservice.tsLogNoLineBreak(string.Format("Company Id: {0}", company.company_id));
                    logservice.tsLogNoLineBreak(string.Format("Created At: {0}", company.created_at));
                    logservice.tsLogNoLineBreak(string.Format("Id: {0}", company.id));
                    logservice.tsLogNoLineBreak(string.Format("Company Name: {0}", company.name));
                    logservice.tsLogLineBreak(string.Format("Updated At: {0}", company.updated_at));
                }
                #endregion

                #region ordersync
                logservice.tsLogNoLineBreak("Get one order by id.");
                var orderOne = tsOrderServices.GetOrderById(ts_config.BEARERTOKEN, "TRUE000001");
                logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", orderOne.primary_id));
                logservice.tsLogLineBreak(string.Format("Status Shipped: {0}", orderOne.status_shipped.ToString()));

                logservice.tsLogNoLineBreak("Get a second order by id.");
                var orderTwo = tsOrderServices.GetOrderById(ts_config.BEARERTOKEN, "TRUE000002");
                logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", orderTwo.primary_id));
                logservice.tsLogLineBreak(string.Format("Status Shipped: {0}", orderTwo.status_shipped.ToString()));

                logservice.tsLogNoLineBreak("Get a list of orders after a certain date.");
                var ordersDict = tsOrderServices.getOrders(ts_config.BEARERTOKEN, ts_config.COMPANYID, ts_config.LASTORDERSYNC);
                foreach (var order in ordersDict)
                {
                    logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", order.primary_id));
                    logservice.tsLogNoLineBreak(string.Format("Shipped: {0}", order.status_shipped.ToString()));
                }
                logservice.tsLogLineBreak("");
                #endregion

                #region locationsync
                logservice.tsLogNoLineBreak("Update A Few Orders With Locations.");
                var boxItemList = tsLocationServices.getUnshippedOrderItemsAfterDateTime(ts_config.BEARERTOKEN, ts_config.COMPANYID, "created_at", ts_config.LASTORDERSYNC);
                var boxItemUpdates = new List<KeyValuePair<string, TrueShip.Models.PickLocation>>();
                foreach (var oneboxitem in boxItemList)
                {
                    var pickLocation = new TrueShip.Models.PickLocation();
                    var somePickLocation = "wherever " + oneboxitem["part_number"] + " is!"; //Look up Sku locations
                    pickLocation.pick_location = somePickLocation; //Load Sku Locations into a Model
                    boxItemUpdates.Add(new KeyValuePair<string, TrueShip.Models.PickLocation>(oneboxitem["resource_uri"], pickLocation));
                }
                tsLocationServices.updateOrderItemPickLocations(ts_config.BEARERTOKEN, ts_config.COMPANYID, boxItemUpdates);
                #endregion
            }

            if (verifyUpdates == true)
            {
                #region verify updates occurred
                logservice.tsLogLineBreak("");
                logservice.tsLogNoLineBreak("Confirm that the updates were successful.");
                //var timespantolookforupdates = DateTime.Now.AddSeconds(-240);
                //var locationOrderUpdateConfirmation = tsOrderServices.getOrders(ts_config.BEARERTOKEN, ts_config.COMPANYID, timespantolookforupdates);
                var locationOrderUpdateConfirmation = tsOrderServices.getOrders(ts_config.BEARERTOKEN, ts_config.COMPANYID, ts_config.LASTORDERSYNC);
                foreach (var order in locationOrderUpdateConfirmation)
                {
                    logservice.tsLogNoLineBreak(string.Format("NEW ORDER: {0}", order.primary_id));
                    logservice.tsLogNoLineBreak(string.Format("Shipped: {0}", order.status_shipped.ToString()));
                    foreach (var orderitem in order.Boxes)
                    {
                        foreach (var orderitem2 in orderitem.boxes_items_dict)
                        {
                            logservice.tsLogNoLineBreak(string.Format("SKU: {0}", orderitem2.Value["part_number"]));
                            logservice.tsLogNoLineBreak(string.Format("Pick Location: {0}", orderitem2.Value["pick_location"]));
                        }
                    }
                }
                #endregion
            }

        }
    }
}