using CuttingEdge.Conditions;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipCredentials
	{
		public TrueShipCredentials( string accessToken )
		{
			Condition.Requires( accessToken ).IsNotNullOrWhiteSpace();

			this.AccessToken = accessToken;
		}

		public string AccessToken { get; private set; }
	}
}