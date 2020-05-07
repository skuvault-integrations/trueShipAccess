using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderBilling
	{
		[ DataMember( Name = "status" ) ]
		public string StatusValue { get; set; }
		public TrueShipOrderBillingStatusEnum GetStatus()
		{
			return StatusValue.ToBillingStatus();
		}

		[ DataMember( Name = "subtotal" ) ]
		public string SubtotalAmountValue { get; set; }
		public TrueShipMoney GetSubtotalAmount()
		{
			return SubtotalAmountValue.ToTrueShipMoney();
		}

		[ DataMember( Name = "shipping" ) ]
		public string ShippingAmountValue { get; set; }
		public TrueShipMoney GetShippingAmount()
		{
			return ShippingAmountValue.ToTrueShipMoney();
		}

		[ DataMember( Name = "tax" )]
		public string TaxAmountValue { get; set; }
		public TrueShipMoney GetTaxAmount()
		{
			return TaxAmountValue.ToTrueShipMoney();
		}

		[ DataMember( Name = "total" )]
		public string TotalAmountValue { get; set; }
		public TrueShipMoney GetTotalAmount()
		{
			return TotalAmountValue.ToTrueShipMoney();
		}
	}

	public enum TrueShipOrderBillingStatusEnum
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
		public static TrueShipOrderBillingStatusEnum ToBillingStatus( this string billingStatus )
		{
			switch( billingStatus )
			{
				case "pending":
					return TrueShipOrderBillingStatusEnum.Pending;
				case "paid":
					return TrueShipOrderBillingStatusEnum.Paid;
				case "partially_paid":
					return TrueShipOrderBillingStatusEnum.PartiallyPaid;
				case "refunded":
					return TrueShipOrderBillingStatusEnum.Refunded;
				case "partially_refunded":
					return TrueShipOrderBillingStatusEnum.PartiallyRefunded;
				case "cancelled":
					return TrueShipOrderBillingStatusEnum.Cancelled;
				default:
					return TrueShipOrderBillingStatusEnum.Undefined;
			}
		}
	}
}