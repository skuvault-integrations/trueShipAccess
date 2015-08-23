using CuttingEdge.Conditions;

namespace TrueShipAccess.Models
{
	public sealed class TrueShipCredentials
	{
		public TrueShipCredentials( int companyId, string accessToken )
		{
			Condition.Requires( accessToken ).IsNotNullOrWhiteSpace();

			this.CompanyId = companyId;
			this.AccessToken = accessToken;
		}

		public string AccessToken { get; private set; }
		public int CompanyId { get; private set; }
	}
}