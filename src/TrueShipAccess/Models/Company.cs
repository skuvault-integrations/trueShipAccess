using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	[DataContract]
	public class Company
	{
		public int Id { get; set; }

		[DataMember(Name = "company_id")]
		public string CompanyId { get; set; }

		public string Name { get; set; }

		[DataMember(Name = "created_at")]
		public string CreatedAt { get; set; }

		[DataMember(Name = "resource_uri")]
		public string ResourceUri { get; set; }
		[DataMember(Name = "updated_at")]
		public string UpdatedAt { get; set; }
	}
}