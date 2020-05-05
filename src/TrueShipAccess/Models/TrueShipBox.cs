using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class TrueShipBox
	{
		[ DataMember( Name = "items" ) ]
		public List< TrueShipItem > Items { get; set; }

		[ DataMember( Name = "url" ) ]
		public string Url { get; set; }

		[ DataMember( Name = "tracking_number" ) ]
		public string TrackingNumber { get; set; }
	}
}