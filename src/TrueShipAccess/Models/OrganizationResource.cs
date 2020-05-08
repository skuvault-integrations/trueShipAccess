using System;
using System.Linq;
using System.Runtime.Serialization;

namespace TrueShipAccess.Models
{
	/// <summary>
	/// https://www.readycloud.com/static/api-doc/v2/02-apireference-v2-01-organizations.html#organizations-collection
	/// </summary>
	public class OrganizationResource : TrueShipBaseResponse< OrganizationResource.TrueShipOrganization >
	{
		[ DataContract ]
		public class TrueShipOrganization
		{
			[ DataMember( Name = "name" ) ]
			public string Name { get; set; }

			[ DataMember( Name = "url" ) ]
			public string Url { get; set; }
			public string OrganizationKey
			{
				get
				{
					if ( Url == null )
						return string.Empty;
					var urlParts = Url.Split( new [] { '/' }, StringSplitOptions.RemoveEmptyEntries );
					return urlParts.LastOrDefault();
				}
			}

			[ DataMember( Name = "deleted_at" ) ]
			public string DeletedAtValue { get; set; }
			public bool IsDeleted
			{
				get
				{
					var deletedAt = DeletedAtValue.ToUtcDateTime();
					return deletedAt != DateTime.MinValue && deletedAt < DateTime.UtcNow;
				}
			}
		}
	}
}
