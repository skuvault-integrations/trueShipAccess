using System;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipConfiguration
	{
		public DateTime LastLocationSync { get; set; }
		public DateTime LastOrderSync { get; set; }

		public TrueShipCredentials Credentials{ get; private set; }

		public TrueShipConfiguration( TrueShipCredentials credentials )
		{
			this.Credentials = credentials;
		}
	}
}