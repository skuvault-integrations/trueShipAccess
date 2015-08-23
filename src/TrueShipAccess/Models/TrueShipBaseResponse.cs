using System.Collections.Generic;

namespace TrueShipAccess.Models
{
	public class TrueShipBaseResponse< T > where T : class
	{
		public Meta Meta { get; set; }
		public List< T > Objects { get; set; }
	}
}