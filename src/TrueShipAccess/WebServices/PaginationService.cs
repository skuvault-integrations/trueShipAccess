using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrueShipAccess.Models;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.WebServices
{
	class PaginationService
	{
		private readonly IWebRequestServices _webRequestServices;

		public PaginationService( IWebRequestServices webRequestServices )
		{
			this._webRequestServices = webRequestServices;
		}

		public async Task< IEnumerable< T > > GetPaginatedResult< T >( TrueShipGetRequestBase request, string logPrefix, CancellationToken ct ) where T : class
		{
			var objectAccumulator = new List< T >();
			var firstPage = await this._webRequestServices.SubmitGet< TrueShipBaseResponse< T > >( request, ct, logPrefix );
			if ( string.IsNullOrEmpty( firstPage.Next ) )
				return firstPage.Results;
			objectAccumulator.AddRange( firstPage.Results );

			var nextPageUri = TrueShipConventions.GetNextPaginationUri( firstPage.Next );
			while( true )
			{
				var nextPage = await this._webRequestServices.SubmitGet< TrueShipBaseResponse< T > >( nextPageUri, ct, logPrefix );
				objectAccumulator.AddRange( nextPage.Results );
				if( HasFinishedIteratingPages( nextPage ) )
					break;
				nextPageUri = TrueShipConventions.GetNextPaginationUri( nextPage.Next );
			}

			return objectAccumulator;
		}

		public IEnumerable< T > GetPaginatedResultBlocking< T >( TrueShipGetRequestBase request, string logPrefix ) where T : class
		{
			var objectAccumulator = new List< T >();
			var fistPage = this._webRequestServices.SubmitGetBlocking< TrueShipBaseResponse< T > >( request, logPrefix );
			if( string.IsNullOrEmpty( fistPage.Next ) )
				return fistPage.Results;

			var nextPageUri = TrueShipConventions.GetNextPaginationUri( fistPage.Next );
			while( true )
			{
				var nextPage = this._webRequestServices.SubmitGetBlocking< TrueShipBaseResponse< T > >( nextPageUri, logPrefix );
				objectAccumulator.AddRange( nextPage.Results );
				if( HasFinishedIteratingPages( nextPage ) )
					break;
				nextPageUri = TrueShipConventions.GetNextPaginationUri( nextPage.Next );
			}

			return objectAccumulator;
		}

		private static bool HasFinishedIteratingPages< T >( TrueShipBaseResponse< T > response ) where T : class
		{
			return string.IsNullOrWhiteSpace( response.Next );
		}
	}
}
