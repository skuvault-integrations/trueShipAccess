using System;

namespace TrueShipAccess.Models
{
	public class TrueShipConfiguration
	{
		public DateTime LastOrderSync { get; set; }
		public DateTime LastLocationSync { get; set; }

		public string ApiBaseUri
		{
			get { return this.ServiceBaseUri + "/api/v1"; }
		}

		public string ServiceBaseUri { get; private set; }

		public TrueShipConfiguration( DateTime lastOrderSync, DateTime lastLocationSync )
		{
			this.LastOrderSync = lastOrderSync;
			this.LastLocationSync = lastLocationSync;

			this.ServiceBaseUri = "https://www.readycloud.com";
		}
	}
}