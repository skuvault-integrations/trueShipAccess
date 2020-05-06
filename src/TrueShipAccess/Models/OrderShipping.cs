using System;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderShipping
	{
		[ DataMember( Name = "status" ) ]
		public string StatusValue { get; set; }
		public OrderShippingStatusEnum Status => StatusValue.ToShippingStatus();

		[ DataMember( Name = "shipped_at" ) ]
		public string ShippedAtValue { get; set; }
		public DateTime ShippedAt => ShippedAtValue.ToUtcDateTime();

		[ DataMember( Name = "ship_to" ) ]
		public ShipTo ShipTo { get; set; }

		[ DataMember( Name = "ship_type" ) ]
		public string ShipType { get; set; }

		[ DataMember( Name = "ship_via" ) ]
		public string ShipVia { get; set; }

		[ DataMember( Name = "ship_cost" ) ]
		public string ShipCostValue { get; set; }
		public TrueShipMoney ShipCost => ShipCostValue.ToTrueShipMoney();

		[ DataMember( Name = "warehouse" ) ]
		public string Warehouse { get; set; }
	}

	[ DataContract ]
	public class ShipTo
	{
		[ DataMember( Name = "address_1" ) ]
		public string Address1 { get; set; }

		[ DataMember( Name = "address_2" ) ]
		public string Address2 { get; set; }

		[ DataMember( Name = "city" ) ]
		public string City { get; set; }

		[ DataMember( Name = "company" ) ]
		public string Company { get; set; }

		[ DataMember( Name = "country" ) ]
		public string Country { get; set; }

		[ DataMember( Name = "email" ) ]
		public string Email { get; set; }

		[ DataMember( Name = "first_name" ) ]
		public string FirstName { get; set; }

		[ DataMember( Name = "last_name" ) ]
		public string LastName { get; set; }

		public string Order { get; set; }

		[ DataMember( Name = "phone" ) ]
		public string Phone { get; set; }

		[ DataMember( Name = "post_code" ) ]
		public string PostCode { get; set; }

		public bool Residential { get; set; }

		[ DataMember( Name = "state" ) ]
		public string State { get; set; }

		public bool Validated { get; set; }
	}
}
