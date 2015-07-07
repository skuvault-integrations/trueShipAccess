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

		public class Response
		{
			public Meta Meta { get; set; }
			public List<Box> Objects { get; set; }
		}
	}
}