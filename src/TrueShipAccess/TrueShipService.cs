using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public class TrueShipService : ITrueShipService
	{
		private readonly TrueShipCredentials _credentials;
		private readonly TrueShipConfiguration _config;
		private readonly IWebRequestServices _webRequestServices;

		private string _format = "JSON";
		private int _limit = 10;
		
		private int Limit
		{
			get { return _limit; }
			set { _limit = value; }
		}

		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}

		public TrueShipService(TrueShipCredentials credentials, TrueShipConfiguration config, IWebRequestServices webRequestServices)
		{
			Condition.Requires(credentials, "credentials").IsNotNull();
			Condition.Requires(config, "config").IsNotNull();
			Condition.Requires(webRequestServices, "webRequestServices").IsNotNull();

			this._credentials = credentials;
			this._config = config;
			this._webRequestServices = webRequestServices;
		}

		public TrueShipService(TrueShipCredentials credentials, TrueShipConfiguration config)
			: this(credentials, config, new WebRequestServices(config, credentials)) { }


		public async Task<IEnumerable<TrueShipOrderResource>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo)
		{
			try
			{
				var uri = string.Format("{0}/{1}", _config.ApiBaseUri, "orders");
				var query = string.Format("bearer_token={0}&updated_at__gte={1:s}&updated_at__lte={2:s}", this._credentials.AccessToken, dateFrom, dateTo);

				OrdersResponse result = null;
				await ActionPolicies
					.GetAsync
					.Do(async () =>
					{
						result = await _webRequestServices.SubmitGet<OrdersResponse>(uri, query);
					})
					.ConfigureAwait(false);

				return result.Objects;
			}
			catch (Exception ex)
			{
				var truShipEx = new TrueShipCommonException("Error on GetOrdersAsync", ex);
				this.LogTraceException("Error. ", truShipEx);
				throw truShipEx;
			}
		}

		/// <summary>
		/// Retrieve the number of remaining orders of your company, if you only have one.
		/// </summary>
		/// <returns></returns>
		public RemainingOrdersResource GetRemainingOrders()
		{
			var APIENDPOINT = "remaining_orders";
			var querystring = string.Format("bearer_token={0}", this._credentials.AccessToken);

			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);

			var jsonresponse = _webRequestServices.SubmitGet<RemainingOrdersResource>(serviceUrl, querystring);
			jsonresponse.Wait();

			return jsonresponse.Result;
		}

		/// <summary>
		/// Retrieve a list of boxes.
		/// </summary>
		/// <param name="limit">The number of entries to limit the returned data to</param>
		/// <param name="offset">The entry to start the returned data with</param>
		/// <param name="orderId">Order ID for which you want retrieve boxes</param>
		/// <returns></returns>
		public async Task<IEnumerable<Box>> GetBoxes(int limit, int offset, int orderId = -1)
		{
			const string apiEndpoint = "boxes";
			var query = string.Format("bearer_token={0}&offset={1}&limit={2}&expand=all", this._credentials.AccessToken, offset, limit);

			if (orderId > -1)
			{
				query += "&order=" + orderId;
			}

			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, apiEndpoint);

			var items = new List<Box>();
			await ActionPolicies
				.GetAsync
				.Do(async () =>
				{
					var response = await this._webRequestServices.SubmitGet<BoxesResponse>(serviceUrl, query);

					items = response.Objects;
				}).ConfigureAwait(false);

			return items;
		}

		/// <summary>
		/// Retrieve a list of companies.
		/// </summary>
		/// <param name="offset">The entry to start the returned data with</param>
		/// <returns></returns>
		public async Task<IEnumerable<CompanyResponse.Company>> GetCompanies(int offset)
		{   //Accounts with only 1 company must use this call
			const string apiendpoint = "company";
			string querystring = string.Format("bearer_token={0}&limit={1}&offset={2}", _credentials.AccessToken, Limit, offset);

			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, apiendpoint);
			
			var companyList = new List<CompanyResponse.Company>();

			await ActionPolicies
				.GetAsync
				.Do(async () =>
				{
					var response = await this._webRequestServices.SubmitGet<CompanyResponse.Companies>(serviceUrl, querystring);
					companyList = response.Objects;
				})
				.ConfigureAwait(false);
			
			return companyList;
		}

		/// <summary>
		/// Retrieve a single order by ID
		/// </summary>
		/// <param name="id">ID of the order to get</param>
		/// <param name="datefield"></param>
		/// <param name="lastsync"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TrueShipOrderResource>> GetAllOrdersByDate(int id, string datefield, DateTime lastsync)
		{
			string EXPAND = "all";
			string formatteddate = string.Format("{0:s}",
				lastsync);
			string filter = String.Format("id={0}&{1}__gte={2}",
				id,
				datefield,
				formatteddate);
			string APIENDPOINT = "orders";
			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);
			var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
				Format,
				Limit,
				this._credentials.AccessToken,
				EXPAND,
				filter);
			var listOrders = new List<TrueShipOrderResource>();
			int grabNumber = 1;

			string querystring1 = querystring;
			IEnumerable<TrueShipOrderResource> orders = new List<TrueShipOrderResource>();
			var response = new OrdersResponse();
			await ActionPolicies
				.GetAsync
				.Do(async () =>
				{
					response = await _webRequestServices.SubmitGet<OrdersResponse>(serviceUrl, querystring1);
					orders = response.Objects;
				}).ConfigureAwait(false);

			listOrders.AddRange(orders);

			decimal totalBatches = (decimal)response.Meta.TotalCount / Limit;
			while (totalBatches - grabNumber > 0)
			{
				await ActionPolicies.GetAsync.Do(async () =>
				{
					serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);
					querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
						Format,
						Limit,
						this._credentials.AccessToken,
						EXPAND,
						filter);

					response = await this._webRequestServices.SubmitGet<OrdersResponse>(serviceUrl, querystring);

					orders = response.Objects;
				});
				
				listOrders.AddRange(orders);
				grabNumber++;
			}

			return listOrders;
		}

		public async Task<List<TrueShipOrderResource>> GetOrdersByDateByShipStatus(string bearertoken, int id, string datefield, string shippingstatus, DateTime lastsync)
		{
			string EXPAND = "boxes,boxes__items";
			string formatteddate = string.Format("{0:s}",
				lastsync);
			string FILTER = String.Format("id={0}&status_shipped={1}&{2}__gte={3}",
				id,
				shippingstatus,
				datefield,
				formatteddate);
			string APIENDPOINT = "orders";
			

			var listJsonResponses = new List<TrueShipOrderResource>();
			int grabNumber = 1;
			IEnumerable<TrueShipOrderResource> orders = new List<TrueShipOrderResource>();
			OrdersResponse response = null;

			await ActionPolicies
				.GetAsync
				.Do(async () =>
				{
					var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);
					var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
							Format,
							Limit,
							bearertoken,
							EXPAND,
							FILTER);

					response = await this._webRequestServices.SubmitGet<OrdersResponse>(serviceUrl, querystring);
					
					orders = response.Objects;
				}).ConfigureAwait(false);

			
			listJsonResponses.AddRange(orders);

			var totalBatches = (decimal)response.Meta.TotalCount / Limit;
			while (totalBatches - grabNumber > 0)
			{
				var querystring = string.Format("format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
					Format,
					Limit,
					bearertoken,
					EXPAND,
					FILTER);

				await ActionPolicies.GetAsync.Do(async () =>
				{
					var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);
					response = await this._webRequestServices.SubmitGet<OrdersResponse>(serviceUrl, querystring);
				}).ConfigureAwait(false);


				listJsonResponses.AddRange(response.Objects);
				grabNumber++;
			}
			return listJsonResponses;
		}

		public async Task<List<Item>> GetUnshippedOrderItemsAfterDateTime(int id, string datefilter, DateTime lastsync)
		{
			const string statusShipped = "False";
			var jsonResponseList = await this.GetOrdersByDateByShipStatus(this._credentials.AccessToken, id, datefilter, statusShipped, lastsync);
			
			var boxList = new List<Item>();
			if (jsonResponseList != null)
			{
				foreach (var oneresponse in jsonResponseList)
				{
					foreach (var onebox in oneresponse.Boxes)
					{
						var boxesItems = onebox.Items;
						if (boxesItems != null)
						{
							boxList.AddRange(boxesItems);
						}
					}
				}

				return boxList;
			}
			else { return boxList; }
		}

		public async Task<Boolean> UpdateOrderItemPickLocations(IEnumerable<KeyValuePair<string, PickLocation>> orderitemlist)
		{
			var client = new HttpClient();

			foreach (KeyValuePair<string, PickLocation> oneorderitem in orderitemlist)
			{
				var request = this._webRequestServices.CreateUpdateOrderItemPickLocationRequest(oneorderitem );

				//logrunner.tsLogNoLineBreak(reSerializedOrder);
				//logrunner.tsLogNoLineBreak("Calling @ '" + putApi + "'");
				try
				{
					var response = await client.SendAsync(request);

					var responseData = await response.Content.ReadAsStringAsync();

					if (response.StatusCode == HttpStatusCode.Accepted)
					{
						return true;
					}

					if (response.StatusCode == HttpStatusCode.Unauthorized)
					{
						throw new TrueShipAuthException("Unauthorized", new Exception());
					}

					Debug.WriteLine(responseData);

					//logrunner.tsLogNoLineBreak("Order Successfully Updated Via API!");
				}
				catch (WebException webe)
				{
					return false;
					//logrunner.tsLogNoLineBreak(webe.Message);
				}
			}
			return true;
		}

		public TrueShipOrderResource GetOrderById(string id)
		{
			string EXPAND = "all";
			string filter = "primary_id=" + id;
			string APIENDPOINT = "orders";
			var querystring = string.Format("format={0}&&bearer_token={1}&expand={2}&{3}",
				Format,
				_credentials.AccessToken,
				EXPAND,
				filter);

			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);
			var jsonresponse = this._webRequestServices.SubmitGet<OrdersResponse>( serviceUrl, querystring);
			jsonresponse.Wait();

			return jsonresponse.Result.Objects.Single();
		}

		public async Task<IEnumerable<TrueShipOrderResource>> GetOrders(DateTime lastsync)
		{
			const string datefilter = "updated_at";

			return await this.GetAllOrdersByDate( this._credentials.CompanyId, datefilter, lastsync);
		}

		public OrderBackupResource.OrderBackupResponse GetOrderBackupDetails(string orderid)
		{
			var APIENDPOINT = string.Format("order_backup_details/{0}", orderid);
			var querystring = string.Format("bearer_token={0}", this._credentials.AccessToken);
			var serviceUrl = string.Format("{0}/{1}", this._config.ApiBaseUri, APIENDPOINT);

			var jsonresponse = this._webRequestServices.SubmitGet<OrderBackupResource.OrderBackupResponse>( serviceUrl, querystring);
			jsonresponse.Wait();

			return jsonresponse.Result;
		}

		private void LogTraceException(string message, TrueShipException TrueShipException)
		{
			TrueShipLogger.Log().Trace(TrueShipException, message);
		}
	}
}