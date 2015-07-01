using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public interface ITrueShipFactory
	{
		ITrueShipService CreateService(TrueShipCredentials userCredentials);
	}
}