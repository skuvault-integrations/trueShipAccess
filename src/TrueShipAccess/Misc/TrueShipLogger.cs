using Netco.Extensions;
using Netco.Logging;
using System;
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
			var info = "Processed {0} pages of {1}".FormatWith( pageNumber, ( decimal )response.Count );
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

		public static string CreateMethodCallInfo( Uri uri, Mark mark = null, string errors = "", string methodResult = "", string additionalInfo = "", string payload = "", [ CallerMemberName ] string memberName = "" )
		{
			string serviceEndPoint = null;
			string requestParameters = null;

			if ( uri != null )
			{
				serviceEndPoint = uri.LocalPath;
				requestParameters = uri.Query;
			}

			var str = string.Format(
				"{{\"MethodName\": \"{0}\", \"Mark\": \"{1}\", \"ServiceEndPoint\": \"{2}\" {3} {4}{5}{6}{7}}}",
				memberName,
				mark ?? Mark.Blank(),
				string.IsNullOrWhiteSpace( serviceEndPoint ) ? string.Empty : serviceEndPoint,
				string.IsNullOrWhiteSpace( requestParameters ) ? string.Empty : ", \"RequestParameters\": " + requestParameters,
				string.IsNullOrWhiteSpace( errors ) ? string.Empty : ", \"Errors\":" + errors,
				string.IsNullOrWhiteSpace( methodResult ) ? string.Empty : ", \"Result\":" + methodResult,
				string.IsNullOrWhiteSpace( additionalInfo ) ? string.Empty : ", \"AdditionalInfo\": " + additionalInfo,
				string.IsNullOrWhiteSpace( payload ) ? string.Empty : ", \"Payload\": " + payload
			);
			return str;
		}
	}
}