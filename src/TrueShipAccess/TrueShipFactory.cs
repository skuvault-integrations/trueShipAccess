using CuttingEdge.Conditions;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public sealed class TrueShipFactory : ITrueShipFactory
	{
		private readonly string _appId;

		public TrueShipFactory( string appId )
		{
			Condition.Requires( appId, "appId" ).IsNotNull();
			this._appId = appId;
		}

		public ITrueShipCommonService CreateCommonService( TrueShipConfiguration config )
		{
			return new TrueShipCommonService( config );
		}

		public ITrueShipAuthService CreateAuthService()
		{
			return new TrueShipAuthService( this._appId );
		}
	}
}