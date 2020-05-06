namespace TrueShipAccess.Models
{
	public enum OrderShippingStatusEnum
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

		public static OrderShippingStatusEnum ToShippingStatus( this string shippingStatus )
		{
			switch( shippingStatus )
			{
				case "unshipped":
					return OrderShippingStatusEnum.Unshipped;
				case "unfulfilled":
					return OrderShippingStatusEnum.Unfulfilled;
				case ShippingStatusFulfilled:
					return OrderShippingStatusEnum.Fulfilled;
				case "partially_fulfilled":
					return OrderShippingStatusEnum.PartiallyFulfilled;
				default:
					return OrderShippingStatusEnum.Undefined;
			}
		}
	}
}