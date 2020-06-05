using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	public class TrueShipBaseResponse< T > where T : class
	{
		[ DataMember( Name = "count" ) ]
		public int Count { get; set; }

		[ DataMember( Name = "next" ) ] 
		public string Next { get; set; }

		[ DataMember( Name = "previous" ) ] 
		public string Previous { get; set; }

		[ DataMember( Name = "results" )]
		public IEnumerable< T > Results { get; set; }
	}
}