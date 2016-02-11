namespace TrueShipAccess.Models.Conventions
{
	public class TrueShipApiEndpoint
	{
		public readonly string Endpoint;

		public TrueShipApiEndpoint( string endpoint )
		{
			this.Endpoint = endpoint;
		}
	}

	internal abstract class TrueShipApiEndpoints
	{
		public static readonly TrueShipApiEndpoint Orders = new TrueShipApiEndpoint( "orders" );
		public static readonly TrueShipApiEndpoint Boxes = new TrueShipApiEndpoint( "boxes" );
		public static readonly TrueShipApiEndpoint Company = new TrueShipApiEndpoint( "company" );
		public static readonly TrueShipApiEndpoint Items = new TrueShipApiEndpoint( "items" );
		public static readonly TrueShipApiEndpoint RemainingOrders = new TrueShipApiEndpoint( "remaining_orders" );
	}
}
