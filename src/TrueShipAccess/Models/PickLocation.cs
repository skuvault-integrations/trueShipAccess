using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class PickLocation
	{
		[ DataMember( Name = "pick_location" ) ]
		public string Location { get; set; }
	}
}