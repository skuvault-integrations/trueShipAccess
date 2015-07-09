using CuttingEdge.Conditions;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public sealed class TrueShipFactory : ITrueShipFactory
	{
		private readonly TrueShipConfiguration _config;

		public TrueShipFactory( TrueShipConfiguration config )
		{
			Condition.Requires( config, "config" ).IsNotNull();

			this._config = config;
		}

		public ITrueShipService CreateService( TrueShipCredentials userCredentials )
		{
			return new TrueShipService( userCredentials, this._config );
		}
	}
}