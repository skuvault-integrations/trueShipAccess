using System;

namespace TrueShipAccess.Models.Conventions
{
	abstract class TrueShipConventions
	{
		public static readonly string ServiceBaseUri = "https://www.readycloud.com";
		public static readonly string ApiBaseUri = ServiceBaseUri + "/api/v2";
		public static readonly string DefaultFormat = "JSON";

		public static Uri GetNextPaginationUri( Meta meta )
		{
			return meta.Next != null ? new Uri( string.Format( "{0}{1}", ServiceBaseUri, meta.Next ) ) : null;  
		}
	}
}
