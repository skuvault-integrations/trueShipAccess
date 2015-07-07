using CuttingEdge.Conditions;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public class WebRequestServices: IWebRequestServices
	{
		private readonly TrueShipConfiguration _config;
		private readonly TrueShipCredentials _credentials;

		readonly TrueShipLogger _logservice = new TrueShipLogger();
		
		public WebRequestServices( TrueShipConfiguration config, TrueShipCredentials credentials )
		{
			Condition.Requires(config, "config").IsNotNull();
			Condition.Requires(credentials, "credentials").IsNotNull();

			this._config = config;
			this._credentials = credentials;
		}

		public async Task<T> SubmitGet<T>(string serviceUrl, string querystring) where T: class 
		{
			var getApi = new Uri(string.Format("{0}?{1}",
				serviceUrl,
				querystring));
			Console.WriteLine("GET " + getApi);
			var request = (HttpWebRequest)WebRequest.Create(getApi);
			request.Method = WebRequestMethods.Http.Get;
			request.ContentType = "application/json";
			try
			{
				_logservice.tsLogNoLineBreak("Calling @ '" + getApi);
				var response = await request.GetResponseAsync();

				return JsonSerializer.DeserializeFromStream<T>(response.GetResponseStream());
			}
			catch (WebException webe)
			{
				_logservice.tsLogWebServiceError(webe, getApi);

				var response = webe.Response as HttpWebResponse;
				if (response != null)
				{
					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new TrueShipAuthException(webe.Message, webe);
					}
				}
				
				return null;
			}
		}


		public HttpRequestMessage CreateUpdateOrderItemPickLocationRequest(KeyValuePair<string, PickLocation> oneorderitem)
		{
			var reSerializedOrder = JsonSerializer.SerializeToString(oneorderitem.Value);

			var putApi = new Uri(string.Format("{0}/{1}?bearer_token={2}",
				this._config.ServiceBaseUri,
				oneorderitem.Key,
				this._credentials.AccessToken));

			var request = new HttpRequestMessage(new HttpMethod("PATCH"), putApi)
			{
				Content = new StringContent(reSerializedOrder, Encoding.UTF8, "application/json")
			};

			return request;
		}
    }
}