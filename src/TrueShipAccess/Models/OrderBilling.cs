using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderBilling
	{
		[ DataMember( Name = "status" ) ]
		public string StatusValue { get; set; }
		public TrueShipOrderBillingStatusEnum Status
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
		private static readonly Dictionary< string, TrueShipOrderBillingStatusEnum > BillingStatusMappings = 
			new Dictionary< string, TrueShipOrderBillingStatusEnum >
			{
				{ "pending", TrueShipOrderBillingStatusEnum.Pending },
				{ "paid", TrueShipOrderBillingStatusEnum.Paid },
				{ "partially_paid", TrueShipOrderBillingStatusEnum.PartiallyPaid },
				{ "refunded", TrueShipOrderBillingStatusEnum.Refunded },
				{ "partially_refunded", TrueShipOrderBillingStatusEnum.PartiallyRefunded },
				{ "cancelled", TrueShipOrderBillingStatusEnum.Cancelled }
			};

		public static TrueShipOrderBillingStatusEnum ToBillingStatus( this string billingStatus )
		{
			if( BillingStatusMappings.ContainsKey( billingStatus ) )
				return BillingStatusMappings[ billingStatus ];
			return TrueShipOrderBillingStatusEnum.Undefined;
		}
	}
}