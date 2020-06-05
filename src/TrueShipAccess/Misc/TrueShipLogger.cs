using Netco.Extensions;
using Netco.Logging;
using Newtonsoft.Json.Linq;
using ServiceStack;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using TrueShipAccess.Models;

namespace TrueShipAccess.Misc
{
	public class TrueShipLogger
	{
		public static ILogger Log()
		{
			return NetcoLogger.GetLogger( "TrueShipLogger" );
		}

		public void LogTrace( string prefix, string info )
		{
			Log().Trace( "[trueship] {0}. {1}", prefix, info );
		}

		public void LogTracePagination< T >( string prefix, int pageNumber, TrueShipBaseResponse< T > response ) where T : class
		{
			var info = string.Format( "Processed {0} pages of {1}", pageNumber, ( decimal )response.Count );
			Log().Trace( "[trueship] {0}. {1}", prefix, info );
		}

		public static void LogTraceEnded( string info )
		{
			Log().Trace( "[trueship] End call:{0}.", info );
		}

		public static void LogTraceInnerEnded( string info )
		{
			Log().Trace( "[trueship] Internal End call:{0}.", info );
		}

		public static void LogTraceInnerError( string info )
		{
			Log().Trace( "[trueship] Internal error:{0}.", info );
		}

		public static void LogTraceInnerStarted( string info )
		{
			Log().Trace( "[trueship] Internal Start call:{0}.", info );
		}

		public static void LogTraceStarted( string info )
		{
			Log().Trace( "[trueship] Start call:{0}.", info );
		}

		public static string CreateMethodCallInfo( Uri uri, Mark mark = null, HttpMethod methodType = null, string errors = "", string responseBodyRaw = "", string additionalInfo = "", string payload = "", [ CallerMemberName ] string libMethodName = "" )
		{
			JObject responseBody = null;
			try
			{
				responseBody = JObject.Parse( responseBodyRaw );
			}
			catch { }

			return new CallInfo()
			{
				Mark = mark != null ? mark.ToString() : "Unknown",
				Endpoint = uri.AbsoluteUri,
				Method = methodType != null ? methodType.ToString() : "Uknown",
				Body = payload,
				LibMethodName = libMethodName,
				AdditionalInfo = additionalInfo,
				Response = (object)responseBody ?? responseBodyRaw,
				Errors = errors
			}.ToJson();
		}
	}
}