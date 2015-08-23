using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess.WebServices
{
	public interface IWebRequestServices
	{
		Task< T > SubmitGet< T >( string serviceUrl, string querystring, CancellationToken ct ) where T : class;
		HttpRequestMessage CreateUpdateOrderItemPickLocationRequest( KeyValuePair< string, PickLocation > oneorderitem );
	}
}