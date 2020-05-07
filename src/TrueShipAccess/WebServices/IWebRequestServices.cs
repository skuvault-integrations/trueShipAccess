using Netco.Logging;
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
		Task< T > SubmitGet< T >( TrueShipGetRequestBase request, string logPrefix, CancellationToken ct ) where T : class;
		Task< T > SubmitGet< T >( Uri absoluteUri, string logPrefix, CancellationToken ct ) where T : class;
		T SubmitGetBlocking< T >( Uri uri, string logPrefix ) where T : class;
		T SubmitGetBlocking< T >( TrueShipGetRequestBase trueShipRequest, string logPrefix ) where T : class;
		Task< HttpResponseMessage > SubmitPatch( TrueShipPatchRequest request, string logPrefix, CancellationToken ct );
	}
}