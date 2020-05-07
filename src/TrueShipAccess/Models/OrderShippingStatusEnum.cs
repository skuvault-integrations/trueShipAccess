namespace TrueShipAccess.Models
{
	public enum TrueShipOrderShippingStatusEnum
	{
		Unshipped,
		Unfulfilled,
		Fulfilled, 
		PartiallyFulfilled,
		Undefined
	}

	public static class OrderShippingStatusExtensions
	{
		public const string ShippingStatusFulfilled = "fulfilled";

		public static TrueShipOrderShippingStatusEnum ToShippingStatus( this string shippingStatus )
		{
			switch( shippingStatus )
			{
				case "unshipped":
					return TrueShipOrderShippingStatusEnum.Unshipped;
				case "unfulfilled":
					return TrueShipOrderShippingStatusEnum.Unfulfilled;
				case ShippingStatusFulfilled:
					return TrueShipOrderShippingStatusEnum.Fulfilled;
				case "partially_fulfilled":
					return TrueShipOrderShippingStatusEnum.PartiallyFulfilled;
				default:
					return TrueShipOrderShippingStatusEnum.Undefined;
			}
		}
	}
}