using CuttingEdge.Conditions;
using System;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipConfiguration
	{
		public DateTime LastLocationSync { get; set; }
		public DateTime LastOrderSync { get; set; }

		public TrueShipCredentials Credentials{ get; private set; }
		public string OrganizationKey { get; private set; }

		public TrueShipConfiguration( TrueShipCredentials credentials, string organizationKey )
		{
			Condition.Requires( organizationKey, "organizationKey" ).IsNotNullOrWhiteSpace();

			this.Credentials = credentials;
			this.OrganizationKey = organizationKey;
		}
	}
}