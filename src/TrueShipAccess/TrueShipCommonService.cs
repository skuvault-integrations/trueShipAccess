using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Netco.Extensions;
using TrueShipAccess.Misc;
using TrueShipAccess.Models;
using TrueShipAccess.WebServices;
using Netco.Logging;
using ServiceStack;

namespace TrueShipAccess
{
	public sealed class TrueShipCommonService : ITrueShipCommonService
	{
		private readonly IWebRequestServices _webRequestServices;
		private readonly TrueShipLogger _logservice = new TrueShipLogger();
		private readonly RequestCreatorService _requestService;
		private readonly PaginationService _paginationService;

		public TrueShipCommonService( TrueShipConfiguration config, IWebRequestServices webRequestServices )
		{
			Condition.Requires( config, "config" ).IsNotNull();
			Condition.Requires( webRequestServices, "webRequestServices" ).IsNotNull();

			this._webRequestServices = webRequestServices;
			this._requestService = new RequestCreatorService( config.Credentials.AccessToken, config.OrganizationKey );
			this._paginationService = new PaginationService( this._webRequestServices );
		}

		public TrueShipCommonService( TrueShipConfiguration config )
			: this( config, new WebRequestServices( config, new ThrottlerAsync() ) )
		{
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo, CancellationToken ct, Mark mark )
		{
			var request = this._requestService.CreateGetOrdersRequest( dateFrom, dateTo );
			var logPrefix = TrueShipLogger.CreateMethodCallInfo( request.GetRequestUri(), mark );

			var result = ( await this._paginationService.GetPaginatedResult< OrderResource.TrueShipOrder >( request, logPrefix, ct ).ConfigureAwait( false ) ).ToList();
			this._logservice.LogTrace( logPrefix, string.Format( "Done. Retrived {0} orders: {1}", result.Count, result.ToJson() ) );
		
			return result;
		}

		public async Task< IEnumerable< OrderResource.TrueShipOrder > > GetUnshippedOrdersAsync( DateTime dateTo, CancellationToken ct, Mark mark )
		{
			var request = this._requestService.CreateGetUnshippedOrdersRequest( dateTo );
			var logPrefix = TrueShipLogger.CreateMethodCallInfo( request.GetRequestUri(), mark );

			var result = ( await this._paginationService.GetPaginatedResult< OrderResource.TrueShipOrder >( request, logPrefix, ct ).ConfigureAwait( false ) ).ToList();
			this._logservice.LogTrace( logPrefix, string.Format( "Done. Retrived {0} orders: {1}", result.Count, result.ToJson() ) );

			return result;
		}

		public IEnumerable< OrderResource.TrueShipOrder > GetOrders( DateTime dateFrom, DateTime dateTo, Mark mark )
		{
			var request = this._requestService.CreateGetOrdersRequest( dateFrom, dateTo );
			var logPrefix = TrueShipLogger.CreateMethodCallInfo( request.GetRequestUri(), mark );

			var result = (this._paginationService.GetPaginatedResultBlocking< OrderResource.TrueShipOrder >( request, logPrefix ) ).ToList();
			this._logservice.LogTrace( logPrefix, string.Format( "Done. Retrived {0} orders: {1}", result.Count, result.ToJson() ) );
		
			return result;
		}

		public async Task< bool > UpdateOrderItemPickLocations( IEnumerable< ItemLocationUpdateModel > orderitemlist, CancellationToken ctx, Mark mark )
		{
			string logPrefix;
			var requestResults = await orderitemlist.ProcessInBatchAsync( 20, async updateModel =>
			{
				var request = this._requestService.CreateUpdatePickLocationRequest( updateModel );
				logPrefix = TrueShipLogger.CreateMethodCallInfo( request.GetRequestUri(), mark );
				try
				{
					this._logservice.LogTrace( logPrefix, string.Format( "Started sending request to update item {0} location to {1}", updateModel.Sku, updateModel.Location.Location ) );
					var response = await this._webRequestServices.SubmitPatch( request, logPrefix, ctx ).ConfigureAwait( false );

					this._logservice.LogTrace( logPrefix, string.Format( "Got response for item {0}, result: {1}", updateModel.Resource, response.StatusCode ) );
					if( response.StatusCode == HttpStatusCode.Unauthorized )
						throw new TrueShipAuthException( "Unauthorized", new Exception() );
				}
				catch( WebException )
				{
					return false;
				}
				return true;
			} );
			return requestResults.All( x => x );
		}
	}
}