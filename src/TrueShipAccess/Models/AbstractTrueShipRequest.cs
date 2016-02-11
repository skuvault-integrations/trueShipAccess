using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Netco.Extensions;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public abstract class AbstractTrueShipRequest
	{
		protected readonly Dictionary< string, string > UrlParams = new Dictionary< string, string >();

		protected TrueShipApiEndpoint Endpoint;

		public AbstractTrueShipRequest SetField( TrueShipField field, string value )
		{
			this.UrlParams[ field.FieldName ] = value;
			return this;
		}

		public virtual Uri GetRequestUri()
		{
			var apiPrefix = string.Format( "{0}/{1}", TrueShipConventions.ApiBaseUri, this.Endpoint.Endpoint );
			var querystring = string.Join( "&", this.UrlParams.Select( kv => "{0}={1}".FormatWith( kv.Key, kv.Value ) ) );

			return new Uri( string.Format( "{0}?{1}",
				apiPrefix,
				querystring ) );
		}
	}
}
