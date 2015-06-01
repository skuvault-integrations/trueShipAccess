using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TrueShipAccess.Misc;

namespace TrueShipAccess
{
    public class WebHelper
    {
        TrueShipLogger logservice = new TrueShipLogger();
        
        #region getregion
        public Dictionary<string, dynamic> submitApiGet(string endpoint, string querystring)
        {
            #region localvariables
            string BASEURL = "https://www.readycloud.com/api/v1";
            Uri getAPI = new Uri(string.Format("{0}/{1}/?{2}",
                BASEURL,
                endpoint,
                querystring));
            #endregion
            Console.WriteLine("GET " + getAPI);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getAPI);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            try
            {
                logservice.tsLogNoLineBreak("Calling @ '" + getAPI);
                WebResponse response = request.GetResponse();
                var jsonresponse = GetJsonResponse(response);
                var jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jData = (Dictionary<string, dynamic>)jSerializer.DeserializeObject(jsonresponse);
                return jData;
            }
            catch (WebException webe)
            {
                logservice.tsLogWebServiceError(webe, getAPI);
                return null;
            }
        }
        #endregion

        #region json deserialize web response
        private string GetJsonResponse( WebResponse response )
        {
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader ( stream );
            string jsonstring = reader.ReadLine();
            stream.Close();
            return jsonstring;
        }
        #endregion

    }

}