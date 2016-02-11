using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess.WebServices
{
	public interface IWebRequestServices
	{
		HttpRequestMessage CreateUpdateOrderItemPickLocationRequest( KeyValuePair< string, PickLocation > oneorderitem );
		Task< T > SubmitGet< T >( TrueShipGetRequestBase request, CancellationToken ct, string logPrefix ) where T : class;
		Task< T > SubmitGet< T >( Uri absoluteUri, CancellationToken ct, string logPrefix ) where T : class;
		T SubmitGetBlocking< T >( Uri uri, string logPrefix ) where T : class;
		T SubmitGetBlocking< T >( TrueShipGetRequestBase trueShipRequest, string logPrefix ) where T : class;
		Task<HttpResponseMessage> SubmitPatch( TrueShipPatchRequestBase request, CancellationToken ct, string logPrefix );
	}
}