using System;

namespace TrueShipAccess.Models.Conventions
{
	abstract class TrueShipConventions
	{
		public static readonly string ServiceBaseUri = "https://www.readycloud.com";
		public static readonly string ApiBaseUri = ServiceBaseUri + "/api/v2";
		public static readonly string DefaultFormat = "JSON";

		public static Uri GetNextPaginationUri( string next )
		{
			if( string.IsNullOrWhiteSpace( next ) )
				return null;
			return new Uri( string.Format( "{0}{1}", ServiceBaseUri, next ) );  
		}
	}
}
