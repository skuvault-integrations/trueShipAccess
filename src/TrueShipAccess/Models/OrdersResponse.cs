using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// Retrieve a list of orders.
	/// https://www.readycloud.com/static/api-doc/apireference.html#order-resource
	/// </summary>
	public class OrdersResource
	{
		public class BillTo
		{
			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }
		}

		public class CustomField
		{
			public int Id { get; set; }
			public string Box { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public string CreatedAt { get; set; }

			public string Name { get; set; }

			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }

			public string Value { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public string UpdatedAt { get; set; }
		}

		public class Charges
		{
			public object actual_shipcost { get; set; }
			public object calculated_grand_total { get; set; }
			public object calculated_shipping { get; set; }
			public object calculated_tax { get; set; }
			public object calculated_total { get; set; }
			public object grand_total { get; set; }
			public object grand_total_source { get; set; }
			public object imported_grand_total { get; set; }
			public object imported_shipping { get; set; }
			public object imported_tax { get; set; }
			public object imported_total { get; set; }
			public object shipping { get; set; }
			public object shipping_source { get; set; }
			public object Tax { get; set; }
			public object tax_source { get; set; }
			public object Total { get; set; }
			public object total_source { get; set; }
		}

		public class ShipFrom
		{
			public string a_type { get; set; }
			public string address_1 { get; set; }
			public object address_2 { get; set; }
			public string City { get; set; }
			public string Company { get; set; }
			public string Country { get; set; }
			public string Email { get; set; }
			public string first_name { get; set; }
			public string last_name { get; set; }
			public string Order { get; set; }
			public string Phone { get; set; }
			public string post_code { get; set; }
			public bool residential { get; set; }
			public string resource_uri { get; set; }
			public string State { get; set; }
			public bool Validated { get; set; }
		}

		public class ShipTo
		{
			public string a_type { get; set; }
			public string address_1 { get; set; }
			public object address_2 { get; set; }
			public string City { get; set; }
			public string Company { get; set; }
			public string Country { get; set; }
			public string Email { get; set; }
			public string first_name { get; set; }
			public string last_name { get; set; }
			public string Order { get; set; }
			public string Phone { get; set; }
			public string post_code { get; set; }
			public bool residential { get; set; }
			public string resource_uri { get; set; }
			public string State { get; set; }
			public bool Validated { get; set; }
		}

		public class Source
		{
			public object Account { get; set; }
			public object Channel { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public string created_at { get; set; }

			public object Name { get; set; }
			public string Order { get; set; }

			/// <summary>
			/// order_source_id
			/// </summary>
			[ DataMember( Name = "order_source_id" ) ]
			public object OrderSourceId { get; set; }

			public string ResourceUri { get; set; }

			[ DataMember( Name = "retrieved_at" ) ]
			public string RetrievedAt { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public string UpdatedAt { get; set; }
		}

		public class Order
		{
			[ DataMember( Name = "bill_to" ) ]
			public BillTo BillTo { get; set; }

			public List< Box > Boxes { get; set; }
			public Charges Charges { get; set; }

			[ DataMember( Name = "created_at" ) ]
			public string CreatedAt { get; set; }

			[ DataMember( Name = "custom_fields" ) ]
			public List< object > CustomFields { get; set; }

			[ DataMember( Name = "customer_number" ) ]
			public object CustomerNumber { get; set; }

			[ DataMember( Name = "future_ship_at" ) ]
			public object FutureShipAt { get; set; }

			[ DataMember( Name = "imported_at" ) ]
			public string ImportedAt { get; set; }

			public string Message { get; set; }

			[ DataMember( Name = "numerical_id" ) ]
			public string NumericalId { get; set; }

			[ DataMember( Name = "order_time" ) ]
			public string OrderTime { get; set; }

			[ DataMember( Name = "po_number" ) ]
			public object PoNumber { get; set; }

			[ DataMember( Name = "primary_id" ) ]
			public string PrimaryId { get; set; }

			[ DataMember( Name = "printed_at" ) ]
			public object PrintedAt { get; set; }

			[ DataMember( Name = "resource_uri" ) ]
			public string ResourceUri { get; set; }

			[ DataMember( Name = "revision_id" ) ]
			public int RevisionId { get; set; }

			[ DataMember( Name = "revisions_resource_uri" ) ]
			public string RevisionsResourceUri { get; set; }

			[ DataMember( Name = "ship_from" ) ]
			public ShipFrom ShipFrom { get; set; }

			[ DataMember( Name = "ship_time" ) ]
			public object ShipTime { get; set; }

			[ DataMember( Name = "ship_to" ) ]
			public ShipTo ShipTo { get; set; }

			[ DataMember( Name = "ship_type" ) ]
			public string ShipType { get; set; }

			[ DataMember( Name = "ship_via" ) ]
			public string ShipVia { get; set; }

			public object Source { get; set; }
			public List< Source > Sources { get; set; }

			[ DataMember( Name = "status_shipped" ) ]
			public bool StatusShipped { get; set; }

			public object Terms { get; set; }

			[ DataMember( Name = "unique_id" ) ]
			public string UniqueId { get; set; }

			[ DataMember( Name = "updated_at" ) ]
			public string UpdatedAt { get; set; }

			public object Warehouse { get; set; }
		}

		public class Response
		{
			public Meta Meta { get; set; }
			public IEnumerable< Order > Objects { get; set; }
		}
	}
}