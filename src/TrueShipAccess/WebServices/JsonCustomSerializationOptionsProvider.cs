using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace TrueShipAccess.WebServices
{
	public static class JsonCustomSerializationOptionsProvider
	{
		public static DateTime CustomDateTimeDeserializer( string time )
		{
			try
			{
				return DateTime.ParseExact( time, "yyyy-MM-ddTHH:mm:sszzz",
					CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal );
			}
			catch ( Exception e )
			{
				return DateTime.Now;
			}
		}

		public static  void Setup() {
			JsConfig.Reset();
			JsConfig< DateTime >.DeSerializeFn = CustomDateTimeDeserializer;
			JsConfig.IncludeNullValues = true;
		}
	}
}
