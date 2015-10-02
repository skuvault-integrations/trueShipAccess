using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface ITrueShipFactory
	{
		ITrueShipCommonService CreateCommonService( TrueShipConfiguration config );
		ITrueShipAuthService CreateAuthService();
	}
}