using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Netco.Extensions;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public abstract class AbstractTrueShipRequest
	{
		protected readonly Dictionary< string, string > UrlParams = new Dictionary< string, string >();

		protected TrueShipApiEndpoint Endpoint;
		protected string OrganizationKey;

		public AbstractTrueShipRequest( TrueShipApiEndpoint endpoint, string organizationKey )
		{
			Condition.Requires( organizationKey, "organizationKey" ).IsNotNullOrWhiteSpace();

			this.Endpoint = endpoint;
			this.OrganizationKey = organizationKey;
		}

		public AbstractTrueShipRequest SetField( TrueShipField field, string value )
		{
			this.UrlParams[ field.FieldName ] = value;
			return this;
		}

		public virtual Uri GetRequestUri()
		{
			var apiPrefix = string.Format( "{0}/orgs/{1}/{2}/", TrueShipConventions.ApiBaseUri, this.OrganizationKey, this.Endpoint.Endpoint );
			var querystring = string.Join( "&", this.UrlParams.Select( kv => "{0}={1}".FormatWith( kv.Key, kv.Value ) ) );

			return new Uri( string.Format( "{0}?{1}",
				apiPrefix,
				querystring ) );
		}
	}
}
