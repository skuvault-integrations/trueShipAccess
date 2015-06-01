using System;
using System.Collections.Generic;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
    public class TruesShipBasicServices
    {
        WebHelper tsWebHelper = new WebHelper();
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
}