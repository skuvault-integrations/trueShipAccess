using System;
using System.Runtime.CompilerServices;

namespace TrueShipAccess
{
	public class TrueShipException : Exception
	{
		protected TrueShipException( string message, Exception exception )
			: base( message, exception )
		{
		}

		protected TrueShipException( string message )
			: base( message )
		{
		}
	}

	public class TrueShipAuthException : TrueShipException
	{
		public TrueShipAuthException( string message, Exception exception, [ CallerMemberName ] string memberName = "" )
			: base( string.Format( "{0}:{1}", memberName, message ), exception )
		{
		}
	}

	public class TrueShipCommonException : TrueShipException
	{
		public TrueShipCommonException( string message, Exception exception, [ CallerMemberName ] string memberName = "" )
			: base( string.Format( "{0}:{1}", memberName, message ), exception )
		{
		}

		public TrueShipCommonException( string message, [ CallerMemberName ] string memberName = "" )
			: base( string.Format( "{0}:{1}", memberName, message ) )
		{
		}
	}
}