using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[DataContract]
	public class Meta
	{
		public int Limit { get; set; }
		/// <summary>
		/// Link to next collection
		/// Looks like - <example>/api/v1/resource/?bearer_token=token&limit=1&offset=4</example>
		/// </summary>
		public string Next { get; set; }
		public int Offset { get; set; }
		/// <summary>
		/// Link to previous collection
		/// Looks like - <example>/api/v1/resource/?bearer_token=token&limit=1&offset=4</example>
		/// </summary>
		public string Previous { get; set; }

		[ DataMember( Name = "total_count" ) ]
		public int TotalCount { get; set; }
	}
}