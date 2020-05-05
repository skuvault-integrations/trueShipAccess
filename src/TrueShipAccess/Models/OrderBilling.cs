using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderBilling
	{
		[ DataMember( Name = "status" ) ]
		public string StatusValue { get; set; }
		public OrderBillingStatusEnum Status => StatusValue.ToBillingStatus();

		[ DataMember( Name = "subtotal" ) ]
		public string SubtotalValue { get; set; }
		public TrueShipMoney Subtotal => SubtotalValue.ToTrueShipMoney();

		[ DataMember( Name = "shipping" ) ]
		public string ShippingValue { get; set; }
		public TrueShipMoney Shipping => ShippingValue.ToTrueShipMoney();

		[ DataMember( Name = "tax" )]
		public string TaxValue { get; set; }
		public TrueShipMoney Tax => TaxValue.ToTrueShipMoney();

		[ DataMember( Name = "total" )]
		public string TotalValue { get; set; }
		public TrueShipMoney Total => TotalValue.ToTrueShipMoney();
	}

	public enum OrderBillingStatusEnum
	{
		Pending,
		Paid,
		PartiallyPaid,
		Refunded,
		PartiallyRefunded,
		Cancelled,
		Undefined
	}

	public static class OrderBillingStatusExtensions
	{
		public static OrderBillingStatusEnum ToBillingStatus( this string billingStatus )
		{
			switch( billingStatus )
			{
				case "pending":
					return OrderBillingStatusEnum.Pending;
				case "paid":
					return OrderBillingStatusEnum.Paid;
				case "partially_paid":
					return OrderBillingStatusEnum.PartiallyPaid;
				case "refunded":
					return OrderBillingStatusEnum.Refunded;
				case "partially_refunded":
					return OrderBillingStatusEnum.PartiallyRefunded;
				case "cancelled":
					return OrderBillingStatusEnum.Cancelled;
				default:
					return OrderBillingStatusEnum.Undefined;
			}
		}
	}
}