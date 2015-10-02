using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Netco.Extensions;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;
using TrueShipAccess.WebServices;

namespace TrueShipAccess
{
	public sealed class TrueShipCommonService : ITrueShipCommonService
	{
		private readonly TrueShipConfiguration _config;
		private readonly IWebRequestServices _webRequestServices;
		private string _format = "JSON";
		private int _limit = 10;

		public TrueShipCommonService( TrueShipConfiguration config, IWebRequestServices webRequestServices )
		{
			Condition.Requires( config, "config" ).IsNotNull();
			Condition.Requires( webRequestServices, "webRequestServices" ).IsNotNull();

			this._config = config;
			this._webRequestServices = webRequestServices;
		}

		public TrueShipCommonService( TrueShipConfiguration config )
			: this( config, new WebRequestServices( config ) )
		{
		}

		public string Format
		{
			get { return this._format; }
			set { this._format = value; }
		}

		public int Limit
		{
			get { return this._limit; }
			set { this._limit = value; }
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetAllOrdersByDate( string datefield, DateTime lastsync, CancellationToken ct )
		{
			var EXPAND = "all";
			var formatteddate = string.Format( "{0:s}",
				lastsync );
			var filter = string.Format( "{0}__gte={1}",
				datefield,
				formatteddate );
			var APIENDPOINT = "orders";
			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
			var querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}", this.Format, this.Limit,
				this._config.Credentials.AccessToken,
				EXPAND,
				filter );
			var listOrders = new List< OrderResource.TrueShipOrder >();
			var grabNumber = 1;

			IEnumerable< OrderResource.TrueShipOrder > orders = new List< OrderResource.TrueShipOrder >();
			var response = new OrderResource();
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );
					orders = response.Objects;
				} );

			listOrders.AddRange( orders );

			var totalBatches = ( decimal )response.Meta.TotalCount / this.Limit;
			while( totalBatches - grabNumber > 0 )
			{
				await ActionPolicies.GetAsync.Do( async () =>
				{
					serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );
					querystring = string.Format( "format={0}&limit={1}&bearer_token={2}&expand={3}&{4}", this.Format, this.Limit,
						this._config.Credentials.AccessToken,
						EXPAND,
						filter );

					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );

					orders = response.Objects;
				} );

				listOrders.AddRange( orders );
				grabNumber++;
			}

			return listOrders;
		}

		public async Task< IEnumerable< TrueShipBox > > GetBoxes( int limit, int offset, CancellationToken ct, int? orderId = null )
		{
			const string apiEndpoint = "boxes";

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, apiEndpoint );

			var boxs = new List< TrueShipBox >();
			var hasMoreBoxes = false;

			do
			{
				var query = string.Format( "bearer_token={0}&offset={1}&expand=all", this._config.Credentials.AccessToken, offset );

				if( orderId.HasValue )
					query += "&order=" + orderId;

				await ActionPolicies
					.GetAsync
					.Do( async () =>
					{
						var response = await this._webRequestServices.SubmitGet< BoxesResource >( serviceUrl, query, ct ).ConfigureAwait( false );

						hasMoreBoxes = !string.IsNullOrWhiteSpace( response.Meta.Next );
						offset = response.Meta.Offset + 1;

						boxs.AddRange( response.Objects );
					} );
			} while( hasMoreBoxes );

			return boxs;
		}

		public async Task< IEnumerable< Company > > GetCompanies( int offset, CancellationToken ct )
		{
			//Accounts with only 1 company must use this call
			const string apiendpoint = "company";
			var querystring = string.Format( "bearer_token={0}&limit={1}&offset={2}", this._config.Credentials.AccessToken, this.Limit, offset );

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, apiendpoint );

			var companyList = new List< Company >();

			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response = await this._webRequestServices.SubmitGet< CompanyResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );
					companyList = response.Objects;
				} );

			return companyList;
		}

		public async Task< IEnumerable< TrueShipItem > > GetItems( CancellationToken ct )
		{
			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "items" );
			var items = new List< TrueShipItem >();
			var hasMoreItems = false;
			var offset = 0;

			do
			{
				var query = string.Format( "bearer_token={0}&offset={1}", this._config.Credentials.AccessToken, offset );

				await ActionPolicies
					.GetAsync
					.Do( async () =>
					{
						var response =
							await this._webRequestServices.SubmitGet< ItemsResource >( serviceUrl, query, ct ).ConfigureAwait( false );

						hasMoreItems = !string.IsNullOrWhiteSpace( response.Meta.Next );
						offset = response.Meta.Offset + 1;

						items.AddRange( response.Objects );
					} );
			} while( hasMoreItems );

			return items;
		}

		public async Task< IEnumerable< TrueShipItem > > GetItems( int limit, int offset, CancellationToken ct, int? boxId = null )
		{
			var query = string.Format( "bearer_token={0}&limit={1}&offset={2}", this._config.Credentials.AccessToken, limit, offset );
			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "items" );

			var items = new List< TrueShipItem >();

			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response =
						await this._webRequestServices.SubmitGet< ItemsResource >( serviceUrl, query, ct ).ConfigureAwait( false );
					items.AddRange( response.Objects );
				} );

			return items;
		}

		public async Task< OrderResource.TrueShipOrder > GetOrder( string orderId, CancellationToken ct )
		{
			var EXPAND = "all";
			var filter = "primary_id=" + orderId;
			var APIENDPOINT = "orders";
			var querystring = string.Format( "format={0}&&bearer_token={1}&expand={2}&{3}",
				this.Format,
				this._config.Credentials.AccessToken,
				EXPAND,
				filter );

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, APIENDPOINT );

			OrderResource.TrueShipOrder data = null;
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					var response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );
					data = response.Objects.Single();
				} );

			return data;
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrders( DateTime lastSync, CancellationToken ct )
		{
			const string datefilter = "updated_at";

			return await this.GetAllOrdersByDate( datefilter, lastSync, ct );
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo, CancellationToken ct )
		{
			try
			{
				var uri = string.Format( "{0}/{1}", this._config.ApiBaseUri, "orders" );
//				var query = string.Format( "bearer_token={0}&updated_at__gte={1:s}&updated_at__lte={2:s}", this._config.Credentials.AccessToken, dateFrom, dateTo );
				var query = string.Format( "bearer_token={0}&expand=all", this._config.Credentials.AccessToken );
				var absoluteUri = WebRequestServices.MakeAbsoluteUri( uri, query ); 

				var ordersAccumulator = new List< OrderResource.TrueShipOrder >();
				do
				{
					OrderResource result = null;
					await ActionPolicies
						.GetAsync
						.Do( async () =>
						{
							result = await this._webRequestServices.SubmitGet< OrderResource >( absoluteUri, ct )
								.ConfigureAwait( false );
						} );
					absoluteUri = ( result.Meta.Next != null ) ? new Uri( result.Meta.Next ) : null;
					ordersAccumulator.AddRange( result.Objects );
				} while ( absoluteUri != null );

				return ordersAccumulator;
			}
			catch( Exception ex )
			{
				var truShipEx = new TrueShipCommonException( "Error on GetOrdersAsync", ex );
				this.LogTraceException( "Error. ", truShipEx );
				throw truShipEx;
			}
		}

		public IEnumerable< OrderResource.TrueShipOrder > GetOrders( DateTime dateFrom, DateTime dateTo )
		{
			try
			{
				var uri = string.Format( "{0}/{1}", this._config.ApiBaseUri, "orders" );
				var query = string.Format( "bearer_token={0}&updated_at__gte={1:s}&updated_at__lte={2:s}", this._config.Credentials.AccessToken, dateFrom, dateTo );

				OrderResource result = null;
				ActionPolicies
					.Get
					.Do( () =>
					{
						result = this._webRequestServices.SubmitGetBlocking< OrderResource >( uri, query );
					} );

				return result.Objects;
			}
			catch( Exception ex )
			{
				var truShipEx = new TrueShipCommonException( "Error on GetOrders", ex );
				this.LogTraceException( "Error. ", truShipEx );
				throw truShipEx;
			}
		}

		public async Task< List< OrderResource.TrueShipOrder > > GetOrdersByDateByShipStatus( string bearertoken, int id, string datefield, string shippingstatus, DateTime lastsync, CancellationToken ct )
		{
			var EXPAND = "boxes,boxes__items";
			var formatteddate = string.Format( "{0:s}",
				lastsync );
			var FILTER = string.Format( "id={0}&status_shipped={1}&{2}__gte={3}",
				id,
				shippingstatus,
				datefield,
				formatteddate );
			var APIENDPOINT = "orders";

			var listJsonResponses = new List< OrderResource.TrueShipOrder >();
			var grabNumber = 1;
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

					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );

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
					response = await this._webRequestServices.SubmitGet< OrderResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );
				} );

				listJsonResponses.AddRange( response.Objects );
				grabNumber++;
			}
			return listJsonResponses;
		}

		public async Task< RemainingOrdersResource > GetRemainingOrders( CancellationToken ct, int? companyId = null )
		{
			var querystring = string.Format( "bearer_token={0}", this._config.Credentials.AccessToken );
			if( companyId.HasValue )
				querystring += "id=" + companyId;

			var serviceUrl = string.Format( "{0}/{1}", this._config.ApiBaseUri, "remaining_orders" );

			RemainingOrdersResource data = null;
			await ActionPolicies
				.GetAsync
				.Do( async () =>
				{
					data = await this._webRequestServices.SubmitGet< RemainingOrdersResource >( serviceUrl, querystring, ct ).ConfigureAwait( false );
				} );

			return data;
		}

		public async Task< List< TrueShipItem > > GetUnshippedOrderItemsAfterDateTime( int id, string datefilter, DateTime lastsync, CancellationToken ct )
		{
			const string statusShipped = "False";
			var boxList = new List< TrueShipItem >();

			await ActionPolicies.GetAsync.Do( async () =>
			{
				var jsonResponseList =
					await this.GetOrdersByDateByShipStatus( this._config.Credentials.AccessToken, id, datefilter, statusShipped, lastsync, ct ).
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

		public async Task< bool > UpdateOrderItemPickLocations( IEnumerable< KeyValuePair< string, PickLocation > > orderitemlist, CancellationToken ctx )
		{
			var client = new HttpClient();

			var requestResults = await orderitemlist.ProcessInBatchAsync( 20, async oneorderitem =>
			{
				var request = this._webRequestServices.CreateUpdateOrderItemPickLocationRequest( oneorderitem );

				try
				{
					HttpResponseMessage response = null;
					await ActionPolicies.GetAsync.Do( async () =>
					{
						response = await client.SendAsync( request, ctx ).ConfigureAwait( false );
					} );

					if ( response.StatusCode == HttpStatusCode.Unauthorized )
						throw new TrueShipAuthException( "Unauthorized", new Exception() );
				}
				catch ( WebException )
				{
					return false;
				}
				return true;
			} );

			return requestResults.All( x => x );
		}

		private void LogTraceException( string message, TrueShipException exception )
		{
			TrueShipLogger.Log().Trace( exception, message );
		}
	}
}