using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[ DataContract ]
	public class Meta
	{
		[ DataMember( Name = "limit" ) ]
		public int Limit { get; set; }

		/// <summary>
		///     Link to next collection
		///     Looks like -
		///     <example>/api/v1/resource/?bearer_token=token&limit=1&offset=4</example>
		/// </summary>
		[ DataMember( Name = "next" ) ] 
		public string Next { get; set; }

		[ DataMember( Name = "offset" ) ] 
		public int Offset { get; set; }

		/// <summary>
		///     Link to previous collection
		///     Looks like -
		///     <example>/api/v1/resource/?bearer_token=token&limit=1&offset=4</example>
		/// </summary>
		[ DataMember( Name = "previous" ) ] 
		public string Previous { get; set; }

		[ DataMember( Name = "total_count" ) ]
		public int TotalCount { get; set; }
	}
}