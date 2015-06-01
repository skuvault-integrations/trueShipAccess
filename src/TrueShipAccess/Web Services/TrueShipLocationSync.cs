using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

//using TSLogger;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
    public class TrueShipLocationSync : TrueShipOrderHelperClass
    {
        WebHelper tsWebHelper = new WebHelper();
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
        public Boolean updateOrderItemPickLocations(string bearertoken, int id, List<KeyValuePair<string, PickLocation>> orderitemlist)
        {
        #region localvariables
            string BASEURL = "https://www.readycloud.com";
        #endregion
            var jserializer = new JavaScriptSerializer();
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            foreach (KeyValuePair<string, PickLocation>oneorderitem in orderitemlist)
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
}
