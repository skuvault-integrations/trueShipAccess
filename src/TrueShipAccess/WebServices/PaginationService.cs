using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			var fistPage = await this._webRequestServices.SubmitGet< TrueShipBaseResponse< T > >( request, ct, logPrefix );
			if( string.IsNullOrEmpty( fistPage.Meta.Next ) )
				return fistPage.Objects;
			objectAccumulator.AddRange( fistPage.Objects );

			var nextPageUri = TrueShipConventions.GetNextPaginationUri( fistPage.Meta );
			while( true )
			{
				var nextPage = await this._webRequestServices.SubmitGet< TrueShipBaseResponse< T > >( nextPageUri, ct, logPrefix );
				objectAccumulator.AddRange( nextPage.Objects );
				if( HasFinishedIteratingPages( nextPage ) )
					break;
				nextPageUri = TrueShipConventions.GetNextPaginationUri( nextPage.Meta );
			}

			return objectAccumulator;
		}

		public IEnumerable< T > GetPaginatedResultBlocking< T >( TrueShipGetRequestBase request, string logPrefix ) where T : class
		{
			var objectAccumulator = new List< T >();
			var fistPage = this._webRequestServices.SubmitGetBlocking< TrueShipBaseResponse< T > >( request, logPrefix );
			if( string.IsNullOrEmpty( fistPage.Meta.Next ) )
				return fistPage.Objects;

			var nextPageUri = TrueShipConventions.GetNextPaginationUri( fistPage.Meta );
			while( true )
			{
				var nextPage = this._webRequestServices.SubmitGetBlocking< TrueShipBaseResponse< T > >( nextPageUri, logPrefix );
				objectAccumulator.AddRange( nextPage.Objects );
				if( HasFinishedIteratingPages( nextPage ) )
					break;
				nextPageUri = TrueShipConventions.GetNextPaginationUri( nextPage.Meta );
			}

			return objectAccumulator;
		}

		private static bool HasFinishedIteratingPages< T >( TrueShipBaseResponse< T > response ) where T : class
		{
			return string.IsNullOrWhiteSpace( response.Meta.Next );
		}
	}
}
