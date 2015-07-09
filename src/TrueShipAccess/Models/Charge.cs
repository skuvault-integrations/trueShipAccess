using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	public class Charge
	{
		[ DataMember( Name = "unit_price" ) ]
		public object UnitPrice { get; set; }
	}
}