using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	public class Meta
	{
		public int Limit { get; set; }
		public object Next { get; set; }
		public int Offset { get; set; }
		public object Previous { get; set; }

		[ DataMember( Name = "total_count" ) ]
		public int TotalCount { get; set; }
	}
}