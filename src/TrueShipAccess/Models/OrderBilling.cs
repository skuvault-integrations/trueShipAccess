using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderBilling
	{
		[ DataMember( Name = "status" ) ]
		public string StatusValue { get; set; }
		public OrderBillingStatusEnum Status
		{
			get
			{
				return StatusValue.ToBillingStatus();
			}
		}

		[ DataMember( Name = "subtotal" ) ]
		public string SubtotalAmountValue { get; set; }
		public TrueShipMoney SubtotalAmount
		{
			get
			{
				return SubtotalAmountValue.ToTrueShipMoney();
			}
		}

		[ DataMember( Name = "shipping" ) ]
		public string ShippingAmountValue { get; set; }
		public TrueShipMoney ShippingAmount
		{
			get
			{
				return ShippingAmountValue.ToTrueShipMoney();
			}
		}

		[ DataMember( Name = "tax" )]
		public string TaxAmountValue { get; set; }
		public TrueShipMoney TaxAmount
		{
			get
			{
				return TaxAmountValue.ToTrueShipMoney();
			}
		}

		[ DataMember( Name = "total" )]
		public string TotalAmountValue { get; set; }
		public TrueShipMoney TotalAmount
		{
			get
			{
				return TotalAmountValue.ToTrueShipMoney();
			}
		}
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