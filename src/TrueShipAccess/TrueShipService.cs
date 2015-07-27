using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;
using TrueShipAccess.WebServices;

namespace TrueShipAccess
{
	public sealed class TrueShipService : ITrueShipService
	{
		private readonly TrueShipCredentials _credentials;
		private readonly TrueShipConfiguration _config;
		private readonly IWebRequestServices _webRequestServices;

		private string _format = "JSON";
		private int _limit = 10;

		public int Limit
		{
			get { return this._limit; }
			set { this._limit = value; }
		}

		public string Format
		{
			get { return this._format; }
			set { this._format = value; }
		}

		public TrueShipService( TrueShipCredentials credentials, TrueShipConfiguration config, IWebRequestServices webRequestServices )
		{
			Condition.Requires( credentials, "credentials" ).IsNotNull();
			Condition.Requires( config, "config" ).IsNotNull();
			Condition.Requires( webRequestServices, "webRequestServices" ).IsNotNull();

			this._credentials = credentials;
			this._config = config;
			this._webRequestServices = webRequestServices;
		}

		public TrueShipService( TrueShipCredentials credentials, TrueShipConfiguration config )
			: this( credentials, config, new WebRequestServices( config, credentials ) )
		{
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
		{
			try
			{
				var uri = string.Format( "{0}/{1}", this._config.ApiBaseUri, "orders" );
				var query = string.Format( "bearer_token={0}&updated_at__gte={1:s}&updated_at__lte={2:s}", this._credentials.AccessToken, dateFrom, dateTo );

				OrderResource result = null;
				await ActionPolicies
					.GetAsync
					.Do( async () =>
					{
						result = await this._webRequestServices.SubmitGet< OrderResource >( uri, query )
							.ConfigureAwait( false );
					} );

				return result.Objects;
			}
			catch( Exception ex )
			{
				var truShipEx = new TrueShipCommonException( "Error on GetOrdersAsync", ex );
				this.LogTraceException( "Error. ", truShipEx );
				throw truShipEx;
			}
		}

		public async Task< RemainingOrdersResource > GetRemainingOrders( int? companyId )
		{
			var querystring = string.Format( "bearer_token={0}", this._credentials.AccessToken );
			if( companyId.HasValue )
				querystring += "id=" + companyId;

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "remaining_orders" );

			RemainingOrdersResource data = null;
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					data = await this._webRequestServices.SubmitGet< RemainingOrdersResource >( serviceUrl, querystring ).ConfigureAwait( false );
				} );

			return data;
		}

		public async Task< IEnumerable< TrueShipBox > > GetBoxes( int limit, int offset, int? orderId )
		{
			const string apiEndpoint = "boxes";
			var query = string.Format( "bearer_token={0}&offset={1}&limit={2}&expand=all", this._credentials.AccessToken, offset, limit );

			if( orderId.HasValue )
				query += "&order=" + orderId;

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, apiEndpoint );

			var items = new List< TrueShipBox >();
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response = await this._webRequestServices.SubmitGet< BoxesResource >( serviceUrl, query ).ConfigureAwait( false );

					items = response.Objects;
				} );

			return items;
		}

		public async Task< IEnumerable< TrueShipItem > > GetItems()
		{
			var items = new List< TrueShipItem >();
			var hasMoreItems = false;
			var offset = 0;

			do
			{
				var query = string.Format( "bearer_token={0}&limit={1}&offset={2}", this._credentials.AccessToken, this.Limit, offset);
				var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "items" );

				await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response =
						await this._webRequestServices.SubmitGet< ItemsResource >( serviceUrl, query ).ConfigureAwait( false );

					hasMoreItems = !string.IsNullOrWhiteSpace(response.Meta.Next);
					offset = response.Meta.Offset + 1;

					items.AddRange( response.Objects );
				} );
			} while (hasMoreItems);

			return items;
		}

		public async Task< IEnumerable< TrueShipItem > > GetItems( int limit, int offset, int? boxId = null )
		{
			var query = string.Format( "bearer_token={0}&limit={1}&offset={2}", this._credentials.AccessToken, limit, offset );
			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "items" );

			var items = new List< TrueShipItem >();

			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response =
						await this._webRequestServices.SubmitGet< ItemsResource >( serviceUrl, query ).ConfigureAwait( false );
					items.AddRange( response.Objects );
				} );

			return items;
		}

		public async Task< IEnumerable< Company > > GetCompanies( int offset )
		{
			//Accounts with only 1 company must use this call
			const string apiendpoint = "company";
			string querystring = string.Format( "bearer_token={0}&limit={1}&offset={2}", this._credentials.AccessToken, this.Limit, offset );

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, apiendpoint );

			var companyList = new List< Company >();

			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response = await this._webRequestServices.SubmitGet< CompanyResource >( serviceUrl, querystring ).ConfigureAwait( false );
					companyList = response.Objects;
				} );

			return companyList;
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetAllOrdersByDate( int id, string datefield, DateTime lastsync )
		{
			string EXPAND = "all";
			string formatteddate = string.Format( "{0:s}",
				lastsync );
			string filter = String.Format( "id={0}&{1}__gte={2}",
				id,
				datefield,
				formatteddate );
			string APIENDPOINT = "orders";
			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
			var querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}", this.Format, this.Limit,
				this._credentials.AccessToken,
				EXPAND,
				filter );
			var listOrders = new List< OrderResource.TrueShipOrder >();
			int grabNumber = 1;

			IEnumerable< OrderResource.TrueShipOrder > orders = new List< OrderResource.TrueShipOrder >();
			var response = new OrderResource();
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring ).ConfigureAwait( false );
					orders = response.Objects;
				} );

			listOrders.AddRange( orders );

			decimal totalBatches = ( decimal )response.Meta.TotalCount / this.Limit;
			while( totalBatches - grabNumber > 0 )
			{
				await ActionPolicies.GetAsync.Do( async () =>
				{
					serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
					querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}", this.Format, this.Limit,
						this._credentials.AccessToken,
						EXPAND,
						filter );

					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring ).ConfigureAwait( false );

					orders = response.Objects;
				} );

				listOrders.AddRange( orders );
				grabNumber++;
			}

			return listOrders;
		}

		public async Task< List< OrderResource.TrueShipOrder > > GetOrdersByDateByShipStatus( string bearertoken, int id, string datefield, string shippingstatus, DateTime lastsync )
		{
			string EXPAND = "boxes,boxes__items";
			string formatteddate = string.Format( "{0:s}",
				lastsync );
			string FILTER = String.Format( "id={0}&status_shipped={1}&{2}__gte={3}",
				id,
				shippingstatus,
				datefield,
				formatteddate );
			string APIENDPOINT = "orders";

			var listJsonResponses = new List< OrderResource.TrueShipOrder >();
			int grabNumber = 1;
			IEnumerable< OrderResource.TrueShipOrder > orders = new List< OrderResource.TrueShipOrder >();
			OrderResource response = null;

			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
					var querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}", this.Format, this.Limit,
						bearertoken,
						EXPAND,
						FILTER );

					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring ).ConfigureAwait( false );

					orders = response.Objects;
				} );

			listJsonResponses.AddRange( orders );

			var totalBatches = ( decimal )response.Meta.TotalCount / this.Limit;
			while( totalBatches - grabNumber > 0 )
			{
				var querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}",
					this.Format,
					this.Limit,
					bearertoken,
					EXPAND,
					FILTER );

				await ActionPolicies.GetAsync.Do( async () =>
				{
					var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring ).ConfigureAwait( false );
				} );

				listJsonResponses.AddRange( response.Objects );
				grabNumber++;
			}
			return listJsonResponses;
		}

		public async Task< List< TrueShipItem > > GetUnshippedOrderItemsAfterDateTime( int id, string datefilter, DateTime lastsync )
		{
			const string statusShipped = "False";
			var boxList = new List< TrueShipItem >();

			await ActionPolicies.GetAsync.Do( async () =>
			{
				var jsonResponseList =
					await this.GetOrdersByDateByShipStatus( this._credentials.AccessToken, id, datefilter, statusShipped, lastsync ).
						ConfigureAwait( false );

				if( jsonResponseList != null )
				{
					foreach( var order in jsonResponseList )
					{
						foreach( var box in order.Boxes )
						{
							var boxesItems = box.Items;
							if( boxesItems != null )
								boxList.AddRange( boxesItems );
						}
					}
				}
			} );

			return boxList;
		}

		public async Task< Boolean > UpdateOrderItemPickLocations( IEnumerable< KeyValuePair< string, PickLocation > > orderitemlist )
		{
			var client = new HttpClient();

			foreach( var oneorderitem in orderitemlist )
			{
				var request = this._webRequestServices.CreateUpdateOrderItemPickLocationRequest( oneorderitem );

				try
				{
					HttpResponseMessage response = null;
					await ActionPolicies.GetAsync.Do( async () =>
					{
						response = await client.SendAsync( request ).ConfigureAwait( false );
					} );

					if( response.StatusCode == HttpStatusCode.Accepted )
						continue;
					if( response.StatusCode == HttpStatusCode.Unauthorized )
						throw new TrueShipAuthException( "Unauthorized", new Exception() );

					throw new TrueShipCommonException( response.ReasonPhrase );
				}
				catch( WebException )
				{
					return false;
				}
			}
			return true;
		}

		public async Task< OrderResource.TrueShipOrder > GetOrder( string id )
		{
			string EXPAND = "all";
			string filter = "primary_id=" + id;
			string APIENDPOINT = "orders";
			var querystring = string.Format( "format={0}&&bearer_token={1}&expand={2}&{3}",
				this.Format,
				this._credentials.AccessToken,
				EXPAND,
				filter );

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );

			OrderResource.TrueShipOrder data = null;
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring ).ConfigureAwait( false );
					data = response.Objects.Single();
				} );

			return data;
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrders( DateTime lastSync )
		{
			const string datefilter = "updated_at";

			return await this.GetAllOrdersByDate( this._credentials.CompanyId, datefilter, lastSync );
		}

		private void LogTraceException( string message, TrueShipException exception )
		{
			TrueShipLogger.Log().Trace( exception, message );
		}
	}
}