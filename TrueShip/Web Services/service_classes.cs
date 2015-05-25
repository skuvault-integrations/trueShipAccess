using System;
using System.Collections.Generic;
using TrueShip.TSWebHelper;
using TrueShip.Models;
using System.Web.Script.Serialization;
using System.Web;
using System.Net;
using System.Text;
//using TSLogger;

namespace TrueShip.TrueShip_Access
{

    #region basicservices
    public class TruesShipBasicServices
    {
        TrueShip.TSWebHelper.WebHelper tsWebHelper = new WebHelper();
        //TSLogger.Logger logrunner = new TSLogger.Logger();
        string FORMAT = "JSON";

        #region getremainingorders
        public RemainingOrdersResource GetRemainingOrders(string bearertoken, int id)
        {
            #region localvariables
            string APIENDPOINT = "remaining_orders";
            string querystring = string.Format("bearer_token={1}",
                id.ToString(),
                bearertoken);
            #endregion
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            var remainingorders = new RemainingOrdersResource();
            if (jsonresponse != null)
            {
                remainingorders.remaining_orders = jsonresponse["remaining_orders"];
                remainingorders.resource_uri = jsonresponse["resource_uri"];
                return remainingorders;
            }
            else { return remainingorders; }
        }
        #endregion
        
        #region getaccounts  //THIS CALL DOES NOT WORK, BUT IT IS MOSTLY USELESS ANYWAY
        public Boolean GetAccounts(string bearertoken)
        {
            #region localvariables
            int LIMIT = 100;
            int OFFSET = 0;
            string logthis;
            #endregion
            string APIENDPOINT = "accounts";
            string querystring = string.Format("limit={0}&bearer_token={1}&offset={2}",
                LIMIT.ToString(),
                bearertoken,
                OFFSET.ToString());
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            if (jsonresponse != null)
            {
                foreach (KeyValuePair<string, dynamic> dictItem in jsonresponse)
                {
                    logthis = (string)dictItem.Key + ": ";
                    logthis = logthis + dictItem.Value.ToString();
                    //logrunner.tsLogNoLineBreak(logthis);
                }
                return true;
            }
            else { return false; }
        }
        #endregion

        #region getcompanies
        public CompaniesResource.Companies GetCompanies(string bearertoken)
        {   //Accounts with only 1 company must use this call
            #region localvariables
            int LIMIT = 50;
            int OFFSET = 0;
            string APIENDPOINT = "company";
            string querystring = string.Format(string.Format("bearer_token={0}&limit={1}&offset={2}",
                bearertoken,
                LIMIT.ToString(),
                OFFSET.ToString()));
            #endregion
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            var companyList = new CompaniesResource.Companies();
            if (jsonresponse != null)
            {
                foreach (var company in jsonresponse["objects"])
                {
                    var oneCompany = new CompaniesResource.Object();
                    oneCompany.company_id = jsonresponse["objects"][0]["company_id"];
                    oneCompany.created_at = jsonresponse["objects"][0]["created_at"];
                    oneCompany.id = jsonresponse["objects"][0]["id"];
                    oneCompany.name = jsonresponse["objects"][0]["name"];
                    oneCompany.resource_uri = jsonresponse["objects"][0]["resource_uri"];
                    oneCompany.updated_at = jsonresponse["objects"][0]["updated_at"];
                    companyList.objects.Add(oneCompany);
                }
            }
            return companyList;
        }
        public Boolean GetCompanies(string id, string bearertoken, string limit, string offset)
        {   //Accounts with multiple companies must specify an ID
            #region localvariables
            string APIENDPOINT = "company";
            string logthis;
            var querystring = string.Format(string.Format("?id={0}&bearer_token={1}&limit={2}&offset={3}",
                id,
                bearertoken,
                limit,
                offset));
            #endregion
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            //logrunner.tsLogNoLineBreak(APIENDPOINT.ToString());
            foreach (KeyValuePair<string, dynamic> dictItem in jsonresponse)
            {
                logthis = (string)dictItem.Key + ": ";
                logthis = logthis + dictItem.Value.ToString();
                //logrunner.tsLogNoLineBreak(logthis);
            }
            return true;
        }
        #endregion

    }
    #endregion

    #region orderhelperclass
    public class TrueShipOrderHelperClass
    {
        TrueShip.TSWebHelper.WebHelper tsWebHelper = new WebHelper();
        //TSLogger.Logger logrunner = new TSLogger.Logger();
        string FORMAT = "JSON";

        #region getallordersbydate
        public List<Dictionary<string, dynamic>> GetAllOrdersByDateJSON(string bearertoken, int id, string datefield, DateTime lastsync)
        {
            #region localvariables
            int LIMIT = 10;
            string EXPAND = "all"; //"all"
            string formatteddate = string.Format("{0:s}",
            lastsync);
            string FILTER = String.Format("id={0}&{1}__gte={2}",
                id,
                datefield,
                formatteddate);
            string APIENDPOINT = "orders";
            var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
                FORMAT,
                LIMIT.ToString(),
                bearertoken,
                EXPAND,
                FILTER);
            #endregion
            var listJsonResponses = new List<Dictionary<string, dynamic>>();
            int grab_number = 1;
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            listJsonResponses.Add(jsonresponse);
            decimal totalBatches = (decimal)jsonresponse["meta"]["total_count"] / LIMIT;
            while ( totalBatches - (decimal)grab_number > 0 )
            {
                #region querystring
                querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
                    FORMAT,
                    LIMIT.ToString(),
                    bearertoken,
                    EXPAND,
                    FILTER);
                #endregion
                var additionaljsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
                listJsonResponses.Add(additionaljsonresponse);
                grab_number++;
            }
            return listJsonResponses;
        }
        #endregion

        #region getordersbydateandshipstatus
        public List<Dictionary<string, dynamic>> GetOrdersByDateByShipStatusJSON(string bearertoken, int id, string datefield, string shippingstatus, DateTime lastsync)
        {
            #region localvariables
            int LIMIT = 10;
            string EXPAND = "boxes,boxes__items";
            string formatteddate = string.Format("{0:s}",
            lastsync);
            string FILTER = String.Format("id={0}&status_shipped={1}&{2}__gte={3}",
                id,
                shippingstatus,
                datefield,
                formatteddate);
            string APIENDPOINT = "orders";
            var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
                FORMAT,
                LIMIT.ToString(),
                bearertoken,
                EXPAND,
                FILTER);
            #endregion
            var listJsonResponses = new List<Dictionary<string, dynamic>>();
            int grab_number = 1;
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            listJsonResponses.Add(jsonresponse);
            decimal totalBatches = (decimal)jsonresponse["meta"]["total_count"] / LIMIT;
            while (totalBatches - (decimal)grab_number > 0)
            {
                #region querystring
                querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
                    FORMAT,
                    LIMIT.ToString(),
                    bearertoken,
                    EXPAND,
                    FILTER);
                #endregion
                var additionaljsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
                listJsonResponses.Add(additionaljsonresponse);
                grab_number++;
            }
            return listJsonResponses;
        }
        #endregion

    }
    #endregion
    
    #region ordersservice
    public class TrueShipOrderSync : TrueShipOrderHelperClass
    {
        TrueShip.TSWebHelper.WebHelper tsWebHelper = new WebHelper();
        //TSLogger.Logger logrunner = new TSLogger.Logger();
        string FORMAT = "JSON";
        
        #region get one order
        public OrderResource GetOrderById( string bearertoken, string orderid )
        {
            #region localvariables
            string LIMIT = "1";
            string EXPAND = "all";
            string filter = "primary_id=" + orderid;
            string APIENDPOINT = "orders";
            var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
                FORMAT,
                LIMIT,
                bearertoken,
                EXPAND,
                filter);
            #endregion
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            var orderObject = new OrderResource();
            if ( jsonresponse != null )
            {
                #region loadFields
                var bill_to = jsonresponse["objects"][0]["bill_to"];
                orderObject.bill_to_dict.Add( "bill_to", bill_to["a_type"]);
                orderObject.bill_to_dict.Add( "address_1", bill_to["address_1"]);
                orderObject.bill_to_dict.Add( "address_2", bill_to["address_2"]);
                orderObject.bill_to_dict.Add( "city", bill_to["city"]);
                orderObject.bill_to_dict.Add( "company", bill_to["company"]);
                orderObject.bill_to_dict.Add( "country", bill_to["country"]);
                orderObject.bill_to_dict.Add( "email", bill_to["email"]);
                orderObject.bill_to_dict.Add( "first_name", bill_to["first_name"]);
                orderObject.bill_to_dict.Add( "last_name", bill_to["last_name"]);
                orderObject.bill_to_dict.Add( "order", bill_to["order"]);
                orderObject.bill_to_dict.Add( "phone", bill_to["phone"]);
                orderObject.bill_to_dict.Add( "post_code", bill_to["post_code"]);
                orderObject.bill_to_dict.Add( "residential", bill_to["residential"].ToString());
                orderObject.bill_to_dict.Add( "state", bill_to["state"]);
                orderObject.bill_to_dict.Add( "validated", bill_to["validated"].ToString());
                foreach (var oneBox in jsonresponse["objects"][0]["boxes"])
                    {
                        var box = new OrderResource.Box();
                        var boxes_charges = oneBox["charges"];  //object
                        orderObject.charges_dict.Add("actual_ship_cost", boxes_charges["actual_ship_cost"]);
                        orderObject.charges_dict.Add("declared_value", boxes_charges["declared_value"]);
                        orderObject.charges_dict.Add("insured_value", boxes_charges["insured_value"]);
                        var boxes_dict = new Dictionary<string, string>();
                        //boxes_dict.Add("custom_fields",boxes[0]["custom_fields"]);
                        boxes_dict.Add("delivery_confirmation", oneBox["delivery_confirmation"]);
                        boxes_dict.Add("general_description", oneBox["general_description"]);
                        boxes_dict.Add("height", oneBox["height"]);
                        #region boxitems
                        var boxes_items_dict = new Dictionary<string, Dictionary<string, string>>();
                        var boxes_items = oneBox["items"]; //object
                        if (boxes_items != null)
                        {
                            int box_number = 0;
                            foreach (var box_item in boxes_items)
                            {
                                var box_items_dict = new Dictionary<string, string>();
                                box_number++;
                                box_items_dict.Add("box", box_item["box"]);
                                box_items_dict.Add("charges", box_item["charges"]["unit_price"]);
                                box_items_dict.Add("commodity_code", box_item["commodity_code"]);
                                box_items_dict.Add("description", box_item["description"]);
                                box_items_dict.Add("export_type_code", box_item["export_type_code"]);
                                box_items_dict.Add("item_id", box_item["item_id"]);
                                box_items_dict.Add("joint_production", box_item["joint_production"].ToString());
                                box_items_dict.Add("net_cost_begin_date", box_item["net_cost_begin_date"]);
                                box_items_dict.Add("net_cost_code", box_item["net_cost_code"]);
                                box_items_dict.Add("net_cost_end_date", box_item["net_cost_end_date"]);
                                box_items_dict.Add("origin_country_code", box_item["origin_country_code"]);
                                box_items_dict.Add("part_number", box_item["part_number"]);
                                box_items_dict.Add("pick_location", box_item["pick_location"]);
                                box_items_dict.Add("preference_criteria_code", box_item["preference_criteria_code"]);
                                box_items_dict.Add("quantity", box_item["quantity"].ToString());
                                box_items_dict.Add("resource_uri", box_item["resource_uri"]);
                                box_items_dict.Add("scheduleb_commodity_code", box_item["scheduleb_commodity_code"]);
                                box_items_dict.Add("unit_weight", box_item["unit_weight"]);
                                box_items_dict.Add("url", box_item["url"]);
                                boxes_items_dict.Add(box_number.ToString(), box_items_dict);
                            }
                        }
                        box.boxes_items_dict = boxes_items_dict;
                        boxes_dict.Add("length", oneBox["length"]);
                        boxes_dict.Add("order", oneBox["order"]);
                        boxes_dict.Add("resource_uri", oneBox["resource_uri"]);
                        boxes_dict.Add("saturday_delivery", oneBox["saturday_delivery"]);
                        boxes_dict.Add("shipper_release", oneBox["shipper_release"].ToString());
                        boxes_dict.Add("tracking_number", oneBox["tracking_number"]);
                        boxes_dict.Add("weight", oneBox["weight"]);
                        boxes_dict.Add("width", oneBox["width"]);
                        box.boxes_dict = boxes_dict;
                        orderObject.Boxes.Add(box);
                    }
                #endregion
                var charges = jsonresponse["objects"][0]["charges"]; //object
                orderObject.charges_dict.Add("calculated_grand_total", charges["calculated_grand_total"]);
                orderObject.charges_dict.Add("calculated_shipping", charges["calculated_shipping"]);
                orderObject.charges_dict.Add("calculated_tax", charges["calculated_tax"]);
                orderObject.charges_dict.Add("calculated_total", charges["calculated_total"]);
                orderObject.charges_dict.Add("grand_total", charges["grand_total"]);
                orderObject.charges_dict.Add("grand_total_source", charges["grand_total_source"]);
                orderObject.charges_dict.Add("imported_grand_total", charges["imported_grand_total"]);
                orderObject.charges_dict.Add("imported_shipping", charges["imported_shipping"]);
                orderObject.charges_dict.Add("imported_tax", charges["imported_tax"]);
                orderObject.charges_dict.Add("imported_total", charges["imported_total"]);
                orderObject.charges_dict.Add("shipping", charges["shipping"]);
                orderObject.charges_dict.Add("tax", charges["tax"]);
                orderObject.charges_dict.Add("tax_source", charges["tax_source"]);
                orderObject.charges_dict.Add("total", charges["total"]);
                orderObject.charges_dict.Add("total_source", charges["total_source"]);
                var created_at = jsonresponse["objects"][0]["created_at"];
                //var custom_fields = jsonresponse["objects"][0]["custom_fields"]; //object
                orderObject.customer_number = jsonresponse["objects"][0]["customer_number"];
                orderObject.future_ship_at = jsonresponse["objects"][0]["future_ship_at"];
                orderObject.imported_at = jsonresponse["objects"][0]["imported_at"];
                orderObject.message = jsonresponse["objects"][0]["message"];
                orderObject.numerical_id = jsonresponse["objects"][0]["numerical_id"];
                orderObject.order_time = jsonresponse["objects"][0]["order_time"];
                orderObject.po_number = jsonresponse["objects"][0]["po_number"];
                orderObject.primary_id = jsonresponse["objects"][0]["primary_id"];
                orderObject.printed_at = jsonresponse["objects"][0]["printed_at"];
                orderObject.resource_uri = jsonresponse["objects"][0]["resource_uri"];
                orderObject.revision_id = jsonresponse["objects"][0]["revision_id"];
                orderObject.revisions_resource_uri = jsonresponse["objects"][0]["revisions_resource_uri"];
                var ship_from = jsonresponse["objects"][0]["ship_from"]; //object
                orderObject.ship_from_dict.Add("a_type", ship_from["a_type"]);
                orderObject.ship_from_dict.Add("address_1", ship_from["address_1"]);
                orderObject.ship_from_dict.Add("address_2", ship_from["address_2"]);
                orderObject.ship_from_dict.Add("city", ship_from["city"]);
                orderObject.ship_from_dict.Add("company", ship_from["company"]);
                orderObject.ship_from_dict.Add("country", ship_from["country"]);
                orderObject.ship_from_dict.Add("email", ship_from["email"]);
                orderObject.ship_from_dict.Add("first_name", ship_from["first_name"]);
                orderObject.ship_from_dict.Add("last_name", ship_from["last_name"]);
                orderObject.ship_from_dict.Add("order", ship_from["order"]);
                orderObject.ship_from_dict.Add("phone", ship_from["phone"]);
                orderObject.ship_from_dict.Add("post_code", ship_from["post_code"]);
                orderObject.ship_from_dict.Add("residential", ship_from["residential"].ToString());
                orderObject.ship_from_dict.Add("resource_uri", ship_from["resource_uri"]);
                orderObject.ship_from_dict.Add("state", ship_from["state"]);
                orderObject.ship_from_dict.Add("validated", ship_from["validated"].ToString());
                orderObject.ship_time = jsonresponse["objects"][0]["ship_time"];
                var ship_to = jsonresponse["objects"][0]["ship_to"]; //object
                orderObject.ship_to_dict.Add("a_type", ship_to["a_type"]);
                orderObject.ship_to_dict.Add("address_1", ship_to["address_1"]);
                orderObject.ship_to_dict.Add("address_2", ship_to["address_2"]);
                orderObject.ship_to_dict.Add("city", ship_to["city"]);
                orderObject.ship_to_dict.Add("company", ship_to["company"]);
                orderObject.ship_to_dict.Add("country", ship_to["country"]);
                orderObject.ship_to_dict.Add("email", ship_to["email"]);
                orderObject.ship_to_dict.Add("first_name", ship_to["first_name"]);
                orderObject.ship_to_dict.Add("last_name", ship_to["last_name"]);
                orderObject.ship_to_dict.Add("order", ship_to["order"]);
                orderObject.ship_to_dict.Add("phone", ship_to["phone"]);
                orderObject.ship_to_dict.Add("post_code", ship_to["post_code"]);
                orderObject.ship_to_dict.Add("residential", ship_to["residential"].ToString());
                orderObject.ship_to_dict.Add("resource_uri", ship_to["resource_uri"]);
                orderObject.ship_to_dict.Add("state", ship_to["state"]);
                orderObject.ship_to_dict.Add("validated", ship_to["validated"].ToString());
                orderObject.ship_type = jsonresponse["objects"][0]["ship_type"];
                orderObject.ship_via = jsonresponse["objects"][0]["ship_via"];
                orderObject.source = jsonresponse["objects"][0]["source"];
                var sources = jsonresponse["objects"][0]["sources"]; //object
                orderObject.sources_dict.Add("account", sources[0]["account"]);
                orderObject.sources_dict.Add("channel", sources[0]["channel"]);
                orderObject.sources_dict.Add("created_at", sources[0]["created_at"]);
                orderObject.sources_dict.Add("name", sources[0]["name"]);
                orderObject.sources_dict.Add("order", sources[0]["order"]);
                orderObject.sources_dict.Add("order_source_id", sources[0]["order_source_id"]);
                orderObject.sources_dict.Add("resource_uri", sources[0]["resource_uri"]);
                orderObject.sources_dict.Add("retrieved_at", sources[0]["retrieved_at"]);
                orderObject.sources_dict.Add("updated_at", sources[0]["updated_at"]);
                orderObject.status_shipped = jsonresponse["objects"][0]["status_shipped"];
                orderObject.terms = jsonresponse["objects"][0]["terms"];
                orderObject.unique_id = jsonresponse["objects"][0]["unique_id"];
                orderObject.updated_at = jsonresponse["objects"][0]["updated_at"];
                orderObject.warehouse = jsonresponse["objects"][0]["warehouse"];
                #endregion
            }
            return orderObject; 
        }
        #endregion
        
        #region get orders by updated date
        public OrdersResource getOrders(string bearertoken, int id, DateTime lastsync)
        {
            #region localvariables
            var datefilter = "updated_at";
            #endregion
            var dictOrderList = new OrdersResource();
            var jsonResponseList = this.GetAllOrdersByDateJSON(bearertoken, id, datefilter, lastsync);
            if ( jsonResponseList != null )
            { 
            #region loadFields
            foreach ( var oneresponse in jsonResponseList[0]["objects"] )
            {
                var orderObject = new OrderResource();
                var bill_to = oneresponse["bill_to"];
                orderObject.bill_to_dict.Add("bill_to", bill_to["a_type"]);
                orderObject.bill_to_dict.Add("address_1", bill_to["address_1"]);
                orderObject.bill_to_dict.Add("address_2", bill_to["address_2"]);
                orderObject.bill_to_dict.Add("city", bill_to["city"]);
                orderObject.bill_to_dict.Add("company", bill_to["company"]);
                orderObject.bill_to_dict.Add("country", bill_to["country"]);
                orderObject.bill_to_dict.Add("email", bill_to["email"]);
                orderObject.bill_to_dict.Add("first_name", bill_to["first_name"]);
                orderObject.bill_to_dict.Add("last_name", bill_to["last_name"]);
                orderObject.bill_to_dict.Add("order", bill_to["order"]);
                orderObject.bill_to_dict.Add("phone", bill_to["phone"]);
                orderObject.bill_to_dict.Add("post_code", bill_to["post_code"]);
                orderObject.bill_to_dict.Add("residential", bill_to["residential"].ToString());
                orderObject.bill_to_dict.Add("state", bill_to["state"]);
                orderObject.bill_to_dict.Add("validated", bill_to["validated"].ToString());
                foreach (var oneBox in oneresponse["boxes"])
                {
                    var box = new OrderResource.Box();
                    var boxes_charges = oneBox["charges"];  //object
                    orderObject.charges_dict.Add("actual_ship_cost", boxes_charges["actual_ship_cost"]);
                    orderObject.charges_dict.Add("declared_value", boxes_charges["declared_value"]);
                    orderObject.charges_dict.Add("insured_value", boxes_charges["insured_value"]);
                    var boxes_dict = new Dictionary<string, string>();
                    //boxes_dict.Add("custom_fields",boxes[0]["custom_fields"]);
                    boxes_dict.Add("delivery_confirmation", oneBox["delivery_confirmation"]);
                    boxes_dict.Add("general_description", oneBox["general_description"]);
                    boxes_dict.Add("height", oneBox["height"]);
                    #region boxitems
                    var boxes_items_dict = new Dictionary<string, Dictionary<string, string>>();
                    var boxes_items = oneBox["items"]; //object
                    if (boxes_items != null)
                    {
                        int box_number = 0;
                        foreach (var box_item in boxes_items)
                        {
                            var box_items_dict = new Dictionary<string, string>();
                            box_number++;
                            box_items_dict.Add("box", box_item["box"]);
                            box_items_dict.Add("charges", box_item["charges"]["unit_price"]);
                            box_items_dict.Add("commodity_code", box_item["commodity_code"]);
                            box_items_dict.Add("description", box_item["description"]);
                            box_items_dict.Add("export_type_code", box_item["export_type_code"]);
                            box_items_dict.Add("item_id", box_item["item_id"]);
                            box_items_dict.Add("joint_production", box_item["joint_production"].ToString());
                            box_items_dict.Add("net_cost_begin_date", box_item["net_cost_begin_date"]);
                            box_items_dict.Add("net_cost_code", box_item["net_cost_code"]);
                            box_items_dict.Add("net_cost_end_date", box_item["net_cost_end_date"]);
                            box_items_dict.Add("origin_country_code", box_item["origin_country_code"]);
                            box_items_dict.Add("part_number", box_item["part_number"]);
                            box_items_dict.Add("pick_location", box_item["pick_location"]);
                            box_items_dict.Add("preference_criteria_code", box_item["preference_criteria_code"]);
                            box_items_dict.Add("quantity", box_item["quantity"].ToString());
                            box_items_dict.Add("resource_uri", box_item["resource_uri"]);
                            box_items_dict.Add("scheduleb_commodity_code", box_item["scheduleb_commodity_code"]);
                            box_items_dict.Add("unit_weight", box_item["unit_weight"]);
                            box_items_dict.Add("url", box_item["url"]);
                            boxes_items_dict.Add(box_number.ToString(), box_items_dict);
                        }
                    }
                    box.boxes_items_dict = boxes_items_dict;
                    boxes_dict.Add("length", oneBox["length"]);
                    boxes_dict.Add("order", oneBox["order"]);
                    boxes_dict.Add("resource_uri", oneBox["resource_uri"]);
                    boxes_dict.Add("saturday_delivery", oneBox["saturday_delivery"]);
                    boxes_dict.Add("shipper_release", oneBox["shipper_release"].ToString());
                    boxes_dict.Add("tracking_number", oneBox["tracking_number"]);
                    boxes_dict.Add("weight", oneBox["weight"]);
                    boxes_dict.Add("width", oneBox["width"]);
                    box.boxes_dict = boxes_dict;
                    orderObject.Boxes.Add(box);
                }
                    #endregion
                var charges = oneresponse["charges"]; //object
                orderObject.charges_dict.Add("calculated_grand_total", charges["calculated_grand_total"]);
                orderObject.charges_dict.Add("calculated_shipping", charges["calculated_shipping"]);
                orderObject.charges_dict.Add("calculated_tax", charges["calculated_tax"]);
                orderObject.charges_dict.Add("calculated_total", charges["calculated_total"]);
                orderObject.charges_dict.Add("grand_total", charges["grand_total"]);
                orderObject.charges_dict.Add("grand_total_source", charges["grand_total_source"]);
                orderObject.charges_dict.Add("imported_grand_total", charges["imported_grand_total"]);
                orderObject.charges_dict.Add("imported_shipping", charges["imported_shipping"]);
                orderObject.charges_dict.Add("imported_tax", charges["imported_tax"]);
                orderObject.charges_dict.Add("imported_total", charges["imported_total"]);
                orderObject.charges_dict.Add("shipping", charges["shipping"]);
                orderObject.charges_dict.Add("tax", charges["tax"]);
                orderObject.charges_dict.Add("tax_source", charges["tax_source"]);
                orderObject.charges_dict.Add("total", charges["total"]);
                orderObject.charges_dict.Add("total_source", charges["total_source"]);
                var created_at = oneresponse["created_at"];
                //var custom_fields = jsonresponse["objects"][0]["custom_fields"]; //object
                orderObject.customer_number = oneresponse["customer_number"];
                orderObject.future_ship_at = oneresponse["future_ship_at"];
                orderObject.imported_at = oneresponse["imported_at"];
                orderObject.message = oneresponse["message"];
                orderObject.numerical_id = oneresponse["numerical_id"];
                orderObject.order_time = oneresponse["order_time"];
                orderObject.po_number = oneresponse["po_number"];
                orderObject.primary_id = oneresponse["primary_id"];
                orderObject.printed_at = oneresponse["printed_at"];
                orderObject.resource_uri = oneresponse["resource_uri"];
                orderObject.revision_id = oneresponse["revision_id"];
                orderObject.revisions_resource_uri = oneresponse["revisions_resource_uri"];
                var ship_from = oneresponse["ship_from"]; //object
                orderObject.ship_from_dict.Add("a_type", ship_from["a_type"]);
                orderObject.ship_from_dict.Add("address_1", ship_from["address_1"]);
                orderObject.ship_from_dict.Add("address_2", ship_from["address_2"]);
                orderObject.ship_from_dict.Add("city", ship_from["city"]);
                orderObject.ship_from_dict.Add("company", ship_from["company"]);
                orderObject.ship_from_dict.Add("country", ship_from["country"]);
                orderObject.ship_from_dict.Add("email", ship_from["email"]);
                orderObject.ship_from_dict.Add("first_name", ship_from["first_name"]);
                orderObject.ship_from_dict.Add("last_name", ship_from["last_name"]);
                orderObject.ship_from_dict.Add("order", ship_from["order"]);
                orderObject.ship_from_dict.Add("phone", ship_from["phone"]);
                orderObject.ship_from_dict.Add("post_code", ship_from["post_code"]);
                orderObject.ship_from_dict.Add("residential", ship_from["residential"].ToString());
                orderObject.ship_from_dict.Add("resource_uri", ship_from["resource_uri"]);
                orderObject.ship_from_dict.Add("state", ship_from["state"]);
                orderObject.ship_from_dict.Add("validated", ship_from["validated"].ToString());
                orderObject.ship_time = oneresponse["ship_time"];
                var ship_to = oneresponse["ship_to"]; //object
                orderObject.ship_to_dict.Add("a_type", ship_to["a_type"]);
                orderObject.ship_to_dict.Add("address_1", ship_to["address_1"]);
                orderObject.ship_to_dict.Add("address_2", ship_to["address_2"]);
                orderObject.ship_to_dict.Add("city", ship_to["city"]);
                orderObject.ship_to_dict.Add("company", ship_to["company"]);
                orderObject.ship_to_dict.Add("country", ship_to["country"]);
                orderObject.ship_to_dict.Add("email", ship_to["email"]);
                orderObject.ship_to_dict.Add("first_name", ship_to["first_name"]);
                orderObject.ship_to_dict.Add("last_name", ship_to["last_name"]);
                orderObject.ship_to_dict.Add("order", ship_to["order"]);
                orderObject.ship_to_dict.Add("phone", ship_to["phone"]);
                orderObject.ship_to_dict.Add("post_code", ship_to["post_code"]);
                orderObject.ship_to_dict.Add("residential", ship_to["residential"].ToString());
                orderObject.ship_to_dict.Add("resource_uri", ship_to["resource_uri"]);
                orderObject.ship_to_dict.Add("state", ship_to["state"]);
                orderObject.ship_to_dict.Add("validated", ship_to["validated"].ToString());
                orderObject.ship_type = oneresponse["ship_type"];
                orderObject.ship_via = oneresponse["ship_via"];
                orderObject.source = oneresponse["source"];
                var sources = oneresponse["sources"]; //object
                orderObject.sources_dict.Add("account", sources[0]["account"]);
                orderObject.sources_dict.Add("channel", sources[0]["channel"]);
                orderObject.sources_dict.Add("created_at", sources[0]["created_at"]);
                orderObject.sources_dict.Add("name", sources[0]["name"]);
                orderObject.sources_dict.Add("order", sources[0]["order"]);
                orderObject.sources_dict.Add("order_source_id", sources[0]["order_source_id"]);
                orderObject.sources_dict.Add("resource_uri", sources[0]["resource_uri"]);
                orderObject.sources_dict.Add("retrieved_at", sources[0]["retrieved_at"]);
                orderObject.sources_dict.Add("updated_at", sources[0]["updated_at"]);
                orderObject.status_shipped = oneresponse["status_shipped"];
                orderObject.terms = oneresponse["terms"];
                orderObject.unique_id = oneresponse["unique_id"];
                orderObject.updated_at = oneresponse["updated_at"];
                orderObject.warehouse = oneresponse["warehouse"];
                dictOrderList.OrderList.Add(orderObject);
            }
            #endregion
            }
            return dictOrderList;
        }
        #endregion

        #region get order backup details
        public Boolean GetOrderBackupDetails(string bearertoken, string orderid)
        {
            #region localvariables
            string APIENDPOINT = string.Format("order_backup_details/{0}",
                orderid);
            string querystring = string.Format("bearer_token={0}",
                bearertoken);
            #endregion
            var jsonresponse = tsWebHelper.submitApiGet(APIENDPOINT, querystring);
            if ( jsonresponse != null )
            {
                return true;
            }
            else { return false; }
        }
        #endregion

    }
    #endregion

    #region locationservice
    public class TrueShipLocationSync : TrueShipOrderHelperClass
    {
        TrueShip.TSWebHelper.WebHelper tsWebHelper = new WebHelper();
        //TSLogger.Logger logrunner = new TSLogger.Logger();

        #region get box items from unshipped orders
        public List<dynamic> getUnshippedOrderItemsAfterDateTime(string bearertoken, int id, string datefilter, DateTime lastsync)
        {
            #region localvariables
                var status_shipped = "False";
            #endregion
            var jsonResponseList = this.GetOrdersByDateByShipStatusJSON(bearertoken, id, datefilter, status_shipped, lastsync);
            var boxList = new List<dynamic>();
            if (jsonResponseList != null)
            {
                #region pull out and return list of box items
                var responsenumber = 0;
                foreach (var oneresponse in jsonResponseList)
                {
                    var ordernumber = 0;
                    foreach (var oneorder in oneresponse["objects"])
                    {
                        var boxnumber = 0;
                        foreach (var onebox in oneorder["boxes"])
                        {
                                var boxes_items = onebox["items"];
                                if (boxes_items != null)
                                {
                                    int box_item_number = 0;
                                    foreach (var box_item in boxes_items)
                                    {
                                        boxList.Add(box_item);
                                        box_item_number++;
                                    }
                                }
                                boxnumber++;
                        }
                        ordernumber++;
                    }
                    responsenumber++;
                }
                #endregion
                return boxList;
            }
            else { return boxList; }
        }
        #endregion

        #region update pick locations on box items
        public Boolean updateOrderItemPickLocations(string bearertoken, int id, List<KeyValuePair<string, TrueShip.Models.PickLocation>> orderitemlist)
        {
        #region localvariables
            string BASEURL = "https://www.readycloud.com";
        #endregion
            var jserializer = new JavaScriptSerializer();
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            foreach (KeyValuePair<string, TrueShip.Models.PickLocation>oneorderitem in orderitemlist)
            {
                Uri putApi = new Uri(string.Format("{0}{1}?bearer_token={2}",
                    BASEURL,
                    oneorderitem.Key,
                    bearertoken));
                var reSerializedOrder = jserializer.Serialize(oneorderitem.Value);
                //logrunner.tsLogNoLineBreak(reSerializedOrder);
                //logrunner.tsLogNoLineBreak("Calling @ '" + putApi + "'");
                try
                {
                    var request = client.UploadString(putApi, "PATCH", reSerializedOrder);
                    //logrunner.tsLogNoLineBreak("Order Successfully Updated Via API!");
                }
                catch (WebException webe)
                {
                    //logrunner.tsLogNoLineBreak(webe.Message);
                }
            }
            return true;
        }
        #endregion

    }
    #endregion

}
