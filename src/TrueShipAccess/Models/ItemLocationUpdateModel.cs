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
		public readonly string Sku; // used only for logging info

		public ItemLocationUpdateModel( string itemResouurce, string sku, PickLocation location )
		{
			this.Resource = itemResouurce;
			this.Location = location;
			this.Sku = sku;
		}

		public TrueShipApiEndpoint GetEndPoint()
		{
			return new TrueShipApiEndpoint( this.Resource );
		}
	}
}
