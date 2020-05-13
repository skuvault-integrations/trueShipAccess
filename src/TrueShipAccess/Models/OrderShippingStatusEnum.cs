using System.Collections.Generic;

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

		private static readonly Dictionary< string, TrueShipOrderShippingStatusEnum > ShippingStatusMappings = 
			new Dictionary< string, TrueShipOrderShippingStatusEnum >
			{
				{ "unshipped", TrueShipOrderShippingStatusEnum.Unshipped },
				{ "unfulfilled", TrueShipOrderShippingStatusEnum.Unfulfilled },
				{ ShippingStatusFulfilled, TrueShipOrderShippingStatusEnum.Fulfilled },
				{ "partially_fulfilled", TrueShipOrderShippingStatusEnum.PartiallyFulfilled }
			};

		public static TrueShipOrderShippingStatusEnum ToShippingStatus( this string shippingStatus )
		{
			if( ShippingStatusMappings.ContainsKey( shippingStatus ) )
				return ShippingStatusMappings[ shippingStatus ];
			return TrueShipOrderShippingStatusEnum.Undefined;
		}
	}
}