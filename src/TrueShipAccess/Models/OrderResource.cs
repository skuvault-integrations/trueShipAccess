using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	///     Retrieve a list of orders.
	///     https://www.readycloud.com/static/api-doc/apireference.html#order-resource
	/// </summary>
	public class OrderResource : TrueShipBaseResponse< OrderResource.TrueShipOrder >
	{
		[ DataContract ]
		public class BillTo
		{
			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }
		}

		[ DataContract ]
		public class CustomField
		{
			public string Box { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public string CreatedAt { get; set; }

			public int Id { get; set; }
			public string Name { get; set; }

			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public string UpdatedAt { get; set; }

			public string Value { get; set; }
		}

		[ DataContract ]
		public class ShipFrom
		{
			[ DataMember( Name = "a_type" ) ]
			public string a_type { get; set; }

			[ DataMember( Name = "address_1" ) ]
			public string address_1 { get; set; }

			[ DataMember( Name = "address_2" ) ]
			public object address_2 { get; set; }

			public string City { get; set; }
			public string Company { get; set; }
			public string Country { get; set; }
			public string Email { get; set; }

			[ DataMember( Name = "first_name" ) ]
			public string FirstName { get; set; }

			[ DataMember( Name = "last_name" ) ]
			public string last_name { get; set; }

			public string Order { get; set; }
			public string Phone { get; set; }

			[ DataMember( Name = "post_code" ) ]
			public string post_code { get; set; }

			public bool residential { get; set; }
			public string State { get; set; }
			public bool Validated { get; set; }
		}

		[ DataContract ]
		public class ShipTo
		{
			[ DataMember( Name = "a_type" ) ]
			public string a_type { get; set; }

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

		[ DataContract ]
		public class Source
		{
			public object Account { get; set; }
			public object Channel { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public DateTime created_at { get; set; }

			public object Name { get; set; }
			public string Order { get; set; }

			[ DataMember( Name = "order_source_id" ) ]
			public object OrderSourceId { get; set; }

			public string ResourceUri { get; set; }

			[ DataMember( Name = "retrieved_at" ) ]
			public string RetrievedAt { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public DateTime UpdatedAt { get; set; }
		}

		[ DataContract ]
		public class TrueShipOrder
		{
			[ DataMember( Name = "bill_to" ) ]
			public BillTo BillTo { get; set; }

			[ DataMember( Name = "boxes") ]
			public List< TrueShipBox > Boxes { get; set; }

			[ DataMember( Name = "charges" ) ]
			public Charges Charges { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public DateTime CreatedAt { get; set; }

			[ DataMember( Name = "customer_number" ) ]
			public string CustomerNumber { get; set; }

			[ DataMember( Name = "future_ship_at" ) ]
			public DateTime FutureShipAt { get; set; }

			[ DataMember( Name = "imported_at" ) ]
			public string ImportedAt { get; set; }

			public string Message { get; set; }

			[ DataMember( Name = "numerical_id" ) ]
			public string NumericalId { get; set; }

			[ DataMember( Name = "order_time" ) ]
			public DateTime OrderTime { get; set; }

			[ DataMember( Name = "po_number" ) ]
			public string PoNumber { get; set; }

			[ DataMember( Name = "primary_id" ) ]
			public string PrimaryId { get; set; }

			[ DataMember( Name = "printed_at" ) ]
			public DateTime PrintedAt { get; set; }

			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }

			[ DataMember( Name = "revision_id" ) ]
			public int RevisionId { get; set; }

			[ DataMember( Name = "revisions_resource_uri" ) ]
			public string RevisionsResourceUri { get; set; }

			[ DataMember( Name = "ship_from" ) ]
			public ShipFrom ShipFrom { get; set; }

			[ DataMember( Name = "ship_time" ) ]
			public DateTime ShipTime { get; set; }

			[ DataMember( Name = "ship_to" ) ]
			public ShipTo ShipTo { get; set; }

			[ DataMember( Name = "ship_type" ) ]
			public string ShipType { get; set; }

			[ DataMember( Name = "ship_via" ) ]
			public string ShipVia { get; set; }

			public string Source { get; set; }
			public List< Source > Sources { get; set; }

			[ DataMember( Name = "status_shipped" ) ]
			public bool StatusShipped { get; set; }

			public string Terms { get; set; }

			[ DataMember( Name = "unique_id" ) ]
			public string UniqueId { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public DateTime UpdatedAt { get; set; }

			public string Warehouse { get; set; }
		}
	}
}