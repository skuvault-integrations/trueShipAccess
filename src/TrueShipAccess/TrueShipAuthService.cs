using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netco.Extensions;

namespace TrueShipAccess
{
	class TrueShipAuthService : ITrueShipAuthService
	{
		private readonly string _appId;

		public TrueShipAuthService( string appId )
		{
			this._appId = appId;
		}

		public string GetAuthUri()
		{
			return "https://www.readycloud.com/api/v1/oauth2/authorize?scope=order%20account%20company%20packaging&response_type=token&client_id={0}".FormatWith( this._appId );
		}
	}
}
