using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// Retrieve a list of boxes.
	/// https://www.readycloud.com/static/api-doc/apireference.html#box-resource
	/// </summary>
	public class BoxesResource
	{
		public class Charges
		{
			[DataMember(Name = "actual_ship_cost")]
			public object ActualShipCost { get; set; }
			[DataMember(Name = "declared_value")]
			public object DeclaredValue { get; set; }
			[DataMember(Name = "insured_value")]
			public object InsuredValue { get; set; }
		}
	}

	public class Item
	{
		public Charge Charges { get; set; }
		[DataMember(Name = "commodity_code")]
		public string CommodityCode { get; set; }
		public string Description { get; set; }
		[DataMember(Name = "export_type_code")]
		public string ExportTypeCode { get; set; }
		[DataMember(Name = "item_id")]
		public string ItemId { get; set; }
		[DataMember(Name = "joint_production")]
		public bool JointProduction { get; set; }
		[DataMember(Name = "net_cost_begin_date")]
		public object NetCostBeginDate { get; set; }
		[DataMember(Name = "net_cost_code")]
		public string NetCostCode { get; set; }
		[DataMember(Name = "net_cost_end_date")]
		public object NetCostEndDate { get; set; }
		[DataMember(Name = "origin_country_code")]
		public string OriginCountryCode { get; set; }
		[DataMember(Name = "part_number")]
		public string PartNumber { get; set; }
		[DataMember(Name = "pick_location")]
		public string PickLocation { get; set; }
		[DataMember(Name = "preference_criteria_code")]
		public string PreferenceCriteriaCode { get; set; }
		[DataMember(Name = "producer_info_code")]
		public string ProducerInfoCode { get; set; }
		public int Quantity { get; set; }
		[DataMember(Name = "resource_uri")]
		public string ResourceUri { get; set; }
		[DataMember(Name = "scheduleb_commodity_code")]
		public string SchedulebCommodityCode { get; set; }
		[DataMember(Name = "unit_weight")]
		public string UnitWeight { get; set; }
		public string Url { get; set; }
	}

	public class Box
	{
		public Charges Charges { get; set; }
		[DataMember(Name = "custom_fields")]
		public List<CustomField> CustomFields { get; set; }
		[DataMember(Name = "delivery_confirmation")]
		public object DeliveryConfirmation { get; set; }
		[DataMember(Name = "general_description")]
		public string GeneralDescription { get; set; }
		public string Height { get; set; }
		public List<Item> Items { get; set; }
		public string Length { get; set; }
		public string Order { get; set; }
		[DataMember(Name = "resource_uri")]
		public string ResourceUri { get; set; }
		[DataMember(Name = "saturday_delivery")]
		public bool SaturdayDelivery { get; set; }
		[DataMember(Name = "shipper_release")]
		public bool ShipperRelease { get; set; }
		[DataMember(Name = "tracking_number")]
		public object TrackingNumber { get; set; }
		public string Weight { get; set; }
		public string Width { get; set; }
	}

	public class BoxesResponse
	{
		public Meta Meta { get; set; }
		public List<Box> Objects { get; set; }
	}
}