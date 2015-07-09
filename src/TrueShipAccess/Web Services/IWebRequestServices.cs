using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface IWebRequestServices
	{
		Task< T > SubmitGet< T >( string serviceUrl, string querystring ) where T : class;
		HttpRequestMessage CreateUpdateOrderItemPickLocationRequest( KeyValuePair< string, PickLocation > oneorderitem );
	}
}