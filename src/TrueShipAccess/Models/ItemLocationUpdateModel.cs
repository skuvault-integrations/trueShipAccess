using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueShipAccess.Models.Conventions;

namespace TrueShipAccess.Models
{
	public class ItemLocationUpdateModel
	{
		public readonly string Resource;
		public readonly PickLocation Location;

		public ItemLocationUpdateModel( string itemResouurce, PickLocation location )
		{
			this.Resource = itemResouurce;
			this.Location = location;
		}

		public TrueShipApiEndpoint GetEndPoint()
		{
			return new TrueShipApiEndpoint( this.Resource );
		}
	}
}
