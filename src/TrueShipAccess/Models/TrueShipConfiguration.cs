using System;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipConfiguration
	{
		public TrueShipConfiguration( DateTime lastOrderSync, DateTime lastLocationSync )
		{
			this.LastOrderSync = lastOrderSync;
			this.LastLocationSync = lastLocationSync;

			this.ServiceBaseUri = "https://www.readycloud.com";
		}

		public string ApiBaseUri
		{
			get { return this.ServiceBaseUri + "/api/v1"; }
		}

		public DateTime LastLocationSync { get; set; }
		public DateTime LastOrderSync { get; set; }
		public string ServiceBaseUri { get; private set; }
	}
}