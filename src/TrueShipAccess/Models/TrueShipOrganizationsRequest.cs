using System;
using System.Linq;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public class TrueShipOrganizationsRequest : TrueShipGetRequestBase
	{
		public override Uri GetRequestUri()
		{
			var apiPrefix = string.Format( "{0}/orgs/", TrueShipConventions.ApiBaseUri );
			var querystring = string.Join( "&", this.UrlParams.Select( kv => string.Format( "{0}={1}", kv.Key, kv.Value ) ) );

			return new Uri( string.Format( "{0}?{1}",
				apiPrefix,
				querystring ) );
		}
	}
}
