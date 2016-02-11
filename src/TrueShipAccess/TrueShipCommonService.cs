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
using TrueShipAccess.Extensions;

namespace TrueShipAccess
{
	public sealed class TrueShipCommonService : MethodLogging, ITrueShipCommonService
	{
		private readonly TrueShipConfiguration _config;
		private readonly IWebRequestServices _webRequestServices;
		private readonly TrueShipLogger _logservice = new TrueShipLogger();
		private readonly RequestCreatorService _requestService;
		private readonly PaginationService _paginationService;

		public TrueShipCommonService( TrueShipConfiguration config, IWebRequestServices webRequestServices )
		{
			Condition.Requires( config, "config" ).IsNotNull();
			Condition.Requires( webRequestServices, "webRequestServices" ).IsNotNull();

			this._config = config;
			this._webRequestServices = webRequestServices;
			this._requestService = new RequestCreatorService( config.Credentials.AccessToken );
			this._paginationService = new PaginationService( this._webRequestServices );
		}

		public TrueShipCommonService( TrueShipConfiguration config )
			: this( config, new WebRequestServices( config ) )
		{
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo, CancellationToken ct )
		{
			var request = this._requestService.CreateGetOrdersRequest( dateFrom, dateTo );
			var logPrefix = this.GetLogPrefix( this._config, "start date: {0}, end date {1}".FormatWith( dateFrom, dateTo ) );

			var result = ( await this._paginationService.GetPaginatedResult< OrderResource.TrueShipOrder >( request, logPrefix, ct ) ).ToList();
			this._logservice.LogTrace( logPrefix, "Done. Retrived {0} orders: {1}".FormatWith( result.Count, result.MakeString() ) );
		
			return result;
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetUnshippedOrdersAsync( DateTime dateTo, CancellationToken ct )
		{
			var request = this._requestService.CreateGetUnshippedOrdersRequest( dateTo );
			var logPrefix = this.GetLogPrefix( this._config, "end date {0}".FormatWith( dateTo ) );

			var result = ( await this._paginationService.GetPaginatedResult< OrderResource.TrueShipOrder >( request, logPrefix, ct ) ).ToList();
			this._logservice.LogTrace( logPrefix, "Done. Retrived {0} orders: {1}".FormatWith( result.Count, result.MakeString() ) );

			return result;
		}

		public IEnumerable< OrderResource.TrueShipOrder > GetOrders( DateTime dateFrom, DateTime dateTo )
		{
			var request = this._requestService.CreateGetOrdersRequest( dateFrom, dateTo );
			var logPrefix = this.GetLogPrefix( this._config, "start date: {0}, end date {0}".FormatWith( dateFrom, dateTo ) );

			var result = (this._paginationService.GetPaginatedResultBlocking< OrderResource.TrueShipOrder >( request, logPrefix ) ).ToList();
			this._logservice.LogTrace( logPrefix, "Done. Retrived {0} orders: {1}".FormatWith( result.Count, result.MakeString() ) );
		
			return result;
		}

		public async Task< bool > UpdateOrderItemPickLocations( IEnumerable< ItemLocationUpdateModel > orderitemlist, CancellationToken ctx )
		{
			var logPrefix = this.GetLogPrefix( this._config, "Update item location data = {0}".FormatWith( orderitemlist.ToList().MakeString() ) );
			var requestResults = await orderitemlist.ProcessInBatchAsync( 20, async updateModel =>
			{
				var request = this._requestService.CreateUpdatePickLocationRequest( updateModel );

				try
				{
					this._logservice.LogTrace( logPrefix, "Started sending request to update item {0} location".FormatWith( updateModel.Location.Location ) );
					var response = await this._webRequestServices.SubmitPatch( request, ctx, logPrefix );

					this._logservice.LogTrace( logPrefix, "Got response for item {0}, result: {1}".FormatWith( updateModel.Resource, response.StatusCode ) );
					if( response.StatusCode == HttpStatusCode.Unauthorized )
						throw new TrueShipAuthException( "Unauthorized", new Exception() );
				}
				catch( WebException )
				{
					return false;
				}
				return true;
			} );
			var result = requestResults.All( x => x );
			this._logservice.LogTrace( logPrefix, "Done. Global result: {0}".FormatWith( result ) );

			return result;
		}
	}
}