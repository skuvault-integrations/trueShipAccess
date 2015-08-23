using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class ChargeUnit
	{
		[ DataMember( Name = "unit_price" ) ]
		public string UnitPrice { get; set; }
	}
}