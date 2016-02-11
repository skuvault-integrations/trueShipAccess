using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Netco.Extensions;
using TrueShipAccess.Models;

namespace TrueShipAccess
{
	public class MethodLogging
	{
		protected string GetLogPrefix( TrueShipConfiguration config, string addtionalInfo, [ CallerMemberName ] string methodName = "" )
		{
			return "{0} ({1}), credentials: {2}".FormatWith( methodName, addtionalInfo, config.Credentials.AccessToken );
		}
	}
}
