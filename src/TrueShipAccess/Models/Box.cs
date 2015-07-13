using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[DataContract]
	public class Box
	{
		public BoxesResource.Charges Charges { get; set; }

		[ DataMember( Name = "custom_fields" ) ]
		public List< OrdersResource.CustomField > CustomFields { get; set; }

		[ DataMember( Name = "delivery_confirmation" ) ]
		public object DeliveryConfirmation { get; set; }

		[ DataMember( Name = "general_description" ) ]
		public string GeneralDescription { get; set; }

		public string Height { get; set; }
		public List< Item > Items { get; set; }
		public string Length { get; set; }
		public string Order { get; set; }

		[ DataMember( Name = "resource_uri" ) ]
		public string ResourceUri { get; set; }

		[ DataMember( Name = "saturday_delivery" ) ]
		public bool SaturdayDelivery { get; set; }

		[ DataMember( Name = "shipper_release" ) ]
		public bool ShipperRelease { get; set; }

		[ DataMember( Name = "tracking_number" ) ]
		public object TrackingNumber { get; set; }

		public string Weight { get; set; }
		public string Width { get; set; }
	}
}