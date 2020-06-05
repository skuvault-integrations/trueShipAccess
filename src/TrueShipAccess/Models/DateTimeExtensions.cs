using System;

namespace TrueShipAccess.Models
{
	public static class DateTimeExtensions
	{
		public static DateTime ToUtcDateTime( this string dateTime )
		{
			if ( string.IsNullOrWhiteSpace( dateTime ) )
				return DateTime.MinValue;
			return DateTime.Parse( dateTime ).ToUniversalTime();
		}
	}
}
