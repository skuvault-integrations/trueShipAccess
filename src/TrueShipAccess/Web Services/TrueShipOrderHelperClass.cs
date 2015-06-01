using System;
using System.Collections.Generic;

namespace TrueShipAccess
{
    public class TrueShipOrderHelperClass
    {
        WebHelper tsWebHelper = new WebHelper();
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
}