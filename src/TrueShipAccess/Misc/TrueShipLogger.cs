using System;
using System.IO;
using System.Net;
using Netco.Logging;

namespace TrueShipAccess.Misc
{
	public class TrueShipLogger
	{
		private string path = "C:\\Temp\\log.txt";

		public Boolean clearLogs()
		{
			var logDir = Path.GetDirectoryName( path );
			if( logDir != null && !Directory.Exists( logDir ) )
			{
				Directory.CreateDirectory( logDir );

				System.IO.FileStream stream = new System.IO.FileStream( path, FileMode.Truncate );
				System.IO.StreamWriter logger = new System.IO.StreamWriter( stream );
				logger.Write( "TrueShip Logging Begin @ {0}\r\n\r\n", DateTime.Now.ToString() );
				logger.Close();
			}

			return true;
		}

		public Boolean tsLogNoLineBreak( string message )
		{
			System.IO.StreamWriter logger = new System.IO.StreamWriter( path, true );
			string logmessage = string.Format( "{0}", message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public Boolean tsLogLineBreak( string message )
		{
			System.IO.StreamWriter logger = new System.IO.StreamWriter( path, true );
			string logmessage = string.Format( "{0}\r\n", message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public Boolean tsLogWebService( string endpoint, string response )
		{
			System.IO.StreamWriter logger = new System.IO.StreamWriter( path, true );
			string logmessage = string.Format( "[TrueShip]\tResponse\t{0}\r\n{1}\r\n", endpoint, response );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public Boolean tsLogWebServiceError( WebException e, Uri uri )
		{
			System.IO.StreamWriter logger = new System.IO.StreamWriter( path, true );
			string logmessage = string.Format( "[TrueShip]\t{0}\t{1}\r\n{2}\r\n{3}\r\n",
				"API Call Failed!",
				uri.ToString(),
				e.Status,
				e.Message );
			logger.WriteLine( logmessage );
			logger.Close();
			return true;
		}

		public static ILogger Log()
		{
			return NetcoLogger.GetLogger( "TrueShipLogger" );
		}

		public static void LogTraceStarted( string info )
		{
			Log().Trace( "[trueship] Start call:{0}.", info );
		}

		public static void LogTraceEnded( string info )
		{
			Log().Trace( "[trueship] End call:{0}.", info );
		}

		public static void LogTraceInnerStarted( string info )
		{
			Log().Trace( "[trueship] Internal Start call:{0}.", info );
		}

		public static void LogTraceInnerEnded( string info )
		{
			Log().Trace( "[trueship] Internal End call:{0}.", info );
		}

		public static void LogTraceInnerError( string info )
		{
			Log().Trace( "[trueship] Internal error:{0}.", info );
		}
	}
}