using System;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipConfiguration
	{
		public string ApiBaseUri
		{
			get { return this.ServiceBaseUri + "/api/v1"; }
		}

		public DateTime LastLocationSync { get; set; }
		public DateTime LastOrderSync { get; set; }
		public string ServiceBaseUri { get; private set; }

		public TrueShipCredentials Credentials{ get; private set; }

		public TrueShipConfiguration( TrueShipCredentials credentials )
		{
			this.Credentials = credentials;
			this.ServiceBaseUri = "https://www.readycloud.com";
		}
	}
}