using System;
using System.Globalization;
using System.IO;
using System.Net;
using Netco.Extensions;
using Netco.Logging;
using TrueShipAccess.Models;

namespace TrueShipAccess.Misc
{
	public class TrueShipLogger
	{
		private readonly string path = "C:\\Temp\\log.txt";

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
			var info = "Processed {0} pages of {1}".FormatWith( pageNumber, ( decimal )response.Meta.TotalCount );
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

		public bool clearLogs()
		{
			var logDir = Path.GetDirectoryName( this.path );
			if( logDir != null && !Directory.Exists( logDir ) )
			{
				Directory.CreateDirectory( logDir );

				var stream = new FileStream( this.path, FileMode.Truncate );
				var logger = new StreamWriter( stream );
				logger.Write( "TrueShip Logging Begin @ {0}\r\n\r\n", DateTime.Now.ToString( CultureInfo.InvariantCulture ) );
				logger.Close();
			}

			return true;
		}

		public bool tsLogLineBreak( string message )
		{
			var logger = new StreamWriter( this.path, true );
			var logmessage = string.Format( "{0}\r\n", message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public bool tsLogNoLineBreak( string message )
		{
			var logger = new StreamWriter( this.path, true );
			var logmessage = string.Format( "{0}", message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public bool tsLogWebService( string endpoint, string response )
		{
			var logger = new StreamWriter( this.path, true );
			var logmessage = string.Format( "[TrueShip]\tResponse\t{0}\r\n{1}\r\n", endpoint, response );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public bool tsLogWebServiceError( WebException e, Uri uri )
		{
			var logger = new StreamWriter( this.path, true );
			var logmessage = string.Format( "[TrueShip]\t{0}\t{1}\r\n{2}\r\n{3}\r\n",
				"API Call Failed!",
				uri,
				e.Status,
				e.Message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}
	}
}