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
		Task< T > SubmitGet< T >( string serviceUrl, string querystring, CancellationToken ct ) where T : class;
		Task< T > SubmitGet< T >( Uri absoluteUri, CancellationToken ct ) where T : class;
		T SubmitGetBlocking< T >( string serviceUrl, string querystring ) where T : class;
	}
}