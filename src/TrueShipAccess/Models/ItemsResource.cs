using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	public class ItemsResource
	{
		public string Box { get; set; }
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
		public int UnitWeight { get; set; }
		public string Url { get; set; }
	}
}