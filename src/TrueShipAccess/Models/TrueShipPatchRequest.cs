using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Netco.Extensions;
using ServiceStack.Text;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public class TrueShipPatchRequestBase : AbstractTrueShipRequest
	{
		private string serializedBody;

		public TrueShipPatchRequestBase( TrueShipApiEndpoint endpoint )
		{
			this.Endpoint = endpoint;
		}

		public string GetSerializedBody() {
			return this.serializedBody ?? "N/A";
		}

		public TrueShipPatchRequestBase SetBearerToken( string token )
		{
			this.SetField( TrueShipFields.Token, token );
			return this;
		}

		public TrueShipPatchRequestBase SetBody( object body )
		{
			this.serializedBody = JsonSerializer.SerializeToString( body );			
			return this;
		}

		public override Uri GetRequestUri()
		{
			var pathString = string.Format( "{0}{1}", TrueShipConventions.ServiceBaseUri, this.Endpoint.Endpoint );
			var paramString = string.Join( "&", this.UrlParams.Select( kv => "{0}={1}".FormatWith( kv.Key, kv.Value ) ) );

			return new Uri( string.Format( "{0}?{1}",
				pathString,
				paramString ) );
		}

		public HttpRequestMessage ToHttpRequest() {
			var uri = this.GetRequestUri();

			var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), uri )
			{
				Content = new StringContent( this.serializedBody, Encoding.UTF8, "application/json" )
			};

			return request;
			
		}
	}

}
