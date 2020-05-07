using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class TrueShipItem
	{
		[ DataMember( Name = "unit_price" ) ]
		public string UnitPriceValue { get; set; }
		public TrueShipMoney GetUnitPrice()
		{
			return UnitPriceValue.ToTrueShipMoney();
		}

		[ DataMember( Name = "kind" )]
		public string Kind { get; set; }
		public bool GetIsDiscount()
		{
			return Kind == "discount";
		}

		[ DataMember( Name = "part_number" ) ]
		public string PartNumber { get; set; }
		
		[ DataMember( Name = "pick_location" ) ]
		public string PickLocation { get; set; }

		[ DataMember( Name = "quantity" ) ]
		public int Quantity { get; set; }

		//TODO GUARD-506 If it's not needed to push locations, then delete it
		[ DataMember( Name = "url" ) ]
		public string Url { get; set; }
	}
}