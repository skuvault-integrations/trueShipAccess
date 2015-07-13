using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[DataContract]
	public class Charge
	{
		[ DataMember( Name = "unit_price" ) ]
		public object UnitPrice { get; set; }
	}
}