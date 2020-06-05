using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class OrderNote
	{
		[ DataMember( Name = "content" ) ]
		public string Content { get; set; }
	}
}