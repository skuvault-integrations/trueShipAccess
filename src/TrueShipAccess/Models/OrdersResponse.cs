using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	public class BillTo
	{
		public string resource_uri { get; set; }
	}

	public class CustomField
	{
		public string Box { get; set; }
		[DataMember(Name = "created_at")]
		public string CreatedAt { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		[DataMember(Name = "resource_uri")]
		public string resource_uri { get; set; }
		[DataMember(Name = "updated_at")]
		public string UpdatedAt { get; set; }
		public string Value { get; set; }
	}

	public class Charges2
	{
		public object unit_price { get; set; }
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
		public string created_at { get; set; }
		public object Name { get; set; }
		public string Order { get; set; }
		public object order_source_id { get; set; }
		public string resource_uri { get; set; }
		public string retrieved_at { get; set; }
		public string updated_at { get; set; }
	}

	public class TrueShipOrderResource
	{
		public BillTo bill_to { get; set; }
		public List<Box> Boxes { get; set; }
		public Charges Charges { get; set; }
		public string created_at { get; set; }
		public List<object> custom_fields { get; set; }
		public object customer_number { get; set; }
		public object future_ship_at { get; set; }
		public string imported_at { get; set; }
		public string Message { get; set; }
		public string numerical_id { get; set; }
		public string order_time { get; set; }
		public object po_number { get; set; }
		public string primary_id { get; set; }
		public object printed_at { get; set; }
		public string resource_uri { get; set; }
		public int revision_id { get; set; }
		public string revisions_resource_uri { get; set; }
		public ShipFrom ship_from { get; set; }
		public object ship_time { get; set; }
		public ShipTo ship_to { get; set; }
		public string ship_type { get; set; }
		public string ship_via { get; set; }
		public object Source { get; set; }
		public List<Source> Sources { get; set; }
		public bool status_shipped { get; set; }
		public object Terms { get; set; }
		public string unique_id { get; set; }
		public string Updated_At { get; set; }
		public object Warehouse { get; set; }
	}

	public class OrdersResponse
	{
		public Meta Meta { get; set; }
		public IEnumerable<TrueShipOrderResource> Objects { get; set; }
	}
}