using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// Retrieve a list of companies.
	/// https://www.readycloud.com/static/api-doc/apireference.html#company-resource
	/// </summary>
	public class CompanyResource
	{
		[DataContract]
		public class Company
		{
			[DataMember(Name = "company_id")]
			public string CompanyId { get; set; }
			[DataMember(Name = "created_at")]
			public string CreatedAt { get; set; }
			public int Id { get; set; }
			public string Name { get; set; }
			[DataMember(Name = "resource_uri")]
			public string ResourceUri { get; set; }
			[DataMember(Name = "updated_at")]
			public string UpdatedAt { get; set; }
		}

		public class Response : IEnumerable< Company >
		{
			public Meta Meta { get; set; }
			public List< Company > Objects { get; set; }

			public Response()
			{
				this.Objects = new List< Company >();
			}

			public IEnumerator< Company > GetEnumerator()
			{
				return this.Objects.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}